namespace Hospital.Host.Models.Dtos
{
    public class SpecializationDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string Description { get; set; } = null!;
    }
}
