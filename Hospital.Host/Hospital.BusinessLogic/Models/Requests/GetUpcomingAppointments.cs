namespace Hospital.BusinessLogic.Models.Requests
{
    public class GetUpcomingAppointments
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Name { get; set; } = null!;
    }
}