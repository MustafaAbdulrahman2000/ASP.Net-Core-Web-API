using Microsoft.AspNetCore.Mvc;
using Module_01.Requests;

namespace Module_01.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController: ControllerBase
{
    [HttpPost]
    public IActionResult Post(CreateProductRequest request)
    {
        /*
        
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        
        */

        return Created($"/api/products/{Guid.NewGuid()}", request);
    }
}
