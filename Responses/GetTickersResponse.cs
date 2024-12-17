namespace CriptoBOT.Responses
{
    /// <summary>
    /// Represents the response for KLine endpoint v5/market/tickers
    /// </summary>
    public class GetTickersResponse : BaseResponse<TickerResultData>
    {
    }

    /// <summary>
    /// Represents the result from the tickers response
    /// </summary>
    public class TickerResultData
    {
        public string Category {  get; set; }   
        public List<TickerCoin> List { get; set; }
    }

    /// <summary>
    /// Coin data
    /// </summary>
    public class TickerCoin
    {
        public string Symbol { get; set; }
        public decimal LastPrice { get; set; }
        public decimal HighPrice24h { get; set; }
        public decimal LowPrice24h { get; set; }
        public decimal Price24hPcnt { get; set; }
        public decimal Volume24h { get; set; }
    }
}
