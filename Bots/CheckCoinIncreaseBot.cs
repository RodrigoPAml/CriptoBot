using CriptoBOT.Api;
using CriptoBOT.Entities;
using CriptoBOT.Responses;
using CriptoBOT.Utils;

namespace CriptoBOT.Bots
{
    /// <summary>
    /// This bot lists the top 10 coins that have an increase in value in the last 24 hours greater than <see cref="_price24HIncrease"/> or
    /// an increase greater than <see cref="_priceIncrease"/> since the start of the bot 
    /// This bot also send email every 1 minute to inform the changes
    /// </summary>
    public class CheckCoinIncreaseBot : BaseBot
    {
        private readonly ByBitApi _api = null;
        private List<string> _recipients = new List<string>();
        private List<TickerCoin> _initialCoins = null;
        private DateTime _initialTime = DateTime.Now;
        private DateTime _initialTimeEmail = DateTime.Now;
        private readonly decimal _price24HIncrease;
        private readonly decimal _priceIncrease;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="production">Production environemnt or not</param>
        /// <param name="priceIncrease24H">Shows coins that have increased their value in 25% in the last 24 hours</param>
        /// <param name="priceIncrease">Shows coins that have increased their value in 1% since the program started</param>
        /// <param name="recipients">Emails to warn about coin changes</param>
        public CheckCoinIncreaseBot(string apiKey, string apiSecret, bool production, decimal priceIncrease24H, decimal priceIncrease, List<string> recipients)
        {
            _api = new ByBitApi(apiKey, apiSecret, true);

            _price24HIncrease = priceIncrease24H;
            _priceIncrease = priceIncrease;
            _recipients = recipients;
        }

        /// <summary>
        /// Tick the bot
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> Tick()
        {
            Thread.Sleep(1000);

            var response = await _api.GetTickers();

            if (!response.Success)
                return false;

            var currentCoins = response.Result.List
                .ToList();

            if(_initialCoins == null)
            {
                _initialCoins = currentCoins.ToList();
                _initialTime = DateTime.Now;
                return true;
            }

            var output = new List<OutputItem>();
            var result = new List<CoinResult>();

            foreach (var currentCoin in currentCoins.OrderByDescending(x => x.Price24hPcnt).Take(10).ToList())
            {
                var initialCoin = _initialCoins.Find(x => x.Symbol == currentCoin.Symbol); 

                if(initialCoin == null)
                {
                    _initialCoins.Add(currentCoin);
                    initialCoin = currentCoin;
                }

                var priceDifferencePercentage = ((currentCoin.LastPrice - initialCoin.LastPrice) / initialCoin.LastPrice) * 100;
                var priceDiff = Functions.FormatPercentage(priceDifferencePercentage);
                var elapsedMin = Functions.FormatMinute((DateTime.Now - _initialTime).TotalMinutes);

                if(priceDifferencePercentage < _priceIncrease && (currentCoin.Price24hPcnt * 100) < _price24HIncrease)
                {
                    continue;
                }

                output.Add(new() { Message = $"{currentCoin.Symbol} {Functions.MakeSpace(15 - currentCoin.Symbol.Count())}", Color = ConsoleColor.White });

                output.Add(new() { Message = $"acc ({elapsedMin} ago): ", Color = ConsoleColor.White });
                output.Add(new() { Message = $"{priceDiff} {Functions.MakeSpace(15 - priceDiff.Count())}", Color = Functions.GetColorForPercentage(priceDiff) });

                var resp5 = await _api.GetKLine(currentCoin.Symbol, "5", 1, DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow);
                var resp5Str = Functions.FormatPercentage(resp5.Result.CalcDiff());

                output.Add(new() { Message = $"5m: ", Color = ConsoleColor.White });
                output.Add(new() { Message = $"{resp5Str} {Functions.MakeSpace(13 - resp5Str.Count())}", Color = Functions.GetColorForPercentage(resp5Str) });

                var resp30 = await _api.GetKLine(currentCoin.Symbol, "30", 1, DateTimeOffset.UtcNow.AddMinutes(-30), DateTimeOffset.UtcNow);
                var resp30Str = Functions.FormatPercentage(resp30.Result.CalcDiff());

                output.Add(new() { Message = $"30m: ", Color = ConsoleColor.White });
                output.Add(new() { Message = $"{resp30Str} {Functions.MakeSpace(13 - resp30Str.Count())}", Color = Functions.GetColorForPercentage(resp30Str) });

                var resp1h = await _api.GetKLine(currentCoin.Symbol, "60", 1, DateTimeOffset.UtcNow.AddMinutes(-60), DateTimeOffset.UtcNow);
                var resp1hStr = Functions.FormatPercentage(resp1h.Result.CalcDiff());

                output.Add(new() { Message = $"1hr: ", Color = ConsoleColor.White });
                output.Add(new() { Message = $"{resp1hStr} {Functions.MakeSpace(13 - resp1hStr.Count())}", Color = Functions.GetColorForPercentage(resp1hStr) });

                var resp24h = Functions.FormatPercentage(currentCoin.Price24hPcnt * 100);

                output.Add(new() { Message = $"24h: ", Color = ConsoleColor.White });
                output.Add(new() { Message = $"{resp24h} {Functions.MakeSpace(15 - resp24h.Count())}\n", Color = Functions.GetColorForPercentage(resp24h) });

                result.Add(new CoinResult()
                {
                    Symbol = currentCoin.Symbol,
                    PctBeg = priceDifferencePercentage,
                    Pct5M = resp5.Result.CalcDiff(),
                    Pct30M = resp30.Result.CalcDiff(),
                    Pct1H = resp1h.Result.CalcDiff(),
                    Pct24H = currentCoin.Price24hPcnt * 100,
                });
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Last update at {DateTime.Now}");
            Console.WriteLine($"Listing top 10 coins with Price24hPcnt > {_price24HIncrease} or AccumulatedPrice > {_priceIncrease}\n");

            foreach (var line in output)
            {
                Console.ForegroundColor = line.Color;
                Console.Write(line.Message);
            }

            SendEmail(result);

            return true;
        }

        private void SendEmail(List<CoinResult> results)
        {
            bool worthSending = results.Any(x =>
                x.PctBeg > 10.0m ||
                x.Pct5M > 15.0m ||
                x.Pct30M > 40.0m
            );

            if ((DateTime.Now - _initialTimeEmail).TotalMinutes > 1 && worthSending)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Sending emails");

                var elapsed = Functions.FormatMinute((DateTime.Now - _initialTime).TotalMinutes);
                EmailSender.SendCoinResultsAsEmail(_recipients, results, elapsed);

                _initialTimeEmail = DateTime.Now;
            }
        }
    }
}
