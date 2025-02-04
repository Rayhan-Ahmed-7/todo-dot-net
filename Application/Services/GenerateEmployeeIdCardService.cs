using Application.Interfaces;
using Domain.Entities;
using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class GenerateEmployeeIdCardService : IGenerateEmployeeIdCardService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IConverter _pdfConverter;

    public GenerateEmployeeIdCardService(IEmployeeRepository employeeRepository, IConverter pdfConverter)
    {
        _employeeRepository = employeeRepository;
        _pdfConverter = pdfConverter;
    }

    public async Task<byte[]> GeneratePdfAsync(Guid employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null) return null;

        var html = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; text-align: center; }}
                    .id-card {{ width: 300px; padding: 10px; border: 2px solid #000; }}
                    .photo {{ width: 100px; height: 100px; border-radius: 50%; }}
                </style>
            </head>
            <body>
                <div class='id-card'>
                    <h2>Employee ID Card</h2>
                    <img src='data:image/png;base64,{Convert.ToBase64String(employee.Photo ?? new byte[0])}' class='photo' />
                    <p><strong>{employee.FullName}</strong></p>
                    <p>{employee.JobTitle} - {employee.Department}</p>
                    <p>ID: {employee.EmployeeNumber}</p>
                </div>
            </body>
            </html>";

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = { ColorMode = ColorMode.Color, PaperSize = PaperKind.A6 },
            Objects = { new ObjectSettings { HtmlContent = html } }
        };

        return _pdfConverter.Convert(doc);
    }
}
