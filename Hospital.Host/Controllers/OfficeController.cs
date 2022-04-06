using Hospital.Host.Models.Requests;
using Hospital.Host.Models.Responses;
using Hospital.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("hospital.office")]
    [Route("api/v1/[controller]/[action]")]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;

        public OfficeController(
            IOfficeService officeService)
        {
            _officeService = officeService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IdResponse<int?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddOffice(AddOfficeRequest request)
        {
            var result = await _officeService.AddOffice(request.Number);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateOffice(UpdateOfficeRequest request)
        {
            var result = await _officeService.UpdateOffice(request.Id, request.Number);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteOffice(GetByIdRequest request)
        {
            var result = await _officeService.DeleteOffice(request.Id);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
