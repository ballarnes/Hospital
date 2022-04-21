namespace Hospital.Host.Models.Requests
{
    public class GetUpcomingAppointments
    {
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; }

        [Range(0, int.MaxValue)]
        public int PageSize { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = null!;
    }
}
