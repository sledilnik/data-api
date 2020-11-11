using System.Collections.Immutable;
using Righthand.Immutable;

namespace SloCovidServer.Models
{
    public record RetirementHomesDay : IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public int? Total { get; init; }
        public int? Employee { get; init; }
        public int? Occupant { get; init; }
        public ImmutableArray<RetirementHomeDay> Homes { get; }
        public RetirementHomesDay(int year, int month, int day, int? total, int? employee, int? occupant, ImmutableArray<RetirementHomeDay> homes)
        {
            Year = year;
            Month = month;
            Day = day;
            Total = total;
            Employee = employee;
            Occupant = occupant;
            Homes = homes;
        }
    }

    public record RetirementHomeDay
    {
        public string Id { get; init; }
        public int? Total { get; init; }
        public int? Employee { get; init; }
        public int? Occupant { get; init; }

        public RetirementHomeDay(string id, int? total, int? employee, int? occupant)
        {
            Id = id;
            Total = total;
            Employee = employee;
            Occupant = occupant;
        }
    }
}
