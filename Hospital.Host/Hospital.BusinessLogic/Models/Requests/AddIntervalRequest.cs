using System;

namespace Hospital.BusinessLogic.Models.Requests
{
    public class AddIntervalRequest
    {
        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}