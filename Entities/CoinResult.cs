namespace CriptoBOT.Entities
{
    /// <summary>
    /// Represents the increase of value in a cripto coin by intervals
    /// </summary>
    public class CoinResult
    {
        public string Symbol { get; set; }
        public decimal PctBeg { get; set; }
        public decimal Pct5M { get; set; }
        public decimal Pct30M { get; set; }
        public decimal Pct1H { get; set; }
        public decimal Pct24H { get; set; }
    }
}
