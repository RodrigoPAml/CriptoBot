using System.Security.Cryptography;
using System.Text;

namespace CriptoBOT.Utils
{
    /// <summary>
    /// Utility Functions
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Format value into percentage string with 4 decimal places (4.444%)
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static string FormatPercentage(decimal percentage)
        {
            return (percentage >= 0)
                   ? $"+{Math.Round(percentage, 3):F3}%"
                   : $"{Math.Round(percentage, 3):F3}%";
        }

        /// <summary>
        /// Format value to minute string (1.1min)
        /// </summary>
        /// <param name="min"></param>
        /// <returns></returns>
        public static string FormatMinute(double min)
        {
            return $"{Math.Round(min, 1):F1}min";
        }

        /// <summary>
        /// Fill a string with n spaces
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string MakeSpace(int n)
        {
            n = Math.Max(1, n);
            return new string(' ', n);
        }

        /// <summary>
        /// Return color to indicate a positive or negative percentage
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static ConsoleColor GetColorForPercentage(string percentage)
        {
            return percentage.StartsWith("-") ? ConsoleColor.Red : ConsoleColor.Green;
        }

        /// <summary>
        /// Signs the message with HMAC algorithm so you prove to the server your identification
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        internal static string GenerateSignature(string message, string secret)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
