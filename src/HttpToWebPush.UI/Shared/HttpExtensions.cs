using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HttpToWebPush.UI.Shared;

public static class HttpClientJsonExtensions
{
    public static Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(this HttpClient client, string? requestUri, TValue value)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        request.Content = JsonContent.Create(value);
        return client.SendAsync(request);
    }
}