using Microsoft.AspNetCore.Mvc;
using Module_01.Filters;

namespace Module_01.Controllers;

[ApiController]
[Route("/api/products")]
// [TrackActionTimeFilterV2]
// [TrackActionTimeFilterV3]
public class ProductController: ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] { "Keyboard [$58.99]", "Mouse [$39.99]" });
    }
}