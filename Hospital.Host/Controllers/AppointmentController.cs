using Hospital.Host.Models.Requests;
using Hospital.Host.Models.Responses;
using Hospital.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("hospital.appointment")]
    [Route("api/v1/[controller]/[action]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(
            IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IdResponse<int?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAppointment(AddAppointmentRequest request)
        {
            var result = await _appointmentService.AddAppointment(request.DoctorId, request.IntervalId, request.OfficeId, request.Date, request.PatientName);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAppointment(UpdateAppointmentRequest request)
        {
            var result = await _appointmentService.UpdateAppointment(request.Id, request.DoctorId, request.IntervalId, request.OfficeId, request.Date, request.PatientName);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAppointment(GetByIdRequest request)
        {
            var result = await _appointmentService.DeleteAppointment(request.Id);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}