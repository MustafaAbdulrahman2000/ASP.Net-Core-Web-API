using Microsoft.AspNetCore.Mvc;
using Module_01.Models;
using Module_01.Requests;

namespace Module_01.Controllers;

[ApiController]
public class ProductController: ControllerBase
{
    /*

    [HttpGet("product-controller/{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok(id);
    }

    [HttpGet("/product-controller")]
    public IActionResult Get(int page, int pageSize)
    {
        return Ok($"Showing {pageSize} items of page # {page}");
    }

    [HttpGet("/product-controller-complex-query")]
    public IActionResult GetComplexQuery([FromQuery] SearchRequest request)
    {
        return Ok(request);
    }

    [HttpGet("/product-controller-array")]
    public IActionResult GetArray([FromQuery] Guid[] ids)
    {
        return Ok(ids);
    }

    [HttpGet("/date-range-controller")]
    public IActionResult GetComplexQuery(DateRangeQuery dateRange)
    {
        return Ok(dateRange);
    }

    [HttpGet("/product-controller")]
    public IActionResult GetHeader([FromHeader(Name = "X-Api-Version")] string apiVersion)
    {
        return Ok($"API version : {apiVersion}");
    }

        [HttpGet("/product-controller")]
    public IActionResult Get(string name, decimal price)
    {
        return Ok(new { name, price});
    }

    [HttpPost("/upload-controller")]
    public async Task<IActionResult> Post(IFormFile file)
    {
        if(file is null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uplaods = Path.Combine(Directory.GetCurrentDirectory(), "uplaods");
        
        Directory.CreateDirectory(uplaods);
        
        var path = Path.Combine(uplaods, file.FileName);
        
        using var stream = new FileStream(path, FileMode.Create);

        await file.CopyToAsync(stream);

        return Ok("Uploaded");
    }

    [HttpPost("/product-controller")]
    public IActionResult Post(ProductRequest request)
    {
        return Ok(request);
    }

    */

    [HttpGet("/preferences")]
    public IActionResult GetPreferences()
    {
        var theme = HttpContext.Request.Cookies["theme"];
        var language = HttpContext.Request.Cookies["language"];
        var timezone = HttpContext.Request.Cookies["timeZone"];

        return Ok(new {
            Theme = theme,
            Language = language,
            Timezone = timezone
        });
    } 
}