using Newtonsoft.Json.Linq;
using CriptoBOT.Bots;

namespace CriptoBOT
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configs
            var jsonFilePath = "settings.json";
            var jsonString = File.ReadAllText(jsonFilePath);
            var jsonObject = JObject.Parse(jsonString);

            bool production = true;
            var emailsToSend = new List<string>() {
                 "your_email@domain.com",
            };

            var percentage24h = 25.0m; // Shows coins that have increased their value in 25% in the last 24 hours
            var percentageIncrease = 1.0m; // Shows coins that have increased their value in 1% since the program started

            CheckCoinIncreaseBot bot = new CheckCoinIncreaseBot(
                (string)jsonObject["apiKey"], 
                (string)jsonObject["apiSecret"], 
                production, 
                percentage24h, 
                percentageIncrease, 
                emailsToSend
            );

            await bot.Run();
        }
    }
}
