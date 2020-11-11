using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record PatientsDay : IModelDate
    {
        public int DayFromStart { get; init; }
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public GeneralUnit Total { get; init; }
        public ImmutableDictionary<string, Unit> Facilities { get; init; }
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

    public record GeneralUnit : BaseUnit<StateDeceased>
    {
        public OutOfHospital OutOfHospital { get; init; }
        public GeneralUnit(HospitalMovement inHospital, HospitalMovement iCU, HospitalMovement critical, StateDeceased deceased, HospitalMovement care, ToDateToday deceasedCare,
            OutOfHospital outOfHospital) : base(inHospital, iCU, critical, deceased, care, deceasedCare)
        {
            OutOfHospital = outOfHospital;
        }
    }
    public record Unit : BaseUnit<HospitalDeceased>
    {
        public Unit(HospitalMovement inHospital, HospitalMovement iCU, HospitalMovement critical, HospitalDeceased deceased, HospitalMovement care, ToDateToday deceasedCare)
            : base(inHospital, iCU, critical, deceased, care, deceasedCare)
        {
        }
    }
    public record BaseUnit<TDeceased>
        where TDeceased : Deceased
    {
        public HospitalMovement InHospital { get; init; }
        public HospitalMovement ICU { get; init; }
        public HospitalMovement Critical { get; init; }
        public TDeceased Deceased { get; init; }
        public HospitalMovement Care { get; init; }
        public ToDateToday DeceasedCare { get; init; }
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

    public record HospitalMovement
    {
        public int? In { get; init; }
        public int? Out { get; init; }
        public int? Today { get; init; }
        public int? ToDate { get; init; }
        public HospitalMovement(int? inMovement, int? outMovement, int? current, int? today)
        {
            In = inMovement;
            Out = outMovement;
            Today = current;
            ToDate = today;
        }
    }

    public record Movement
    {
        public int? In { get; init; }
        public int? Out { get; init; }
        public int? Today { get; init; }
        public Movement(int? inMovement, int? outMovement, int? today)
        {
            In = inMovement;
            Out = outMovement;
            Today = today;
        }
    }

    public record Deceased
    {
        public int? Today { get; init; }
        public int? ToDate { get; init; }
        public Deceased(int? today, int? toDate)
        {
            Today = today;
            ToDate = toDate;
        }
    }

    public record StateDeceased : Deceased
    {
        public record HospitalStats : ToDateToday
        {
            public ToDateToday Icu { get; init; }
            public HospitalStats(int? today, int? toDate, ToDateToday icu) : base(today, toDate)
            {
                Icu = icu;
            }
        }
        public HospitalStats Hospital { get; init; }
        public ToDateToday Care { get; init; }
        public ToDateToday Home { get; init; }
        public StateDeceased(int? today, int? toDate, HospitalStats hospital, ToDateToday care, ToDateToday home) : base(today, toDate)
        {
            Hospital = hospital;
            Care = care;
            Home = home;
        }
    }
    public record HospitalDeceased : Deceased
    {
        public ToDateToday Icu { get; init; }
        public HospitalDeceased(int? today, int? toDate, ToDateToday icu) : base(today, toDate)
        {
            Icu = icu;
        }
    }

    public record ToDateToday
    {
        public int? Today { get; init; }
        public int? ToDate { get; init; }
        public ToDateToday(int? today, int? toDate)
        {
            ToDate = toDate;
            Today = today;
        }
    }

    public record OutOfHospital
    {
        public int? ToDate { get; init; }
        public OutOfHospital(int? toDate)
        {
            ToDate = toDate;
        }
    }
}
