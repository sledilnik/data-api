namespace SloCovidServer.Models
{
    public record PerRegion
    {
        public int? LJSum { get; init; }
        public int? CESum { get; init; }
        public int? NMSum { get; init; }
        public int? MBSum { get; init; }
        public int? KRSum { get; init; }
        public int? SGSum { get; init; }
        public int? POSum { get; init; }
        public int? MSSum { get; init; }
        public int? KPSum { get; init; }
        public int? NGSum { get; init; }
        public int? KKSum { get; init; }
        public int? ForeignSum { get; init; }
        public int? UnknownSum { get; init; }
        public PerRegion(int? lJSum, int? cESum, int? nMSum, int? mBSum, int? kRSum, int? sGSum, int? pOSum, int? mSSum, int? kPSum,
            int? nGSum, int? kKSum, int? foreignSum, int? unknownSum)
        {
            LJSum = lJSum;
            CESum = cESum;
            NMSum = nMSum;
            MBSum = mBSum;
            KRSum = kRSum;
            SGSum = sGSum;
            POSum = pOSum;
            MSSum = mSSum;
            KPSum = kPSum;
            NGSum = nGSum;
            KKSum = kKSum;
            ForeignSum = foreignSum;
            UnknownSum = unknownSum;
        }
    }
}
