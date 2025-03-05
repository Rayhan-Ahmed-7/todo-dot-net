using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;


[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IPdfGenerationService _pdfGenerationService;

    public PdfController(IPdfGenerationService pdfGenerationService)
    {
        _pdfGenerationService = pdfGenerationService;
    }

    [HttpPost("generate-pdf")]
    public async Task<IActionResult> GeneratePdf([FromBody] PdfData data)
    {
        try
        {
            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "html-to-pdf-templates", "id-cards", "bangjin");

            // Construct file paths dynamically
            var model = new
            {
                cssUrl = $"file://{Path.Combine(wwwRootPath, "assets/css/bangla-id-card-style.css")}",
                logoUrl = $"file://{Path.Combine(wwwRootPath, "assets/logo/logo.png")}",
                photoUrl = $"file://{Path.Combine(wwwRootPath, "assets/employee/employee_profile.jpeg")}"
            };

            Console.WriteLine(model.cssUrl);

            // Handlebars template as a string
            var templateFilePath = "id-cards/bangjin/templates/id-card-bangla.html";

            // Generate PDF using the PdfGenerationService
            var pdfBytes = await _pdfGenerationService.GeneratePdfAsync(templateFilePath, model);

            // Return the PDF as a file response
            return File(pdfBytes, "application/pdf", "generated.pdf");
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

