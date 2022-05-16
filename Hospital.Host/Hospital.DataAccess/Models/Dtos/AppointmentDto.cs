using System;

namespace Hospital.DataAccess.Models.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DoctorDto Doctor { get; set; } = null!;

        public int OfficeId { get; set; }

        public OfficeDto Office { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PatientName { get; set; } = null!;
    }
}