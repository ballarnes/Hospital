using Hospital.Host.Models.Requests;
using Hospital.Host.Models.Responses;
using Hospital.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("hospital.specialization")]
    [Route("api/v1/[controller]/[action]")]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(
            ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IdResponse<int?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddSpecialization(AddSpecializationRequest request)
        {
            var result = await _specializationService.AddSpecialization(request.Name, request.Description);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateSpecialization(UpdateSpecializationRequest request)
        {
            var result = await _specializationService.UpdateSpecialization(request.Id, request.Name, request.Description);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteSpecialization(GetByIdRequest request)
        {
            var result = await _specializationService.DeleteSpecialization(request.Id);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
