namespace WebApp.Global.Shared
{
    public class PagedResponseDto<T> where T : class
    {
        public PagedResponseDto()
        {
            this.List = new List<T>();
        }

        public IList<T> List { get; set; }
        public int TotalRecords { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
