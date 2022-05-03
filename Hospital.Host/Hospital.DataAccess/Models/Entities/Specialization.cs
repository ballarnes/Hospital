namespace Hospital.DataAccess.Models.Entities
{
    public class Specialization
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
