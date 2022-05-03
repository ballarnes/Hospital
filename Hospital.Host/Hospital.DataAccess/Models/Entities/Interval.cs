using System;

namespace Hospital.DataAccess.Models.Entities
{
    public class Interval
    {
        public int Id { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}
