using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class GetAppointmentsByDoctorDate
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
