namespace Hospital.DataAccess.Models.Dtos
{
    public class DoctorDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public int SpecializationId { get; set; }

        public SpecializationDto Specialization { get; set; } = null!;
    }
}