using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;
using Hospital.Host.Repositories.Interfaces;
using Hospital.Host.Services.Interfaces;

namespace Hospital.Host.Services
{
    public class AppointmentService : BaseDataService, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IDbConnectionWrapper connection,
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            ILogger<BaseDataService> logger)
            : base(connection, logger)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsResponse<AppointmentDto>?> GetAppointments(int pageIndex, int pageSize)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _appointmentRepository.GetAppointments(pageIndex, pageSize);

                if (result == null)
                {
                    return null;
                }

                return new PaginatedItemsResponse<AppointmentDto>()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    PagesCount = result.PagesCount,
                    TotalCount = result.TotalCount,
                    Data = result.Data
                };
            });
        }

        public async Task<AppointmentDto?> GetAppointmentById(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _appointmentRepository.GetAppointmentById(id);
                var mapped = _mapper.Map<AppointmentDto>(result);
                return mapped;
            });
        }
    }
}