using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hospital.BusinessLogic.Services.Interfaces;
using Hospital.PresentationLogic.Models.Requests;
using Hospital.PresentationLogic.Models.Responses;
using Infrastructure.Identity;
using Hospital.DataAccess.Models.Dtos;

namespace Hospital.PresentationLogic.Controllers
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
        public async Task<IActionResult> UpdateOffice(OfficeDto request)
        {
            var result = await _officeService.UpdateOffice(request);

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
