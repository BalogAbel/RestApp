using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestApp.Util
{
    internal class Helper
    {
        public static string Hash(string value)
        {
            using (var hash = SHA256.Create())
            {
                return string.Join("", hash
                    .ComputeHash(Encoding.UTF8.GetBytes(value))
                    .Select(item => item.ToString("x2")));
            }
        }
    }
}