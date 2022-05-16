using System;

namespace Hospital.BusinessLogic.Models.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        public int OfficeId { get; set; }

        public Office Office { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PatientName { get; set; } = null!;
    }
}
