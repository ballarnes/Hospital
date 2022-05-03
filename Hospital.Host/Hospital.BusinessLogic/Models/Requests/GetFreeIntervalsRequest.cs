using System;

namespace Hospital.BusinessLogic.Models.Requests
{
    public class GetFreeIntervalsRequest
    {
        public int DoctorId { get; set; }

        public DateTime Date { get; set; }
    }
}