using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.RequestCallDto;
using QuartierLatin.Backend.Models.CallRequest;

namespace QuartierLatin.Backend.Controllers
{
    public class CallRequestController : Controller
    {
        private readonly ICallRequestAppService _callRequestAppService;

        public CallRequestController(ICallRequestAppService callRequestAppService)
        {
            _callRequestAppService = callRequestAppService;
        }

        [AllowAnonymous]
        [HttpPost("/api/call/request/")]
        public async Task<IActionResult> RequestCall([FromBody] RequestCallDto requestCallDto)
        {
            var requestCall = new CallRequest(requestCallDto.FirstName, requestCallDto.LastName, requestCallDto.Email, requestCallDto.Phone, requestCallDto.Url, requestCallDto.Comment);
            var response = await _callRequestAppService.SendCallRequest(requestCall);

            if (response)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
