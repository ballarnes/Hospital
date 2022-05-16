namespace Hospital.BusinessLogic.Models.Requests
{
    public class AddSpecializationRequest
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}