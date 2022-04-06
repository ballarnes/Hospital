namespace Hospital.Host.Models.Requests
{
    public class GetByIdRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
