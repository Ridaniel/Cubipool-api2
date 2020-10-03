using Cubipool.Service.Abstractions;
using Cubipool.Service.Common;
using Cubipool.Service.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Cubipool.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService requestService;

        public RequestController(IRequestService requestService)
        {
            this.requestService = requestService;
        }

        [Route("requests")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestRequestDto requestDto)
        {
            try
            {
                var response = await requestService.CreateOneAsync(requestDto);
                return Ok(response);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [Route("users/{id}/requests")]
        [HttpGet]
        public IActionResult GetAllMyPendingAndAcceptedCurrentRequests(int id)
        {
            try
            {
                // var x = User.FindFirst(ClaimTypes.NameIdentifier);
                //var jwtUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var response = requestService.GetAllPendingAndAcceptedCurrentRequestsByUserId(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [Route("confirmRequest")]
        [HttpPost]
        public async Task<IActionResult> ConfirmRequest([FromBody] ConfirmGuestDTORequest RequestDto)
        {
            try
            {
                var response = await requestService.ConfirmGuest(RequestDto);
                return Ok(response);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [Route("publications/{publicationId}/requests")]
        [HttpGet]
        public async Task<IActionResult> GetAllPendingAndAcceptedRequestByPublicationId([FromRoute] int publicationId)
        {
            try
            {
                var response = await requestService.GetAllPendingAndAcceptedRequestByPublicationId(publicationId);
                return Ok(response);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [Route("requests/answer")]
        [HttpPut]
        public async Task<IActionResult> AnswerRequest([FromBody] AnswerRequestDTO requestDto)
        {
            try
            {
                var response = await requestService.answerRequest(requestDto);
                return Ok();
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [Route("requests/cancel")]
        [HttpPut]
        public async Task<IActionResult> CancelRequest([FromBody] CancelRequestDTO cancelRequest)
        {
            try
            {
                var response = await requestService.cancelRequest(cancelRequest);
                return Ok();
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
    }
}
