using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Module_01.Controllers;

[ApiController]
[Route("/api/controller-fake-errors")]
public class FakeErrorController: ControllerBase
{
    /*

        [HttpPost("bad-request")]
        public IActionResult BadRequestExample() => BadRequest("Product SKU is required.");

        [HttpPost("bad-request-no-body")]
        public IActionResult BadRequestNoBodyExample() => BadRequest();

        [HttpPost("not-found")]
        public IActionResult NotFoundExample() => NotFound("Product not found.");

        [HttpPost("unauthorized")]
        public IActionResult UnauthorizedExample() => Unauthorized();

        [HttpPost("conflict")]
        public IActionResult ConflictExample() => Conflict("This product already exists.");


        [HttpPost("bad-request")]
        public IActionResult BadRequestExample() => 
        BadRequest(new ProblemDetails
        {
            Title = "Product SKU is required."
        });

        [HttpPost("bad-request-2")]
        public IActionResult BadRequestExample2() => 
        Problem
        (
            type: "https://example.com/probs/sku-required",
            title: "Bad Request",
            detail: "Product SKU is required.",
            statusCode: StatusCodes.Status400BadRequest
        );

        [HttpPost("not-found")]
        public IActionResult NotFoundExample() => 
        NotFound(new ProblemDetails
        {
            Title = "Product not found."
        });

        [HttpPost("unauthorized")]
        public IActionResult UnauthorizedExample() => 
        Unauthorized(new ProblemDetails
        {
            Title = "Unauthorized."
        });

        [HttpPost("conflict")]
        public IActionResult ConflictExample() => 
        Conflict(new ProblemDetails
        {
            Title = "This product already exists."
        });

    */

    [HttpGet("server-error")]
    public IActionResult ServerErrorExample()
    {
        System.IO.File.ReadAllText("E:\\something.json");

        return Ok();
    }

    [HttpPost("bad-request")]
    public IActionResult BadRequestExample() => 
    Problem
    (
            type: "https://example.com/probs/sku-required",
            title: "Bad Request",
            detail: "Product SKU is required.",
            statusCode: StatusCodes.Status400BadRequest
    );

    [HttpPost("bad-request-no-body")]
    public IActionResult BadRequestNoBodyExample() => BadRequest();

    [HttpPost("not-found")]
    public IActionResult NotFoundExample() =>
    Problem
    (
            type: "https://example.com/probs/product-not-found",
            title: "Not Found",
            detail: "Product not found.",
            statusCode: StatusCodes.Status404NotFound
    );

    [HttpPost("unauthorized")]
    public IActionResult UnauthorizedExample() => Unauthorized();

    [HttpPost("conflict")]
    public IActionResult ConflictExample() => 
    Problem
    (
            type: "https://example.com/probs/product-existed",
            title: "Conflict",
            detail: "This product already exists.",
            statusCode: StatusCodes.Status409Conflict
    );
    
    [HttpPost("business-rule-error")]
    public IActionResult BusinessRuleExample() => throw new ValidationException("A discontinued product can not be put on promotion.");
        
}
