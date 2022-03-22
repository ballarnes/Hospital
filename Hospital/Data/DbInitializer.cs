using Hospital.Host.Data.Entities;

namespace Hospital.Host.Data
{
    public class DbInitializer
    {
        public static IEnumerable<Office> GetPreconfiguredOffices()
        {
            return new List<Office>()
            {
                new Office()
                {
                    Number = 101 
                },
                new Office()
                {
                    Number = 102
                },
                new Office()
                {
                    Number = 103
                },
                new Office()
                {
                    Number = 104
                },
                new Office()
                {
                    Number = 105
                }
            };
        }

        public static IEnumerable<Interval> GetPreconfiguredIntervals()
        {
            return new List<Interval>()
            {
                new Interval()
                {
                    Start = new TimeSpan(9, 0, 0),
                    End = new TimeSpan(9, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(9, 30, 0),
                    End = new TimeSpan(10, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(10, 0, 0),
                    End = new TimeSpan(10, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(10, 30, 0),
                    End = new TimeSpan(11, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(11, 0, 0),
                    End = new TimeSpan(11, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(11, 30, 0),
                    End = new TimeSpan(12, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(12, 0, 0),
                    End = new TimeSpan(12, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(12, 30, 0),
                    End = new TimeSpan(13, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(14, 0, 0),
                    End = new TimeSpan(14, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(14, 30, 0),
                    End = new TimeSpan(15, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(15, 30, 0),
                    End = new TimeSpan(16, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(16, 0, 0),
                    End = new TimeSpan(16, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(16, 30, 0),
                    End = new TimeSpan(17, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(17, 0, 0),
                    End = new TimeSpan(17, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(17, 30, 0),
                    End = new TimeSpan(18, 0, 0)
                }
            };
        }

        public static IEnumerable<Specialization> GetPreconfiguredSpecializations()
        {
            return new List<Specialization>()
            {
                new Specialization()
                {
                    Name = "Therapist",
                    Description = "Therapist is the most popular medical specialist who performs the initial examination and diagnosis of diseases."
                },
                new Specialization()
                {
                    Name = "Surgeon",
                    Description = "A surgeon is one of the most responsible and complex professions in the world, whose representatives treat diseases, injuries and pathologies through surgical intervention."
                },
                new Specialization()
                {
                    Name = "Ophthalmologist",
                    Description = "Ophthalmologists treat eye diseases and pathologies, perform vision correction, and prescribe glasses and contact lenses."
                },
                new Specialization()
                {
                    Name = "Nutritionist",
                    Description = "A nutritionist is a doctor who specializes in solving health problems and overweight with the help of a properly selected diet."
                }
            };
        }

        public static IEnumerable<Doctor> GetPreconfiguredDoctors()
        {
            return new List<Doctor>()
            {
                new Doctor()
                {
                    Name = "Kerry",
                    Surname = "Bridges",
                    SpecializationId = 1
                },
                new Doctor()
                {
                    Name = "Stephany",
                    Surname = "Dawson",
                    SpecializationId = 1
                },
                new Doctor()
                {
                    Name = "Maria",
                    Surname = "Grant",
                    SpecializationId = 2
                },
                new Doctor()
                {
                    Name = "Donald",
                    Surname = "Webb",
                    SpecializationId = 3
                },
                new Doctor()
                {
                    Name = "David",
                    Surname = "Blair",
                    SpecializationId = 4
                }
            };
        }
    }
}
