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

        [Required]
        [Range(1, int.MaxValue)]
        public int IntervalId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int OfficeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(50)]
        public string PatientName { get; set; } = null!;
    }
}
