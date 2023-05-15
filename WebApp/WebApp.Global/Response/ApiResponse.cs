namespace WebApp.Global.Response
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string? message = null, object? data = null)
        {
            StatusCode = statusCode;
            Message = message ?? DefaultMessageByStatusCode(statusCode);
            Data = data;
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public bool Success { get { return (StatusCode == 200); } }
        public IEnumerable<string>? Errors { get; set; }
        private string DefaultMessageByStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Seccess",
                400 => "A bad request, you have made",
                401 => "Unauthorized",
                412 => "Precondition failed",
                404 => "Resource not found",
                500 => "Internal server error",
                _ => ""
            };
        }
    }
}
