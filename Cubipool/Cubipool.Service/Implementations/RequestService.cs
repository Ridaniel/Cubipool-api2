using Cubipool.Common.Constants;
using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository;
using Cubipool.Repository.Abstractions;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Common;
using Cubipool.Service.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cubipool.Service.Implementations
{
    public class RequestService : IRequestService
    {
        private readonly IUserRepository userRepository;
        private readonly IRequestRepository requestRepository;
        private readonly ISharedSpaceRepository sharedSpaceRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly IReservationRepository reservationRepository;
        private readonly IConstantsRepository constantsRepository;


        public RequestService(
            IRequestRepository requestRepository,
            ISharedSpaceRepository sharedSpaceRepository,
            IUserRepository userRepository,
            ICampusRepository campusRepository,
            IReservationRepository reservationRepository,
            IConstantsRepository constantsRepository
        )
        {
            this.requestRepository = requestRepository;
            this.sharedSpaceRepository = sharedSpaceRepository;
            this.userRepository = userRepository;
            _campusRepository = campusRepository;
            this.reservationRepository = reservationRepository;
            this.constantsRepository = constantsRepository;
        }

        public async Task<CreateRequestResponseDto> CreateOneAsync(CreateRequestRequestDto requestDto)
        {
            var foundUser = await userRepository.FindOneByIdAsync(requestDto.UserId);
            if (foundUser == null)
                throw new NotFoundException("No se encontró el User para la creación del Request");

            var sharedSpaceEntity = await sharedSpaceRepository.FindOneByIdAsync(requestDto.SharedSpaceId);
            if (sharedSpaceEntity == null)
                throw new NotFoundException("No se encontró el SharedSpace para la creación del Request");

            var requestsAlreadySent = requestRepository.GetAllByUserIdAndPublicationId(requestDto.UserId, sharedSpaceEntity.PublicationId);
            if (requestsAlreadySent.Count() > 0)
                throw new BadRequestException("El usuario ya ha mandado una solicitud para esa publicacion");

            if (sharedSpaceEntity.IsOccupied)
                throw new BadRequestException("El SharedSpace ya se encuentra ocupado");

            var requestEntity = requestDto.ToRequestEntity();
            await requestRepository.CreateOneAsync(requestEntity);
            return CreateRequestResponseDto.FromRequestEntity(requestEntity);
        }

        public IEnumerable<PendingAndAcceptedCurrentRequestResponseDto> GetAllPendingAndAcceptedCurrentRequestsByUserId(int id)
        {
            var requests = requestRepository.GetAllPendingAndAcceptedCurrentRequestsByUserId(id);
            var response = requests.Select(request =>
            {
                var cubicle = request.SharedSpace.Publication.Reservation.Cubicle;
                var campus = _campusRepository.GetOneById(cubicle.CampusId);

                return PendingAndAcceptedCurrentRequestResponseDto.FromSharedSpace(
                    request,
                    request.SharedSpace.Publication.Reservation.Cubicle,
                    request.SharedSpace.Publication,
                    request.SharedSpace.Resource,
                    request.SharedSpace,
                    request.Constant,
                    campus
                );
            });

            return response;
        }
        public async Task<bool> ConfirmGuest(ConfirmGuestDTORequest request)
        {
            Request req = await requestRepository.GetOneByIdAsync(request.RequestId);

            if (req == null)
            {
                throw new NotFoundException($"Request not found");
            }

            Reservation reserv = await reservationRepository.FindOneByIdAsync(request.ReservationId);

            if (reserv == null)
            {
                throw new NotFoundException($"Reservation not found");
            }
            Console.WriteLine("Cantidad de user Reservations");
            Console.WriteLine(reserv.UserReservations.Count);
            var ur = reserv.UserReservations.Where(uR => uR.UserId == request.hostId);

            if (ur == null)
            {
                throw new NotFoundException($"Host not found");
            }

            req.ConstantId = RequestStatus.Confirmed;

            req = await requestRepository.UpdateOneAsync(req);

            return true;
        }

        public async Task<ICollection<RequestResponseDto>> GetAllPendingAndAcceptedRequestByPublicationId(int id)
        {
            ICollection<Request> requests = await requestRepository.GetAllPendingAndAcceptedRequestByPublicationId(id);

            ICollection<RequestResponseDto> requestsdto = new List<RequestResponseDto>();

            foreach (var request in requests)
            {

                requestsdto.Add(RequestResponseDto.FromRequestEntity(request));
            }


            return requestsdto;
        }

        public async Task<bool> answerRequest(AnswerRequestDTO answer)
        {
            Request request = await requestRepository.GetOneByIdAsync(answer.RequestId);
            if (request == null || request.ConstantId != RequestStatus.Waiting)
                throw new BadRequestException("No existe ese request");

            if (!answer.answer)
            {
                request.ConstantId = RequestStatus.Denied;
                await requestRepository.UpdateOneAsync(request);
                return true;
            }
            else
            {
                if (request.ConstantId != RequestStatus.Waiting)
                {
                    throw new BadRequestException("Esta solicitud ya esta respondida");
                }

                if (request.SharedSpace.IsOccupied)
                {
                    throw new BadRequestException("Esta ocupado el espacio compartido que se solicita");
                }
                request.ConstantId = RequestStatus.Accepted;

                SharedSpace SharedSpace = await sharedSpaceRepository.FindOneByIdAsync(request.SharedSpace.Id);
                foreach (var item in SharedSpace.Requests)
                {
                    if (item.ConstantId == RequestStatus.Waiting)
                    {
                        if (item.Id != answer.RequestId)
                        {
                            item.ConstantId = RequestStatus.Denied;
                        }
                        else
                        {
                            item.ConstantId = RequestStatus.Accepted;
                        }
                        await requestRepository.UpdateOneAsync(item);
                    }
                }
                SharedSpace.IsOccupied = true;
                await sharedSpaceRepository.UpdateAsync(SharedSpace);



            }





            return true;
        }

        public async Task<bool> cancelRequest(CancelRequestDTO cancel)
        {
            var request = await requestRepository.GetOneByIdAsync(cancel.RequestId);
            if (request == null)
            {
                throw new BadRequestException($"Request with id {cancel.RequestId} was not found");
            }
            if (request.ConstantId == RequestStatus.Denied)
            {
                throw new BadRequestException($"Request with id {cancel.RequestId} is denied");
            }
            if (request.ConstantId == RequestStatus.Confirmed)
            {
                throw new BadRequestException($"Request with id {cancel.RequestId} is confirmed");
            }
            if (request.ConstantId == RequestStatus.Canceled)
            {
                throw new BadRequestException($"Request with id {cancel.RequestId} is canceled");
            }
            request.ConstantId = RequestStatus.Canceled;
            if (request.ConstantId != RequestStatus.Accepted)
            {
                await requestRepository.UpdateOneAsync(request);
            }
            else
            {
                Console.WriteLine("Requeste aceptado");
                SharedSpace sharedSpace = await sharedSpaceRepository.FindOneByIdAsync(request.SharedSpaceId);
                sharedSpace.IsOccupied = false;
                await requestRepository.UpdateOneAsync(request);
                await sharedSpaceRepository.UpdateAsync(sharedSpace);
                Console.WriteLine("ShareSpace ya no ocupado");
            }





            return true;
        }
    }
}
