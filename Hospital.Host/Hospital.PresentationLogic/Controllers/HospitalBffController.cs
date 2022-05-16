using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hospital.BusinessLogic.Services.Interfaces;
using Hospital.PresentationLogic.Models.Requests;
using Hospital.PresentationLogic.Models.Responses;
using Hospital.DataAccess.Models.Dtos;

namespace Hospital.PresentationLogic.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class HospitalBffController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly ISpecializationService _specializationService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public HospitalBffController(
            IOfficeService officeService,
            ISpecializationService specializationService,
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _officeService = officeService;
            _specializationService = specializationService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<OfficeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOffices(PaginatedItemsRequest request)
        {
            var result = await _officeService.GetOffices(request.PageIndex, request.PageSize);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OfficeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOfficeById(GetByIdRequest request)
        {
            var result = await _officeService.GetOfficeById(request.Id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ArrayResponse<OfficeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetFreeOfficesByDate(GetFreeOfficesRequest request)
        {
            var result = await _officeService.GetFreeOfficesByDate(request.Date);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<SpecializationDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSpecializations(PaginatedItemsRequest request)
        {
            var result = await _specializationService.GetSpecializations(request.PageIndex, request.PageSize);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SpecializationDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetSpecializationById(GetByIdRequest request)
        {
            var result = await _specializationService.GetSpecializationById(request.Id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<DoctorDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDoctors(PaginatedItemsRequest request)
        {
            var result = await _doctorService.GetDoctors(request.PageIndex, request.PageSize);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DoctorDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDoctorById(GetByIdRequest request)
        {
            var result = await _doctorService.GetDoctorById(request.Id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<DoctorDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDoctorsBySpecializationId(GetByIdRequest request)
        {
            var result = await _doctorService.GetDoctorsBySpecializationId(request.Id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<AppointmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAppointments(PaginatedItemsRequest request)
        {
            var result = await _appointmentService.GetAppointments(request.PageIndex, request.PageSize);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<AppointmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUpcomingAppointments(GetUpcomingAppointments request)
        {
            var result = await _appointmentService.GetUpcomingAppointments(request.PageIndex, request.PageSize, request.Name);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ArrayResponse<AppointmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAppointmentsByDoctorDate(GetAppointmentsByDoctorDate request)
        {
            var result = await _appointmentService.GetAppointmentsByDoctorDate(request.DoctorId, request.Date);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AppointmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAppointmentById(GetByIdRequest request)
        {
            var result = await _appointmentService.GetAppointmentById(request.Id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
