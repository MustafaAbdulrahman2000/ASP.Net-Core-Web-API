using Microsoft.AspNetCore.Mvc;

namespace Module_01.Controllers;

[ApiController]
[Route("/api/documents")]
public class DocumentController: ControllerBase
{
    [HttpGet("{docNo}")]
    public IActionResult Get(int docNo)
    {
        string fileName = "somefile.pdf";
        
        var filePath = Path.Combine("E:\\SensitiveFiles", fileName);

        if (!System.IO.File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        return PhysicalFile(filePath, "application/pdf", fileName);
    }
}