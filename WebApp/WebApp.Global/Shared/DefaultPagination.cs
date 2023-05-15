namespace WebApp.Global.Shared
{
    public static class DefaultPagination
    {
        public static int PageSize { get; set; } = 10;
        public static int PageIndex { get; set; } = 1;
        public static string OrderBy { get; set; } = "Id";
        public static string OrderByDirection { get; set; } = "Desc";
    }
}
