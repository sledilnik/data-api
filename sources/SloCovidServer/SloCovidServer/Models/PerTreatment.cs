namespace SloCovidServer.Models
{
    public record PerTreatment
    {
        public int? InHospital { get; init; }
        public int? InHospitalToDate { get; init; }
        public int? InICU { get; init; }
        public int? Critical { get; init; }
        public int? DeceasedToDate { get; init; }
        public int? Deceased { get; init; }
        public int? OutOfHospitalToDate { get; init; }
        public int? OutOfHospital { get; init; }
        public PerTreatment(int? inHospital, int? inHospitalToDate, int? inICU, int? critical, int? deceasedToDate, int? deceased, int? outOfHospitalToDate,
            int? outOfHospital)
        {
            InHospital = inHospital;
            InHospitalToDate = inHospitalToDate;
            InICU = inICU;
            Critical = critical;
            DeceasedToDate = deceasedToDate;
            Deceased = deceased;
            OutOfHospitalToDate = outOfHospitalToDate;
            OutOfHospital = outOfHospital;
        }
    }
}
