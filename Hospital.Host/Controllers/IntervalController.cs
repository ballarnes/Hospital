using Hospital.Host.Models.Requests;
using Hospital.Host.Models.Responses;
using Hospital.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("hospital.interval")]
    [Route("api/v1/[controller]/[action]")]
    public class IntervalController : ControllerBase
    {
        private readonly IIntervalService _intervalService;

        public IntervalController(
            IIntervalService intervalService)
        {
            _intervalService = intervalService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IdResponse<int?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddInterval(AddIntervalRequest request)
        {
            var result = await _intervalService.AddInterval(request.Start, request.End);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateInterval(UpdateIntervalRequest request)
        {
            var result = await _intervalService.UpdateInterval(request.Id, request.Start, request.End);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteInterval(GetByIdRequest request)
        {
            var result = await _intervalService.DeleteInterval(request.Id);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
