﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class UpdateAppointmentRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int OfficeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(50)]
        public string PatientName { get; set; } = null!;
    }
}
