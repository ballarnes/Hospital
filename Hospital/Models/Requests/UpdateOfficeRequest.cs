namespace Hospital.Host.Models.Requests
{
    public class UpdateOfficeRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int Number { get; set; }
    }
}
