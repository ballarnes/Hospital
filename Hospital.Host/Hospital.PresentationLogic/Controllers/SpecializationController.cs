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
        public async Task<IActionResult> UpdateSpecialization(SpecializationDto request)
        {
            var result = await _specializationService.UpdateSpecialization(request);

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
