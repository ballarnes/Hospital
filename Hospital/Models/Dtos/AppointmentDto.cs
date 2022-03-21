namespace Hospital.Host.Models.Dtos
{
    public class AppointmentDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        public DoctorDto Doctor { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int IntervalId { get; set; }

        public IntervalDto Interval { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int OfficeId { get; set; }

        public OfficeDto Office { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [StringLength(50)]
        public string PatientName { get; set; } = null!;
    }
}
