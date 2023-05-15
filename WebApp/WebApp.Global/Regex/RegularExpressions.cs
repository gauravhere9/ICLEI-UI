namespace WebApp.Global.Regex
{
    public static class RegularExpressions
    {
        public const string Password = "^.*(?=.{8,16})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@@#$%&]).*$";
        public const string Email = "^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$";

    }
}
