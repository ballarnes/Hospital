namespace Hospital.Host.Models.Requests
{
    public class UpdateIntervalRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}
