namespace WebApp.Global.Response
{
    public class ApiResponse<T>
    {
        public ApiResponse(int statusCode)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public bool Success { get { return (StatusCode == 200); } }
        public IEnumerable<string>? Errors { get; set; }
    }
}
