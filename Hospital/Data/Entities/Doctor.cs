namespace Hospital.Host.Data.Entities
{
    public class Doctor
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public int SpecializationId { get; set; }
    }
}
