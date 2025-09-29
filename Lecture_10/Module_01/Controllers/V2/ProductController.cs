using System.Text;
using Microsoft.AspNetCore.Mvc;
using Module_01.Data;
using Module_01.Models;
using Module_01.Responses.V2;

namespace Module_01.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/products")]
// [Route("api/v{version:apiVersion}/products")]
public class ProductController(ProductRepository repository) : ControllerBase
{
    [HttpGet("{productId}")]
    public ActionResult<Product> GetProduct(Guid productId)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return NotFound();

        return Ok(ProductResponse.FromModel(product));
    }
}