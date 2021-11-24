namespace SloCovidServer.Models
{
    public record PerAgeBucket
    {
        public int? AgeFrom { get; init; }
        public int? AgeTo { get; init; }
        public int? AllToDate { get; init; }
        public int? FemaleToDate { get; init; }
        public int? MaleToDate { get; init; }
        public int? Administered { get; init; }
        public int? Administered2nd { get; init; }
        public int? Administered3rd { get; init; }
        public PerAgeBucket(int? ageFrom, int? ageTo, int? allToDate, int? femaleToDate, int? maleToDate, int? administered, int? administered2nd, int? administered3rd)
        {
            AgeFrom = ageFrom;
            AgeTo = ageTo;
            AllToDate = allToDate;
            FemaleToDate = femaleToDate;
            MaleToDate = maleToDate;
            Administered = administered;
            Administered2nd = administered2nd;
            Administered3rd = administered3rd;
        }
    }
}
