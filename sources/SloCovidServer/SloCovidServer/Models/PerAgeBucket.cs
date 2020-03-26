namespace SloCovidServer.Models
{
    public class PerAgeBucket
    {
        public int? AgeFrom { get; }
        public int? AgeTo { get; }
        public int? AllToDate { get; }
        public int? FemaleToDate { get; }
        public int? MaleToDate { get; }
        public PerAgeBucket(int? ageFrom, int? ageTo, int? allToDate, int? femaleToDate, int? maleToDate)
        {
            AgeFrom = ageFrom;
            AgeTo = ageTo;
            AllToDate = allToDate;
            FemaleToDate = femaleToDate;
            MaleToDate = maleToDate;
        }
    }
}
