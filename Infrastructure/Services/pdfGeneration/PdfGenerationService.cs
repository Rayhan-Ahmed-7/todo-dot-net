using HandlebarsDotNet;
using PuppeteerSharp;
using Application.Interfaces;
using Azure;
using PuppeteerSharp.Media;

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
            // Step 2: Launch Puppeteer and generate the PDF
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox" } // For Linux environments
            }))
            {
                var page = await browser.NewPageAsync();
                // To enable PuppeteerSharp access local files you need to manually go to a local file first
                await page.GoToAsync($"file://{newTemplateFilePath}");
                await page.SetContentAsync(htmlContent, new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                });
                var pdfBytes = await page.PdfDataAsync(new PdfOptions()
                {
                    PrintBackground = true,
                    Format = PaperFormat.A4,
                    DisplayHeaderFooter = true,
                    MarginOptions = new MarginOptions
                    {
                        Top = "0mm",
                        Bottom = "0mm",
                        Left = "0mm",
                        Right = "0mm"
                    },
                });

                return pdfBytes;
            }
        }
    }
}
