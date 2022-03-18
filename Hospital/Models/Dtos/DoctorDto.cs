namespace Hospital.Host.Models.Dtos
{
    public class DoctorDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Surname { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int SpecializationId { get; set; }
    }
}
