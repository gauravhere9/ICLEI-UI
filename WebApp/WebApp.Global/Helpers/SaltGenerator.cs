using System.Security.Cryptography;

namespace WebApp.Global.Helpers
{
    public static class SaltGenerator
    {
        public static string Generate()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
