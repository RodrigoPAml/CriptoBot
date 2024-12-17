using System.Net.Mail;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using CriptoBOT.Entities;

namespace CriptoBOT.Utils
{
    public static class EmailSender
    {
        /// <summary>
        /// Send an email with coin resutls
        /// </summary>
        /// <param name="to"></param>
        /// <param name="coinResults"></param>
        /// <param name="elapsed"></param>
        public static void SendCoinResultsAsEmail(List<string> to, List<CoinResult> coinResults, string elapsed)
        {
            StringBuilder htmlMessage = new StringBuilder();

            htmlMessage.AppendLine("<html><body>");
            htmlMessage.AppendLine("<h1>Last Update at " + DateTime.Now.ToString() + "</h1>");
            htmlMessage.AppendLine("<p>Listing coins with good percentage changes</p>");

            htmlMessage.AppendLine("<table border='1' style='border-collapse: collapse;'>");
            htmlMessage.AppendLine($"<tr><th>Coin</th><th>% {elapsed} </th><th>% 5m</th><th>% 30m</th><th>% 1h</th><th>% 24h</th></tr>");

            foreach (var coin in coinResults)
            {
                htmlMessage.AppendLine("<tr>");
                htmlMessage.AppendLine($"<td>{coin.Symbol}</td>");

                htmlMessage.AppendLine($"<td style='color:{GetColorForPercentage(coin.PctBeg)};'>{Functions.FormatPercentage(coin.PctBeg)}</td>");
                htmlMessage.AppendLine($"<td style='color:{GetColorForPercentage(coin.Pct5M)};'>{Functions.FormatPercentage(coin.Pct5M)}</td>");
                htmlMessage.AppendLine($"<td style='color:{GetColorForPercentage(coin.Pct30M)};'>{Functions.FormatPercentage(coin.Pct30M)}</td>");
                htmlMessage.AppendLine($"<td style='color:{GetColorForPercentage(coin.Pct1H)};'>{Functions.FormatPercentage(coin.Pct1H)}</td>");
                htmlMessage.AppendLine($"<td style='color:{GetColorForPercentage(coin.Pct24H)};'>{Functions.FormatPercentage(coin.Pct24H)}</td>");

                htmlMessage.AppendLine("</tr>");
            }

            htmlMessage.AppendLine("</table>");
            htmlMessage.AppendLine("</body></html>");

            Send($"Important Coin Update {DateTime.Now.ToString()}", htmlMessage.ToString(), to, true);
        }

        private static string GetColorForPercentage(decimal percentage)
        {
            return percentage >= 0 ? "green" : "red";
        }

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="to"></param>
        /// <param name="isHtml"></param>
        public static void Send(string title, string message, List<string> to, bool isHtml)
        {
            try
            {
                var jsonFilePath = "settings.json";
                var jsonString = File.ReadAllText(jsonFilePath);
                var jsonObject = JObject.Parse(jsonString);

                var senderEmail = (string)jsonObject["email"];
                var password = (string)jsonObject["emailSecret"];
                var smtp = (string)jsonObject["smpt"];
                var smtpPort = (int)jsonObject["smptPort"];

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.Subject = title;
                mail.Body = message;
                mail.IsBodyHtml = isHtml;

                foreach(var email in to)
                    mail.To.Add(email);

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(senderEmail, password),
                    EnableSsl = true,
                };

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
}
