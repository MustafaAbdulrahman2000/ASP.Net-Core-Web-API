using Microsoft.AspNetCore.Mvc;
using Module_01.Services;

namespace Module_01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(OrderService orderService): ControllerBase
{
    [HttpGet("{orderId:guid}")]
    public IActionResult Process(Guid orderId)
    {
        var userId = Guid.Parse("b7cc98df-bcce-4bed-be26-d734797549c8");

        orderService.ProcessOrder(orderId, userId);

        return Ok(new
        {
            OrderId = orderId,
            Status = "Processed"
        });
    }
}
