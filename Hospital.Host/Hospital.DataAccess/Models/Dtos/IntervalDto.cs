using System;

namespace Hospital.DataAccess.Models.Dtos
{
    public class IntervalDto
    {
        public int Id { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}
