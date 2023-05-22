using Microsoft.AspNetCore.Mvc;
using RabbitMqProducer.RabbitMq;

[Route("api/[controller]")]
[ApiController]
public class RabbitMqController : ControllerBase
{
    private readonly IRabbitMqService _mqService;

    public RabbitMqController(IRabbitMqService mqService)
    {
        _mqService = mqService;
    }

    [HttpGet]
    public IActionResult SendMessage()
    {
        _mqService.SendMessageAsync();

        return Ok();
    }
}