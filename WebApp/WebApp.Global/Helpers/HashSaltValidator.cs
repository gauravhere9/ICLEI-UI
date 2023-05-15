namespace WebApp.Global.Helpers
{
    public static class HashSaltValidator
    {
        public static bool Validate(string value, string salt, string hash)
        {
            string newHash = HashGenerator.Generate(value, salt);

            if (newHash != hash)
            {
                return false;
            }
            return true;
        }
    }
}
