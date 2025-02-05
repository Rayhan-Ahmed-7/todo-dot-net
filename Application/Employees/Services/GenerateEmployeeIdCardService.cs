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
        _assetsPath = "/home/rayhan/projects/todo-dot-net/assets/"; // Adjust to your actual path
        _templatePath = Path.Combine(_assetsPath, "templates/id_card_bangla.html");
        _cssPath = Path.Combine(_assetsPath, "css/bangla_id_card_style.css");
        _fontPath = Path.Combine(_assetsPath, "fonts/SutonnyMJ/SutonnyOMJ.ttf");
    }

    public async Task<byte[]> GeneratePdfAsync(Guid employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null) return null;

        // Read the HTML template
        var htmlTemplate = await File.ReadAllTextAsync(_templatePath);

        // Convert employee photo to Base64 or use default
        string employeePhotoPath = Path.Combine(_assetsPath, "images/employee/employee_profile.jpeg");
        string photoBase64 = employee.Photo != null && employee.Photo.Length > 0
            ? Convert.ToBase64String(employee.Photo)
            : Convert.ToBase64String(await File.ReadAllBytesAsync(employeePhotoPath));

        // Replace placeholders with actual data
        var htmlContent = htmlTemplate
            .Replace("{{EMPLOYEE_NAME}}", employee.FullName)
            .Replace("{{JOB_TITLE}}", employee.JobTitle)
            .Replace("{{DEPARTMENT}}", employee.Department)
            .Replace("{{EMPLOYEE_NUMBER}}", employee.EmployeeNumber)
            .Replace("{{PHOTO}}", $"data:image/png;base64,{photoBase64}")
            .Replace("{{CSS_PATH}}", _cssPath) // Include correct CSS file path
            .Replace("{{PHONE_ICON}}", Path.Combine(_assetsPath, "images/icons/phone.svg"))
            .Replace("{{LOGO}}", Path.Combine(_assetsPath, "images/logo/logo.png"))
            .Replace("{{FONT_PATH}}", _fontPath);

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                PaperSize = PaperKind.A6
            }
        };

        // Create ObjectSettings and add it to the document
        var objectSettings = new ObjectSettings
        {
            HtmlContent = htmlContent,
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
