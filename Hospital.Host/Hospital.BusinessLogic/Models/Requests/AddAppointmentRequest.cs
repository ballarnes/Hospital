using System;

namespace Hospital.BusinessLogic.Models.Requests
{
    public class AddAppointmentRequest
    {
        public int DoctorId { get; set; }

        public int IntervalId { get; set; }

        public int OfficeId { get; set; }

        public DateTime Date { get; set; }

        public string PatientName { get; set; } = null!;
    }
}
