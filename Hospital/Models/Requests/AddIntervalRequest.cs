namespace Hospital.Host.Models.Requests
{
    public class AddIntervalRequest
    {
        [Required]
        public TimeSpan Start { get; set; }

        [Required]
        public TimeSpan End { get; set; }
    }
}
