namespace Hospital.Host.Models.Requests
{
    public class AddDoctorRequest
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Surname { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int SpecializationId { get; set; }
    }
}
