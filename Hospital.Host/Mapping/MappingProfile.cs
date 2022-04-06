using Hospital.Host.Data.Entities;
using Hospital.Host.Models.Dtos;

namespace Hospital.Host.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Office, OfficeDto>();
        CreateMap<Interval, IntervalDto>();
        CreateMap<Specialization, SpecializationDto>();
        CreateMap<Doctor, DoctorDto>();
        CreateMap<Appointment, AppointmentDto>();
    }
}