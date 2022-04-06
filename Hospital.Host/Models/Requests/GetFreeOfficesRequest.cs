namespace Hospital.Host.Models.Requests
{
    public class GetFreeOfficesRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IntervalId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
