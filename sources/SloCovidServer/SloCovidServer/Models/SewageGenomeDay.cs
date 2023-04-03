namespace SloCovidServer.Models
{
    public record SewageGenomeDay(int Year, int Month, int Day, string Station, string Genome, float? Ratio,
        string Region);
}