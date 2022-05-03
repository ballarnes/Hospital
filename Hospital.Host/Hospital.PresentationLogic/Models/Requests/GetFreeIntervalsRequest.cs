using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class GetFreeIntervalsRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
