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
        public async Task<IActionResult> UpdateDoctor(DoctorDto request)
        {
            var result = await _doctorService.UpdateDoctor(request);

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