using CriptoBOT.Responses;
using CriptoBOT.Utils;
using Newtonsoft.Json;

namespace CriptoBOT.Api
{
    /// <summary>
    /// By Bit API Class 
    /// For more info check https://bybit-exchange.github.io/docs/v5/intro
    /// </summary>
    public class ByBitApi
    {
        private readonly string _baseAddress;
        private readonly string _apiKey;
        private readonly string _secretKey;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="production">If true redirects to production environment else to test environment</param>
        public ByBitApi(string apiKey, string secretKey, bool production = false)
        {
            if (production)
                _baseAddress = "https://api.bybit.com";
            else
                _baseAddress = "https://api-testnet.bybit.com";

            _apiKey = apiKey;
            _secretKey = secretKey;
        }

        /// <summary>
        /// Tickers API
        /// For more info check https://bybit-exchange.github.io/docs/v5/market/tickers
        /// </summary>
        /// <param name="symbol">The criyto coin symbol, if empty get every coin</param>
        /// <returns></returns>
        public async Task<GetTickersResponse> GetTickers(string symbol = "")
        {
            var url = $"{_baseAddress}/v5/market/tickers";

            var queryString = $"api_key={_apiKey}&category=spot&symbol={symbol}";
            var signature = Functions.GenerateSignature(queryString, _secretKey);
            var fullUrl = $"{url}?{queryString}&sign={signature}";

            using (HttpClient client = new HttpClient())
            {
                var jsonResponse = string.Empty;

                try
                {
                    var response = await client.GetAsync(fullUrl);
                    jsonResponse = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<GetTickersResponse>(jsonResponse);
                }
                catch (Exception ex)
                {
                    return new GetTickersResponse()
                    {
                        Result = null,
                        RetMsg = $"Exception {ex.Message}-{ex.StackTrace}-{jsonResponse}",
                        RetCode = -1,
                    };
                }
            }
        }

        /// <summary>
        /// KLine data API
        /// For more info check https://bybit-exchange.github.io/docs/v5/market/kline
        /// </summary>
        /// <param name="symbol">The crypto coin identifier</param>
        /// <param name="interval">Kline interval. 1,3,5,15,30,60,120,240,360,720,D,M,W</param>
        /// <param name="limit">The start timestamp (ms)</param>
        /// <param name="start">The end timestamp (ms)</param>
        /// <param name="end">Limit for data size per page</param>
        /// <returns></returns>
        public async Task<GetKLineResponse> GetKLine(string symbol, string interval, int limit, DateTimeOffset start, DateTimeOffset end)
        {
            var url = $"{_baseAddress}/v5/market/kline";
            var endTime = end.ToUnixTimeMilliseconds();
            var startTime = start.ToUnixTimeMilliseconds();
            var queryString = $"api_key={_apiKey}&category=spot&symbol={symbol}&interval={interval}&limite={limit}&start={startTime}&end={endTime}";
            var signature = Functions.GenerateSignature(queryString, _secretKey);
            var fullUrl = $"{url}?{queryString}&sign={signature}";

            using (HttpClient client = new HttpClient())
            {
                string jsonResponse = string.Empty;

                try
                {
                    var response = await client.GetAsync(fullUrl);
                    jsonResponse = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<GetKLineResponse>(jsonResponse);
                }
                catch (Exception ex)
                {
                    return new GetKLineResponse()
                    {
                        Result = null,
                        RetMsg = $"Exception {ex.Message}-{ex.StackTrace}-{jsonResponse}",
                        RetCode = -1,
                    };
                }
            }
        }
    }
}
