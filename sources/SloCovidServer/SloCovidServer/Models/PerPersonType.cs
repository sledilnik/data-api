namespace SloCovidServer.Models
{
    public record PerPersonType
    {
        public int? RhOccupant { get; init; }
        public int? Other { get; init; }
        public PerPersonType(int? rhOccupant, int? other)
        {
            RhOccupant = rhOccupant;
            Other = other;
        }
    }
}
