namespace Hospital.BusinessLogic.Models.Requests
{
    public class UpdateSpecializationRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}