namespace SloCovidServer.Models
{
    public class PerRegion
    {
        public int? LJSum { get; }
        public int? CESum { get; }
        public int? NMSum { get; }
        public int? MBSum { get; }
        public int? KRSum { get; }
        public int? SGSum { get; }
        public int? POSum { get; }
        public int? MSSum { get; }
        public int? KPSum { get; }
        public int? NGSum { get; }
        public int? KKSum { get; }
        public int? ForeignSum { get; }
        public int? UnknownSum { get; }
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
