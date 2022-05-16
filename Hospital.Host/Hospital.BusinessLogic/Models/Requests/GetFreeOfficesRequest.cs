using System;

namespace Hospital.BusinessLogic.Models.Requests
{
    public class GetFreeOfficesRequest
    {
        public int IntervalId { get; set; }

        public DateTime Date { get; set; }
    }
}