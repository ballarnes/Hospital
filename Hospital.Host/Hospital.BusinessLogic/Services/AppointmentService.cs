using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Hospital.BusinessLogic.Models.Responses;
using Hospital.BusinessLogic.Services.Interfaces;
using Hospital.DataAccess.Repositories.Interfaces;
using Infrastructure.Connection.Interfaces;
using AutoMapper;
using Hospital.DataAccess.Models.Dtos;
using System.Linq;

namespace Hospital.BusinessLogic.Services
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

        public async Task<PaginatedItemsResponse<AppointmentDto>> GetAppointments(int pageIndex, int pageSize)
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
                    Data = result.Data.Select(s => _mapper.Map<AppointmentDto>(s)).ToList()
                };
            });
        }

        public async Task<PaginatedItemsResponse<AppointmentDto>> GetUpcomingAppointments(int pageIndex, int pageSize, string name)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _appointmentRepository.GetUpcomingAppointments(pageIndex, pageSize, name);

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
                    Data = result.Data.Select(s => _mapper.Map<AppointmentDto>(s)).ToList()
                };
            });
        }

        public async Task<AppointmentDto> GetAppointmentById(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _appointmentRepository.GetAppointmentById(id);
                var mapped = _mapper.Map<AppointmentDto>(result);
                return mapped;
            });
        }

        public async Task<IdResponse<int>> AddAppointment(int doctorId, int intervalId, int officeId, DateTime date, string patientName)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _appointmentRepository.AddAppointment(doctorId, intervalId, officeId, date, patientName);

                if (result == default)
                {
                    return null;
                }

                return new IdResponse<int>()
                {
                    Id = result.Value
                };
            });
        }

        public async Task<int?> UpdateAppointment(int id, int doctorId, int intervalId, int officeId, DateTime date, string patientName)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _appointmentRepository.UpdateAppointment(id, doctorId, intervalId, officeId, date, patientName);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }

        public async Task<int?> DeleteAppointment(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _appointmentRepository.DeleteAppointment(id);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }
    }
}