using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class GetFreeOfficesRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IntervalId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
