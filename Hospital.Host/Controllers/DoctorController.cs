using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Requests;
using Hospital.Host.Models.Responses;
using Hospital.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("hospital.doctor")]
    [Route("api/v1/[controller]/[action]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(
            IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IdResponse<int?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddDoctor(AddDoctorRequest request)
        {
            var result = await _doctorService.AddDoctor(request.Name, request.Surname, request.SpecializationId);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDoctor(UpdateDoctorRequest request)
        {
            var result = await _doctorService.UpdateDoctor(request.Id, request.Name, request.Surname, request.SpecializationId);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteDoctor(GetByIdRequest request)
        {
            var result = await _doctorService.DeleteDoctor(request.Id);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}