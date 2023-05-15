using Microsoft.AspNetCore.Http;

namespace WebApp.Global.Helpers
{
    public static class HttpRequestHelper
    {
        public async static Task<string> RequestBodyToString(HttpRequest request)
        {
            var body = "";

            //We need to enable rewind to be able to read the request body multiple times
            request.EnableBuffering();

            using (var requestStream = new MemoryStream())
            {
                // Copy the body to a separate stream
                await request.Body.CopyToAsync(requestStream);

                // Read the stream
                body = await ReadStreamInChunks(requestStream);
            }

            // Rewind the body stream so that it can be reused by other layers
            request.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }

        public static async Task<string> ReadStreamInChunks(Stream stream)
        {
            const int ReadChunkBufferLength = 4096;
            string result;

            stream.Seek(0, SeekOrigin.Begin);
            using (var textWriter = new StringWriter())
            {
                using (var reader = new StreamReader(stream))
                {
                    var readChunk = new char[ReadChunkBufferLength];
                    int readChunkLength;
                    do
                    {
                        readChunkLength = await reader.ReadBlockAsync(readChunk, 0, ReadChunkBufferLength);
                        await textWriter.WriteAsync(readChunk, 0, readChunkLength);
                    } while (readChunkLength > 0);

                    result = textWriter.ToString();
                }
            }

            return result;
        }
    }
}
