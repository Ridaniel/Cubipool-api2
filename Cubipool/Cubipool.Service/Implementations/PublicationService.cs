using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Common.Exceptions;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Publications;
using Cubipool.Service.Dtos.Cubicles;
using Cubipool.Common.Constants;

namespace Cubipool.Service.Implementations
{
    public class PublicationService : IPublicationService
    {

        private readonly EFDbContext _context;
        private readonly IPublicationRepository _publicationRepository;
        private readonly ISharedSpaceRepository _sharedSpaceRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ICampusRepository campusRepository;
        private readonly IResourceTypeRepository _resourceTypeRepository;
        public PublicationService(
            EFDbContext context,
            IPublicationRepository publicationRepository,
            ISharedSpaceRepository sharedSpaceRepository,
            IReservationRepository reservationRepository,
            ICampusRepository campusRepository,
            IResourceTypeRepository resourceTypeRepository
        )
        {
            _context = context;
            _publicationRepository = publicationRepository;
            _sharedSpaceRepository = sharedSpaceRepository;
            _reservationRepository = reservationRepository;
            this.campusRepository = campusRepository;
            _resourceTypeRepository = resourceTypeRepository;
        }
        public async Task<GetPublicationDto> FindOneByIdAsync(int id)
        {
            var item = await _publicationRepository.FindOneByIdAsync(id);
            return GetPublicationDto.FromPublication(item);
        }

        public async Task<IEnumerable<GetPublicationDto>> FindAllAsync()
        {
            var items = await _publicationRepository.GetAllAsync();

            return items.Select(GetPublicationDto.FromPublication);

        }

        public async Task<GetPublicationDto> CreateOneAsync(CreatePublicationDto item)
        {
            try
            {
                foreach (var space in item.SharedSpaces)
                {
                    if (await _sharedSpaceRepository.IsAvaliableSharedSpace(space.CreateSharedSpace()))
                        throw new BadRequestException($"The space with resource id {space.ResourceId} has been shared already");
                }


                if (!await ValidPublicationTime(item))
                    throw new BadRequestException($"The Publication with reservartion id {item.ReservationId} time is not valid");

                await _publicationRepository.CreateOneAsync(item.CreatePublication());

                // Se debe actualizar el estado de la reserva a compartido(ID=8)
                var reservation = await _reservationRepository.FindOneByIdAsync(item.ReservationId);
                reservation.ReservationStateId = ReservationStates.Shared;
                reservation.UpdatedAt = DateTime.Now;
                await _reservationRepository.UpdateOneAsync(reservation);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                throw new BadRequestException("System error");
            }

            return GetPublicationDto.FromPublication(item.CreatePublication());
        }

        public async Task<IEnumerable<GetByFiltersResponseDto>> GetByFiltersAsync(GetByFiltersRequestDto requestDto)
        {
            var campusFromDb = await campusRepository.GetOneByIdAsync(requestDto.CampusId);
            if (campusFromDb == null)
                throw new BadRequestException("El campus elegido no existe");

            if (!(requestDto.SeatStartTime >= DateTime.Now.AddMinutes(-30) && requestDto.SeatStartTime <= DateTime.Now.AddHours(1).AddMinutes(30)))
                throw new BadRequestException("La hora de inicio del asiento está fuera de los límites");

            if (!((requestDto.SeatStartTime.Minute == 30 || requestDto.SeatStartTime.Minute == 0) && requestDto.SeatStartTime.Second == 0))
                throw new BadRequestException("La hora de inicio del asiento debe ser redonda");

            if (requestDto.SeatEndTime.HasValue)
            {
                var intervalMinutes = (requestDto.SeatEndTime.Value - requestDto.SeatStartTime).TotalMinutes;
                if (!(intervalMinutes >= 30 && intervalMinutes <= 120))
                    throw new BadRequestException("La hora de fin del asiento está fuera de los límites");

                if (!((requestDto.SeatEndTime.Value.Minute == 30 || requestDto.SeatEndTime.Value.Minute == 0) && requestDto.SeatEndTime.Value.Second == 0))
                    throw new BadRequestException("La hora de fin del asiento debe ser redonda");
            }
            else
                requestDto.SeatEndTime = requestDto.SeatStartTime.AddHours(2);

            if (requestDto.SeatEndTime.Value.Ticks - requestDto.SeatStartTime.Ticks <= 0)
                throw new BadRequestException("La hora de fin del asiento no puede ser igual o menor que la hora de inicio del asiento");

            // si tiene un recurso equipado
            if (requestDto.ResourceTypeId.HasValue)
            {
                var resourceTypeFromDb = await _resourceTypeRepository.GetOneByIdAsync(requestDto.ResourceTypeId.Value);
                if (resourceTypeFromDb == null)
                    throw new BadRequestException("El recurso elegido no existe");
            }

            var publicationEntities = _publicationRepository.GetAllForGuests(
                requestDto.CampusId,
                requestDto.SeatStartTime,
                requestDto.SeatEndTime.Value,
                requestDto.ResourceTypeId
            );

            return publicationEntities.Select(p => GetByFiltersResponseDto.FromPublicationAndCubicle(p, p.Reservation.Cubicle));
        }

        private async Task<bool> ValidPublicationTime(CreatePublicationDto pub)
        {
            
            var res = await _reservationRepository.FindOneByIdAsync(pub.ReservationId);
            if (pub.StartTime.Hour < res.StartTime.Hour || pub.EndTime.Hour > res.EndTime.Hour)
                return false;
            return true;
        }






    }
}