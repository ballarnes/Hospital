using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class GetFreeOfficesRequest
    {
        [Required]
        public DateTime Date { get; set; }
    }
}
