namespace Hospital.Host.Models.Dtos
{
    public class OfficeDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Number { get; set; }
    }
}
