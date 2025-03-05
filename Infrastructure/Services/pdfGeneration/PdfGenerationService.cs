using HandlebarsDotNet;
using PuppeteerSharp;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class PdfGenerationService : IPdfGenerationService
    {
        public async Task<byte[]> GeneratePdfAsync(string templateFilePath, object data)
        {
            // Step 1: Read the HTML template from a file
            var template = await File.ReadAllTextAsync(templateFilePath);

            // Step 1: Render HTML using Handlebars
            var compiledTemplate = Handlebars.Compile(template);
            var htmlContent = compiledTemplate(data);

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
