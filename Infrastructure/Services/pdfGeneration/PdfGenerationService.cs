using HandlebarsDotNet;
using PuppeteerSharp;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class PdfGenerationService : IPdfGenerationService
    {
        private readonly string _templateDirectory;

        public PdfGenerationService()
        {
            // Set the base directory where templates are stored
            _templateDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "html-to-pdf-templates");
        }
        public async Task<byte[]> GeneratePdfAsync(string templateFilePath, object data)
        {
            // Construct the full path dynamically based on the provided template name
            var newTemplateFilePath = Path.Combine(_templateDirectory, templateFilePath);

            if (!File.Exists(newTemplateFilePath))
            {
                throw new FileNotFoundException($"Template file '{templateFilePath}' not found.");
            }
            // Step 1: Read the HTML template from a file
            var template = await File.ReadAllTextAsync(newTemplateFilePath);

            // Step 1: Render HTML using Handlebars
            var compiledTemplate = Handlebars.Compile(template);
            var htmlContent = compiledTemplate(data);
            Console.WriteLine(htmlContent);
            // Step 2: Launch Puppeteer and generate the PDF
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                var page = await browser.NewPageAsync();
                await page.SetContentAsync(htmlContent);
                var pdfBytes = await page.PdfDataAsync();

                return pdfBytes;
            }
        }
    }
}
