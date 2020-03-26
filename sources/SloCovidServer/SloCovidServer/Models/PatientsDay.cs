using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class PatientsDay
    {
        public int DayFromStart { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public GeneralUnit Total { get; }
        public ImmutableDictionary<string, Unit> Facilities { get; }
        public PatientsDay(int dayFromStart, int year, int month, int day, GeneralUnit total, ImmutableDictionary<string, Unit> facilities)
        {
            DayFromStart = dayFromStart;
            Year = year;
            Month = month;
            Day = day;
            Total = total;
            Facilities = facilities;
        }
    }

    public class GeneralUnit: BaseUnit<StateDeceased>
    {
        public int? InCare { get; }
        public OutOfHospital OutOfHospital { get; }
        public GeneralUnit(int? inCare, HospitalMovement inHospital, NeedsO2 needsO2, Movement iCU, Movement critical, StateDeceased deceased,
            OutOfHospital outOfHospital) : base(inHospital, needsO2, iCU, critical, deceased)
        {
            InCare = inCare;
            OutOfHospital = outOfHospital;
        }
    }
    public class Unit: BaseUnit<Deceased>
    {
        public Unit(HospitalMovement inHospital, NeedsO2 needsO2, Movement iCU, Movement critical, Deceased deceased) 
            : base(inHospital, needsO2, iCU, critical, deceased)
        {
        }
    }
    public class BaseUnit<TDeceased>
        where TDeceased: Deceased
    {
        public HospitalMovement InHospital { get; }
        public NeedsO2 NeedsO2 { get; }
        public Movement ICU { get; }
        public Movement Critical { get; }
        public TDeceased Deceased { get; }
        public BaseUnit(HospitalMovement inHospital, NeedsO2 needsO2, Movement iCU, Movement critical, TDeceased deceased)
        {
            InHospital = inHospital;
            NeedsO2 = needsO2;
            ICU = iCU;
            Critical = critical;
            Deceased = deceased;
        }
    }

    public class NeedsO2
    {
        public int? ToDate { get; }
        public NeedsO2(int? toDate)
        {
            ToDate = toDate;
        }
    }

    public class HospitalMovement
    {
        public int? In { get; }
        public int? Out { get; }
        public int? Today { get; }
        public int? ToDate { get; }
        public HospitalMovement(int? inMovement, int? outMovement, int? current, int? today)
        {
            In = inMovement;
            Out = outMovement;
            Today = current;
            ToDate = today;
        }
    }

    public class Movement
    {
        public int? In { get; }
        public int? Out { get; }
        public int? Today { get; }
        public Movement(int? inMovement, int? outMovement, int? today)
        {
            In = inMovement;
            Out = outMovement;
            Today = today;
        }
    }

    public class Deceased
    {
        public int? Today { get; }
        public Deceased(int? today)
        {
            Today = today;
        }
    }

    public class StateDeceased: Deceased
    {
        public int? ToDate { get; }
        public int? Hospital { get; }
        public int? Home { get; }
        public StateDeceased(int? today, int? toDate, int? hospital, int? home) : base(today)
        {
            ToDate = toDate;
            Hospital = hospital;
            Home = home;
        }
    }

    public class OutOfHospital
    {
        public int? ToDate { get; }
        public OutOfHospital(int? toDate)
        {
            ToDate = toDate;
        }
    }
}
