namespace Hospital.Host.Models.Requests
{
    public class AddOfficeRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Number { get; set; }
    }
}
