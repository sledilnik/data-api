using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class PatientsDay : IModelDate
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
        public OutOfHospital OutOfHospital { get; }
        public GeneralUnit(HospitalMovement inHospital, HospitalMovement iCU, HospitalMovement critical, StateDeceased deceased, HospitalMovement care, ToDateToday deceasedCare,
            OutOfHospital outOfHospital) : base(inHospital, iCU, critical, deceased, care, deceasedCare)
        {
            OutOfHospital = outOfHospital;
        }
    }
    public class Unit: BaseUnit<HospitalDeceased>
    {
        public Unit(HospitalMovement inHospital, HospitalMovement iCU, HospitalMovement critical, HospitalDeceased deceased, HospitalMovement care, ToDateToday deceasedCare) 
            : base(inHospital, iCU, critical, deceased, care, deceasedCare)
        {
        }
    }
    public class BaseUnit<TDeceased>
        where TDeceased: Deceased
    {
        public HospitalMovement InHospital { get; }
        public HospitalMovement ICU { get; }
        public HospitalMovement Critical { get; }
        public TDeceased Deceased { get; }
        public HospitalMovement Care { get; }
        public ToDateToday DeceasedCare { get; }
        public BaseUnit(HospitalMovement inHospital, HospitalMovement iCU, HospitalMovement critical, TDeceased deceased, HospitalMovement care, ToDateToday deceasedCare)
        {
            InHospital = inHospital;
            ICU = iCU;
            Critical = critical;
            Deceased = deceased;

            Care = care;
            DeceasedCare = deceasedCare;
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
        public int? ToDate { get; }
        public Deceased(int? today, int? toDate)
        {
            Today = today;
            ToDate = toDate;
        }
    }

    public class StateDeceased: Deceased
    {
        public class HospitalStats: ToDateToday
        {
            public ToDateToday Icu { get; }
            public HospitalStats(int? today, int? toDate, ToDateToday icu) : base(today, toDate)
            {
                Icu = icu;
            }
        }
        public HospitalStats Hospital { get; }
        public ToDateToday Care { get; }
        public ToDateToday Home { get; }
        public StateDeceased(int? today, int? toDate, HospitalStats hospital, ToDateToday care, ToDateToday home) : base(today, toDate)
        {
            Hospital = hospital;
            Care = care;
            Home = home;
        }
    }
    public class HospitalDeceased: Deceased
    {
        public ToDateToday Icu { get; }
        public HospitalDeceased(int? today, int? toDate, ToDateToday icu) : base(today, toDate)
        {
            Icu = icu;
        }
    }

    public class ToDateToday
    {
        public int? Today { get; }
        public int? ToDate { get; }
        public ToDateToday(int? today, int? toDate)
        {
            ToDate = toDate;
            Today = today;
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
