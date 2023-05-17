﻿namespace WebApp.Global.Extensions
{
    public static class StringExtensions
    {
        public static string GetInitials(this string value)
           => string.Concat(value
              .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
              .Where(x => x.Length >= 1 && char.IsLetter(x[0]))
              .Select(x => char.ToUpper(x[0])));
    }
}
