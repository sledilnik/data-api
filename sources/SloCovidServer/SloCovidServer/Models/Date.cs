namespace SloCovidServer.Models
{
    /// <summary>
    /// Represents REST date
    /// </summary>
    public record Date
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public Date(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }
    }
}
