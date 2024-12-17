namespace CriptoBOT.Responses
{
    /// <summary>
    /// Represents the response for KLine endpoint v5/market/kline
    /// </summary>
    public class GetKLineResponse : BaseResponse<KLineResultData>
    {
    }

    /// <summary>
    /// The result data for the KLine endpoint
    /// </summary>
    public class KLineResultData
    {
        public string Category { get; set; }
        public string Symbol { get; set; }
        public List<List<decimal>> List { get; set; }

        /// <summary>
        /// Friendlt format for the array returned
        /// </summary>
        public List<KLineEachData> ReadableList => List?.Select(data => new KLineEachData
        {
            StartTime = DateTimeOffset.FromUnixTimeMilliseconds((long)data[0]).UtcDateTime, 
            OpenPrice = data[1],
            HighPrice = data[2],
            LowPrice = data[3],
            ClosePrice = data[4],
        })
        .ToList();

        /// <summary>
        /// Calculates the difference between the prices
        /// Return the percentage (0-100%) in changes 
        /// </summary>
        /// <returns></returns>
        public decimal CalcDiff()
        {
            var prev = ReadableList.Last().ClosePrice;
            var current = ReadableList.First().ClosePrice;
            return ((current - prev) / prev) * 100;
        }
    }

    /// <summary>
    /// Friendly format for the array returned
    /// </summary>
    public class KLineEachData
    {
        public DateTime StartTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
    }
}
