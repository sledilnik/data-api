namespace SloCovidServer.Models
{
    public record DailyDeathsSlovenia : IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public int Deceased { get; init; }
    }
}
