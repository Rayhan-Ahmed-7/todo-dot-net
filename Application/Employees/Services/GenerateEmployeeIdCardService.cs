using System.Text;
using DinkToPdf;
using DinkToPdf.Contracts;
using TodoApp.Application.Employees.Interfaces;
using TodoApp.Infrastructure.Interfaces;

public class GenerateEmployeeIdCardService : IGenerateEmployeeIdCardService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IConverter _pdfConverter;
    private readonly string _assetsPath;
    private readonly string _templatePath;
    private readonly string _cssPath;
    private readonly string _fontPath;

    public GenerateEmployeeIdCardService(IEmployeeRepository employeeRepository, IConverter pdfConverter)
    {
        _employeeRepository = employeeRepository;
        _pdfConverter = pdfConverter;
        _assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");
        // _assetsPath = "/home/rayhan/projects/todo-dot-net/assets/"; // Adjust to your actual path
        _templatePath = Path.Combine(_assetsPath, "templates/id_card_bangla.html");
        _cssPath = Path.Combine(_assetsPath, "css/bangla_id_card_style.css");
        _fontPath = Path.Combine(_assetsPath, "fonts/SutonnyMJ/SutonnyOMJ.ttf");
    }

    public async Task<byte[]> GeneratePdfAsync(Guid employeeId)
    {
        var employees = await _employeeRepository.GetAllAsync();
        if (employees == null || !employees.Any()) return null;

        // Read the HTML template
        var htmlTemplate = await File.ReadAllTextAsync(_templatePath);

        var employeeCardsHtml = new StringBuilder();

        foreach (var employee in employees)
        {
            string employeePhotoPath = Path.Combine(_assetsPath, "images/employee/employee_profile.jpeg");
            string phoneIcon = Path.Combine(_assetsPath, "images/employee/employee_profile.jpeg");
            string photoBase64 = employee.Photo != null && employee.Photo.Length > 0
                ? Convert.ToBase64String(employee.Photo)
                : Convert.ToBase64String(await File.ReadAllBytesAsync(employeePhotoPath));

            // Replace placeholders with actual data
            var employeeHtml = htmlTemplate
                .Replace("{{EMPLOYEE_NAME}}", employee.FullName)
                .Replace("{{JOB_TITLE}}", employee.JobTitle)
                .Replace("{{DEPARTMENT}}", employee.Department)
                .Replace("{{EMPLOYEE_NUMBER}}", employee.EmployeeNumber)
                .Replace("{{PHOTO}}", $"data:image/png;base64,{photoBase64}")
                .Replace("{{CSS_PATH}}", _cssPath)
                .Replace("{{PHONE_ICON}}", Path.Combine(_assetsPath, "images/icons/phone.svg"))
                .Replace("{{LOGO}}", Path.Combine(_assetsPath, "images/logo/logo.png"))
                .Replace("{{FONT_PATH}}", _fontPath);

            employeeCardsHtml.Append(employeeHtml); // Append the generated HTML for each employee
        }

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                PaperSize = PaperKind.A4, // Each card will be A6 size
                Orientation = Orientation.Portrait,
                Margins = new MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 } // Add margins for spacing
            }
        };

        // Create ObjectSettings and add it to the document
        var objectSettings = new ObjectSettings
        {
            HtmlContent = employeeCardsHtml.ToString(), // All employee cards in one HTML document
            WebSettings = new WebSettings
            {
                DefaultEncoding = "utf-8",
                UserStyleSheet = _cssPath,
            },
            LoadSettings = new LoadSettings
            {
                BlockLocalFileAccess = false,
            }
        };

        doc.Objects.Add(objectSettings); // Add the ObjectSettings to the document

        return _pdfConverter.Convert(doc);
    }


}
