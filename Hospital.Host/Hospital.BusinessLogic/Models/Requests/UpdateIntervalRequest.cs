using System;

namespace Hospital.BusinessLogic.Models.Requests
{
    public class UpdateIntervalRequest
    {
        public int Id { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}