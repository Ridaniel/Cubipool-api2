using Cubipool.Entity;
using Cubipool.Service.Common;
using Cubipool.Service.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubipool.Service.Abstractions
{
    public interface IRequestService
    {
        Task<bool> ConfirmGuest(ConfirmGuestDTORequest confirmGuestDTORequest);
        Task<CreateRequestResponseDto> CreateOneAsync(CreateRequestRequestDto requestDto);
        IEnumerable<PendingAndAcceptedCurrentRequestResponseDto> GetAllPendingAndAcceptedCurrentRequestsByUserId(int id);
        Task<ICollection<RequestResponseDto>> GetAllPendingAndAcceptedRequestByPublicationId(int id);

        Task<Boolean> answerRequest(AnswerRequestDTO answer);

        Task<Boolean> cancelRequest(CancelRequestDTO cancel);
    }
}
