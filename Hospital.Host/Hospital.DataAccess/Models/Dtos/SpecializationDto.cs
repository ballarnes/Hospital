namespace Hospital.DataAccess.Models.Dtos
{
    public class SpecializationDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}