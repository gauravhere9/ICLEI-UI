using System.Web;

namespace WebApp.Global.Extensions
{
    public static class StringExtensions
    {
        public static string GetInitials(this string value)
           => string.Concat(value
              .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
              .Where(x => x.Length >= 1 && char.IsLetter(x[0]))
              .Select(x => char.ToUpper(x[0])));


        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null && !string.IsNullOrWhiteSpace(p.GetValue(obj, null).ToString())
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
