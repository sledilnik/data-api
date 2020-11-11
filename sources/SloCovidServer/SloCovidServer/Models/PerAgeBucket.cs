namespace SloCovidServer.Models
{
    public record PerAgeBucket
    {
        public int? AgeFrom { get; init; }
        public int? AgeTo { get; init; }
        public int? AllToDate { get; init; }
        public int? FemaleToDate { get; init; }
        public int? MaleToDate { get; init; }
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
