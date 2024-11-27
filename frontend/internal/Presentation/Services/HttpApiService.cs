using System.Text.Json;

namespace AppointmentScheduler.Presentation.Services;

public abstract class HttpApiService(
    HttpClient client, IHttpContextAccessor contextAccessor,
    ILogger<HttpApiService> logger, JsonSerializerOptions options)
{
    public HttpContext Context => contextAccessor.HttpContext;
    public string SerializeJson<T>(T value) => JsonSerializer.Serialize<T>(value, options);
    public void SerializeJson<T>(Stream utf8Json, T value) => JsonSerializer.Serialize<T>(utf8Json, value, options);
    public void SerializeJson<T>(Utf8JsonWriter writer, T value) => JsonSerializer.Serialize(writer, value, options);
    public Task SerializeJsonAsync<T>(Stream utf8Json, T value, CancellationToken cancellationToken = default) => JsonSerializer.SerializeAsync<T>(utf8Json, value, options, cancellationToken);
    public T DeserializeJson<T>(string json) => JsonSerializer.Deserialize<T>(json, options);
    public T DeserializeJson<T>(Stream utf8Json) => JsonSerializer.Deserialize<T>(utf8Json, options);
    public T DeserializeJson<T>(ref Utf8JsonReader reader) => JsonSerializer.Deserialize<T>(ref reader, options);
    public ValueTask<T> DeserializeAsyncJson<T>(Stream utf8Json, CancellationToken cancellationToken = default) => JsonSerializer.DeserializeAsync<T>(utf8Json, options, cancellationToken);
    public HttpResponseMessage Send(HttpRequestMessage request) => client.Send(request);
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => client.SendAsync(request);
}