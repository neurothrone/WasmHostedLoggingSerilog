using Microsoft.AspNetCore.Mvc;

namespace WasmHostedLoggingSerilog.Server.Controllers;

// [Route("api/[controller]")]
[Route("api/hellos")]
[ApiController]
public class HelloController : ControllerBase
{
    private readonly ILogger<HelloController> _logger;

    private readonly IList<string> _hellos = ["Hi", "Hey", "Hello"];

    public HelloController(ILogger<HelloController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IList<string> GetAll()
    {
        _logger.LogWarning("Get all hellos from Controller");

        return _hellos;
    }
}