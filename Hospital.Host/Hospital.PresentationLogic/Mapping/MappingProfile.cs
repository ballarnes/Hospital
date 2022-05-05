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
            CreateMap<Specialization, SpecializationDto>();
            CreateMap<Doctor, DoctorDto>();
            CreateMap<Appointment, AppointmentDto>();
        }
    }
}