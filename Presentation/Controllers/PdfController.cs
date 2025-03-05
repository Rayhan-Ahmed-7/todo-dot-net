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
        // Handlebars template as a string
        var template = @"
            <html>
              <head>
                <style>
                  body { font-family: Arial, sans-serif; }
                  h1 { color: #333; }
                  p { font-size: 14px; }
                </style>
              </head>
              <body>
                <h1>{{title}}</h1>
                <p>{{description}}</p>
                <ul>
                  {{#each items}}
                    <li>{{this.name}}: ${{this.price}}</li>
                  {{/each}}
                </ul>
              </body>
            </html>";

        // Generate PDF using the PdfGenerationService
        var pdfBytes = await _pdfGenerationService.GeneratePdfAsync(template, data);

        // Return the PDF as a file response
        return File(pdfBytes, "application/pdf", "generated.pdf");
    }
}

