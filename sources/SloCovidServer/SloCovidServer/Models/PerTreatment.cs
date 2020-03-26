namespace SloCovidServer.Models
{
    public class PerTreatment
    {
        public int? InHospital { get; }
        public int? InICU { get; }
        public int? Critical { get; }
        public int? DeceasedToDate { get; }
        public int? Deceased { get; }
        public int? OutOfHospitalToDate { get; }
        public int? OutOfHospital { get; }
        public int? RecoveredToDate { get; }
        public PerTreatment(int? inHospital, int? inICU, int? critical, int? deceasedToDate, int? deceased, int? outOfHospitalToDate,
            int? outOfHospital, int? recoveredToDate)
        {
            InHospital = inHospital;
            InICU = inICU;
            Critical = critical;
            DeceasedToDate = deceasedToDate;
            Deceased = deceased;
            OutOfHospitalToDate = outOfHospitalToDate;
            OutOfHospital = outOfHospital;
            RecoveredToDate = recoveredToDate;
        }
    }
}
