namespace WebApp.Global.Shared
{
    public interface IPaginationRequestDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
    }
}
