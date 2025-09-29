using System.Text;
using Microsoft.AspNetCore.Mvc;
using Module_01.Data;
using Module_01.Models;
using Module_01.Responses.V1;

namespace Module_01.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/products")]
// [Route("api/v{version:apiVersion}/products")]
public class ProductController(ProductRepository repository) : ControllerBase
{
    [HttpGet("{productId}")]
    public ActionResult<ProductResponse> GetProduct(Guid productId)
    {
        Response.Headers["Deprecation"] = "true";
        Response.Headers["Sunset"] = "Wed, 31 Dec 2025 23:59:59 GMT";
        Response.Headers["Link"] = "<api/v2/products>; rel=\"successor-version\"";

        var product = repository.GetProductById(productId);

        if (product is null)
            return NotFound();

        return Ok(ProductResponse.FromModel(product));
    }
}