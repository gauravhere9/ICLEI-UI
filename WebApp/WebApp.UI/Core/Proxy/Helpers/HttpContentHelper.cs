using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using WebApp.Global.Response;

namespace WebApp.UI.Core.Proxy.Helpers
{
    public static class HttpContentHelper<T>
    {
        public static HttpContent GetHttpRequestContentFromModel(object model)
        {
            return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        }

        public async static Task<ApiResponse<T>> GetModelFromHttpResponseContent(HttpContent content)
        {
            try
            {
                var stringResult = await content.ReadAsStringAsync();

                var result = JObject.Parse(stringResult);

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(result.ToString());

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>((int)HttpStatusCode.BadRequest) { Message = ex.Message };
            }
        }
    }
}
