using AutoMapper;
using Hospital.DataAccess.Models.Dtos;
using Hospital.DataAccess.Models.Entities;

namespace Hospital.PresentationLogic.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Office, OfficeDto>();
            CreateMap<OfficeDto, Office>();

            CreateMap<Specialization, SpecializationDto>();
            CreateMap<SpecializationDto, Specialization>();

            CreateMap<Doctor, DoctorDto>();
            CreateMap<DoctorDto, Doctor>();

            CreateMap<Appointment, AppointmentDto>();
            CreateMap<AppointmentDto, Appointment>();
        }
    }
}