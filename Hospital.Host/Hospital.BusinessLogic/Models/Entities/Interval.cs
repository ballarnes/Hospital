using System;

namespace Hospital.BusinessLogic.Models.Entities
{
    public class Interval
    {
        public int Id { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}
