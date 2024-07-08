using System.Net.Http.Json;

namespace WasmHostedLoggingSerilog.Client.Services;

public interface IHelloService
{
    Task<IList<string>> GetAll();
}

public class HelloService : IHelloService
{
    private readonly HttpClient _client;

    public HelloService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IList<string>> GetAll()
    {
        return await _client.GetFromJsonAsync<IList<string>>("api/hellos") ?? [];
    }
}