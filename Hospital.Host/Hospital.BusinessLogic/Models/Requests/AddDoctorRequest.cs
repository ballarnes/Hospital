namespace Hospital.BusinessLogic.Models.Requests
{
    public class AddDoctorRequest
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public int SpecializationId { get; set; }
    }
}