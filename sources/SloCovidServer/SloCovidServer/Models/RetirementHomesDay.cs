using System.Collections.Immutable;
using Righthand.Immutable;

namespace SloCovidServer.Models
{
    public class RetirementHomesDay : IModelDate
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public int? Total { get; }
        public int? Employee { get; }
        public int? Occupant { get; }
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

    public class RetirementHomeDay
    {
        public string Id { get; }
        public int? Total { get; }
        public int? Employee { get; }
        public int? Occupant { get; }

        public RetirementHomeDay(string id, int? total, int? employee, int? occupant)
        {
            Id = id;
            Total = total;
            Employee = employee;
            Occupant = occupant;
        }

        public RetirementHomeDay Clone(Param<string>? id = null, Param<int?>? total = null, Param<int?>? employee = null, Param<int?>? occupant = null)
        {
            return new RetirementHomeDay(id.HasValue ? id.Value.Value : Id,
				total.HasValue ? total.Value.Value : Total,
				employee.HasValue ? employee.Value.Value : Employee,
				occupant.HasValue ? occupant.Value.Value : Occupant);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (RetirementHomeDay)obj;
            return Equals(Id, o.Id) && Equals(Total, o.Total) && Equals(Employee, o.Employee) && Equals(Occupant, o.Occupant);
}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = 23;
				hash = hash * 37 + (Id != null ? Id.GetHashCode() : 0);
				hash = hash * 37 + (Total != null ? Total.GetHashCode() : 0);
				hash = hash * 37 + (Employee != null ? Employee.GetHashCode() : 0);
				hash = hash * 37 + (Occupant != null ? Occupant.GetHashCode() : 0);
				return hash;
			}
        }
    }
}
