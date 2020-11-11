using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record HospitalsDay : IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public HospitalDay Overall { get; init; }
        public ImmutableDictionary<string, HospitalDay> PerHospital { get; init; }
        public HospitalsDay(int year, int month, int day, HospitalDay overall, ImmutableDictionary<string, HospitalDay> perHospital)
        {
            Year = year;
            Month = month;
            Day = day;
            Overall = overall;
            PerHospital = perHospital;
        }
    }

    public record HospitalDay
    {
        public HospitalBedDay Beds { get; init; }
        public HospitalICUDay ICU { get; init; }
        public HospitalVentDay Vents { get; init; }
        public HospitalCareDay Care { get; init; }
        public HospitalDay(HospitalBedDay beds, HospitalICUDay iCU, HospitalVentDay vents, HospitalCareDay care)
        {
            Beds = beds;
            ICU = iCU;
            Vents = vents;
            Care = care;
        }
    }

    public record HospitalBedDay
    {
        public int? Total { get; init; }
        public int? Max { get; init; }
        public int? Occupied { get; init; }
        public int? Free { get; init; }
        public int? MaxFree { get; init; }
        public HospitalBedDay(int? total, int? max, int? occupied, int? free, int? maxFree)
        {
            Total = total;
            Max = max;
            Occupied = occupied;
            Free = free;
            MaxFree = maxFree;
        }
    }
    public record HospitalICUDay
    {
        public int? Total { get; init; }
        public int? Max { get; init; }
        public int? Occupied { get; init; }
        public int? Free { get; init; }
        public HospitalICUDay(int? total, int? max, int? occupied, int? free)
        {
            Total = total;
            Max = max;
            Occupied = occupied;
            Free = free;
        }
    }

    public record HospitalVentDay
    {
        public int? Total { get; init; }
        public int? Max { get; init; }
        public int? Occupied { get; init; }
        public int? Free { get; init; }
        public HospitalVentDay(int? total, int? max, int? occupied, int? free)
        {
            Total = total;
            Max = max;
            Occupied = occupied;
            Free = free;
        }
    }

    public record HospitalCareDay
    {
        public int? Total { get; init; }
        public int? Max { get; init; }
        public int? Occupied { get; init; }
        public int? Free { get; init; }
        public HospitalCareDay(int? total, int? max, int? occupied, int? free)
        {
            Total = total;
            Max = max;
            Occupied = occupied;
            Free = free;
        }
    }
}
