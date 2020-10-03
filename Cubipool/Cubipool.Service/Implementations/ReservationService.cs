using System;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Common;
using System.Collections.Generic;
using System.Linq;
using Cubipool.Common.Constants;
using Cubipool.Entity;
using Cubipool.Repository;
using Cubipool.Repository.Abstractions;
using Cubipool.Common.Exceptions;
using Cubipool.Repository.Implementations;
using Cubipool.Service.Dtos.Reservations;
using Cubipool.Service.Dtos.Reservations;


namespace Cubipool.Service.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICubicleRepository _cubicleRepository;
        private readonly IConstantsRepository _constantsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserReservationRepository _userReservationRepository;
        private readonly IRequestRepository _requestRepository;


        public ReservationService()
        {
            
        }

        public ReservationService(
            IReservationRepository reservationRepository,
            ICubicleRepository cubicleRepository,
            IConstantsRepository constantsRepository,
            IUserRepository userRepository,
            IUserReservationRepository userReservationRepository,
            IRequestRepository requestRepository
        )
        {
            _reservationRepository = reservationRepository;
            _cubicleRepository = cubicleRepository;
            _constantsRepository = constantsRepository;
            _userRepository = userRepository;
            _userReservationRepository = userReservationRepository;
            _requestRepository = requestRepository;
        }


        public async Task<ReservationResponseDto> ReservationAsync(ReservationDto reservationDTO)
        {
            if (reservationDTO.EndTime < reservationDTO.StartTime)
            {
                throw new BadRequestException($"No puede terminar la reserva antes de empezar");
            }

            if (reservationDTO.EndTime.Minute > 0 || reservationDTO.StartTime.Minute > 0)
            {
                throw new BadRequestException($"La reserva se debe realizar en horas completas");
            }

            if (reservationDTO.StartTime.Hour < 7 || reservationDTO.StartTime.Hour >= 23 ||
                reservationDTO.EndTime.Hour > 23 || reservationDTO.EndTime.Hour <= 7)
            {
                throw new BadRequestException($"Los cubículos son solo de 7:00 am a 11:00 pm");
            }

            if (reservationDTO.StartTime.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new BadRequestException($"Los cubículos no estan disponibles los domingos.");
            }

            TimeSpan validate24Hrs = DateTime.Now.Subtract(reservationDTO.EndTime);
            if (validate24Hrs.Days >= 1)
            {
                throw new BadRequestException($"Solo se puede reservar con 24 hrs de anticipación.");
            }
/*
            if (DateTime.Now > reservationDTO.StartTime && (validate24Hrs.Hours >= 1 || validate24Hrs.Minutes > 15))
            {
                throw new BadRequestException(
                    $"Solo se puede reservar cubículos hasta 15 minutos después de su inicio.");
            }*/

            TimeSpan rest = reservationDTO.EndTime.Subtract(reservationDTO.StartTime);
            if ((rest.Hours <= 0 || rest.Hours > 2))
            {
                throw new BadRequestException($"Máximo 2 horas de reserva.");
            }

            ICollection<Reservation> reservations = await _reservationRepository.FindByCubicleAndTimeIntervalAsync(
                reservationDTO.cubicleID,
                reservationDTO.StartTime, reservationDTO.EndTime);

            if (reservations.Count > 0)
            {
                throw new BadRequestException($"El cubículo esta ocupado");
            }

            ICollection<UserReservation> userReservations =
                await _userReservationRepository.ActiveReserveByUser(reservationDTO.hostID);

            if (userReservations.Count > 0)
            {
                throw new BadRequestException($"Ya tiene una reserva activa");
            }

            DateTime day = new DateTime(reservationDTO.StartTime.Year, reservationDTO.StartTime.Month,
                reservationDTO.StartTime.Day);
            userReservations =
                await _userReservationRepository.ReserveUserAndTimeInterval(reservationDTO.hostID, day, day.AddDays(1));

            int hours = 0;
            foreach (var item in userReservations)
            {
                TimeSpan duration = item.Reservation.EndTime.Subtract(item.Reservation.StartTime);
                hours += duration.Hours;
            }

            if (hours > 2)
            {
                throw new BadRequestException($"Ya has usado tus horas máxima de reservas en el día");
            }

            var cubicle = await _cubicleRepository.FindOneByIdAsync(reservationDTO.cubicleID);
            var user = await _userRepository.FindOneByIdAsync(reservationDTO.hostID);

            if (cubicle == null)
            {
                throw new BadRequestException($"No existe ese cubículo");
            }

            if (user == null)
            {
                throw new BadRequestException($"No existe ese usuario");
            }

            var userReservation = new UserReservation
            {
                UserRoleId = UserReservationRoles.Host,
                User = user,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            Reservation reservation = new Reservation
            {
                UserReservations = new List<UserReservation>(),
                StartTime = reservationDTO.StartTime,
                ReservationStateId = ReservationStates.NotActive,
                EndTime = reservationDTO.EndTime,
                HostLeaveTime = reservationDTO.EndTime,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            reservation.UserReservations.Add(userReservation);


            cubicle.Reservations.Add(reservation);
            await _cubicleRepository.UpdateAsync(cubicle);
      
            var response = new ReservationResponseDto
            {
               Id = cubicle.Reservations.Last().Id,
                CubicleCode = cubicle.Code,
                CubicleDescription = cubicle.Description,
                CubicleTotalSeats = cubicle.TotalSeats,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime
            };

            return response;
        }


        public async Task<ReservationResponseDto> StopSharingReservationWithId(int id)
        {
            // Validando la existencia de la reserva
            var reservation = await _reservationRepository.FindOneByIdAsync(id);
            if (reservation == null)
                throw new NotFoundException($"Reservation with id {id} was not found");


            // Validando que el estado de la reserva sea compartido
            if (reservation.ReservationStateId != ReservationStates.Shared)
                throw new BadRequestException("Reservation state must be 'Shared' in order to stop sharing");


            // Se cambia el estado de la reserva a activo (no compartido)
            reservation.ReservationStateId = ReservationStates.Active;
            reservation.UpdatedAt = DateTime.Now;
            await _reservationRepository.UpdateOneAsync(reservation);


            // Actualizando el estado de los requests de 'waiting' a 'denied'
            var requests = await _requestRepository.GetAllWaitingRequestsByReservationId(reservation.Id);
            foreach (var request in requests)
            {
                request.ConstantId = RequestStatus.Denied;
                request.UpdatedAt = DateTime.Now;
                await _requestRepository.UpdateOneAsync(request);
            }

            var cubicle = await _cubicleRepository.FindOneByIdAsync(reservation.CubicleId);

            return new ReservationResponseDto
            {
                CubicleCode = cubicle.Code,
                CubicleDescription = cubicle.Description,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                CubicleTotalSeats = cubicle.TotalSeats
            };
        }


        public async Task<Reservation> CreateOne(Reservation reservation)
        {
            return await _reservationRepository.CreateOneAsync(reservation);
        }

        public async Task<Reservation> Delete(Reservation reservation)
        {
            return await _reservationRepository.DeleteOneAsync(reservation);
        }
        
        public async Task<Reservation> FindOneById(Reservation reservation)
        {
            return await _reservationRepository.FindOneByIdAsync(reservation.Id);
        }
        

        public async Task<IEnumerable<ReservationResponseDto>> GetAllNonActiveReservationsByUserIdAsync(int id)
        {
            var user = await _userRepository.FindOneByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with id {id} was not found");

            var reservations = await _reservationRepository.GetAllNonActiveReservationsByUserIdAsync(id);
            //Console.WriteLine($"This is reservation name: {reservations.ElementAt(0).}");
            var response = reservations.Select(x => new ReservationResponseDto
            {
                Id = x.Id,
                CubicleId = x.CubicleId,
                CubicleCode = x.Cubicle.Code,
                CubicleDescription = x.Cubicle.Description,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                CubicleTotalSeats = x.Cubicle.TotalSeats,
                Status = x.ReservationState.Name
            });

            return response;
        }

        public async Task<IEnumerable<ReservationResponseDto>> GetAllCompletedReservationsByUserIdAsync(int id)
        {
            var user = await _userRepository.FindOneByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with id {id} was not found");

            var reservations = await _reservationRepository.GetAllCompletedReservationsByUserIdAsync(id);

            var response = reservations.Select(x => new ReservationResponseDto
            {
                CubicleCode = x.Cubicle.Code,
                CubicleDescription = x.Cubicle.Description,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                CubicleTotalSeats = x.Cubicle.TotalSeats,
                Status = x.ReservationState.Name
            });

            return response;
        }


        public async Task<ReservationResponseDto> ActiveReservation(ActiveReservationDto activeReservationDto)
        {
            // Se valida que exista la reserva
            var reservation = await _reservationRepository.FindOneByIdAsync(activeReservationDto.ReservationId);
            if (reservation == null)
                throw new BadRequestException(
                    $"Reservation with id {activeReservationDto.ReservationId} was not found");


            // Se valida que la reserva no pueda ser activada por segunda vez
            if (reservation.ReservationStateId == ReservationStates.Active)
                throw new BadRequestException($"Reservation with id {reservation.Id} is already active");

            // Se valida que el activador exista
            var activator = await _userRepository.FindOneByStudentCodeAsync(activeReservationDto.ActivatorCode);
            if (activator == null)
                throw new BadRequestException($"User with id {activeReservationDto.ActivatorCode} was not found");

            /* TODO: Se debe preguntar a los bibliotecólogos cuantos
			 cubiculos puede activar una persona por día [activador] */


            // Se valida que el activador no se encuentre en otra reserva 
            var temp = reservation.StartTime;
            var startTime = new DateTime(temp.Year, temp.Month, temp.Day);
            var isActivatorInAnotherReservation = await _userReservationRepository.IsActivatorInReservation(
                activator.Id, startTime, startTime.AddDays(1));
            if (isActivatorInAnotherReservation)
                throw new BadRequestException($"User with id {activator.Id} has already active in another reservation");


            // Se valida que el activador no sea el mismo host
            var host = await _userReservationRepository.GetHostByReservationId(reservation.Id);
            if (host == null)
                throw new BadRequestException("Host must be present in reservation");
            if (host.Id == activator.Id)
                throw new BadRequestException("Host cannot be activator");


            // TODO: Agregar timer para que se cambie el estado de la reserva
            // y agregar validacion por estado


            // Se valida que la reserva no se pueda activar hasta su hora de inicio
            // TODO: uncomment
            // if (DateTime.Now < reservation.StartTime)
            //     throw new BadRequestException($"Reservation cannot be activated until {reservation.StartTime}");


            // Se valida que la reserva no haya expirado
            // TODO: uncomment
            // var reservationStartTimeWithFiveMinutesAdded = reservation.StartTime.AddMinutes(5);
            // if (reservationStartTimeWithFiveMinutesAdded < DateTime.Now)
            //     throw new BadRequestException(
            //         $"Reservation time for activation has expired, your reservation has been canceled");


            // Se actualiza la información de la reserva
            reservation.UpdatedAt = DateTime.Now;
            reservation.ReservationStateId = ReservationStates.Active;


            // Se coloca el activador como parte de la reserva
            var userReservations = new List<UserReservation>();
            userReservations.Add(new UserReservation
            {
                UserId = activator.Id,
                UserRoleId = UserReservationRoles.Activator,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            reservation.UserReservations = userReservations;


            // Se actualiza la reserva
            await _reservationRepository.UpdateOneAsync(reservation);

            var cubicle = await _cubicleRepository.FindOneByIdAsync(reservation.CubicleId);

            return new ReservationResponseDto
            {
                CubicleCode = cubicle.Code,
                CubicleDescription = cubicle.Description,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                CubicleTotalSeats = cubicle.TotalSeats
            };
        }

        public async Task<bool> CancelReservation(CancelReservationDTO cancelReservation)
        {
            var reservation = await _reservationRepository.FindOneByIdAsync(cancelReservation.ReservationId);
            if (reservation == null)
                throw new BadRequestException(
                    $"Reservation with id {cancelReservation.ReservationId} was not found");
            if (reservation.ReservationStateId != ReservationStates.NotActive)
            {
                throw new BadRequestException("Reservation can no longer be canceled");
            }
            var host = await _userReservationRepository.GetHostByReservationId(reservation.Id);

            if (host == null)
                throw new BadRequestException("Host must be present in reservation");
            if (host.Id != cancelReservation.hostID)
            {
                throw new BadRequestException("You must be the host of the reservation");
            }

            reservation.ReservationStateId = ReservationStates.Canceled;
            await _reservationRepository.UpdateOneAsync(reservation);
            return true;
        }
    }
}