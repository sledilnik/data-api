using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class HospitalsDay : IModelDate
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public HospitalDay Overall { get; }
        public ImmutableDictionary<string, HospitalDay> PerHospital { get; }
        public HospitalsDay(int year, int month, int day, HospitalDay overall, ImmutableDictionary<string, HospitalDay> perHospital)
        {
            Year = year;
            Month = month;
            Day = day;
            Overall = overall;
            PerHospital = perHospital;
        }
    }

    public class HospitalDay
    {
        public HospitalBedDay Beds { get; }
        public HospitalICUDay ICU { get; }
        public HospitalVentDay Vents { get; }
        public HospitalCareDay Care { get; }
        public HospitalDay(HospitalBedDay beds, HospitalICUDay iCU, HospitalVentDay vents, HospitalCareDay care)
        {
            Beds = beds;
            ICU = iCU;
            Vents = vents;
            Care = care;
        }
    }

    public class HospitalBedDay
    { 
        public int? Total { get; }
        public int? Max { get; }
        public int? Occupied { get; }
        public int? Free { get; }
        public int? MaxFree { get; }
        public HospitalBedDay(int? total, int? max, int? occupied, int? free, int? maxFree)
        {
            Total = total;
            Max = max;
            Occupied = occupied;
            Free = free;
            MaxFree = maxFree;
        }
    }
    public class HospitalICUDay
    {
        public int? Total { get; }
        public int? Max { get; }
        public int? Occupied { get; }
        public int? Free { get; }
        public HospitalICUDay(int? total, int? max, int? occupied, int? free)
        {
            Total = total;
            Max = max;
            Occupied = occupied;
            Free = free;
        }
    }

    public class HospitalVentDay
    {
        public int? Total { get; }
        public int? Max { get; }
        public int? Occupied { get; }
        public int? Free { get; }
        public HospitalVentDay(int? total, int? max, int? occupied, int? free)
        {
            Total = total;
            Max = max;
            Occupied = occupied;
            Free = free;
        }
    }

    public class HospitalCareDay
    {
        public int? Total { get; }
        public int? Max { get; }
        public int? Occupied { get; }
        public int? Free { get; }
        public HospitalCareDay(int? total, int? max, int? occupied, int? free)
        {
            Total = total;
            Max = max;
            Occupied = occupied;
            Free = free;
        }
    }
}
