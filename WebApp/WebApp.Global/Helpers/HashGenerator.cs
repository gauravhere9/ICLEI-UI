using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace WebApp.Global.Helpers
{
    public static class HashGenerator
    {
        public static string Generate(string value, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }
    }
}
