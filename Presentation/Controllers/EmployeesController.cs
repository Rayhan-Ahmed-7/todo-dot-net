using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Employees.Interfaces;
using TodoApp.Domain.Employees.Entities;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IGenerateEmployeeIdCardService _idCardService;

    public EmployeesController(IEmployeeService employeeService, IGenerateEmployeeIdCardService idCardService)
    {
        _employeeService = employeeService;
        _idCardService = idCardService;
    }

    // ✅ Create Employee
    [HttpPost("create")]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employeeDto)
    {
        Console.WriteLine("Received null employee data.", employeeDto);

        if (employeeDto == null)
        {
            return BadRequest("Employee data is required.");
        }
        var employee = new Employee
        {
            Id = employeeDto.Id,
            FullName = employeeDto.FullName,
            JobTitle = employeeDto.JobTitle,
            Department = employeeDto.Department,
            EmployeeNumber = employeeDto.EmployeeNumber,
            // Photo = string.IsNullOrEmpty(employeeDto.Photo) ? null : Convert.FromBase64String(employeeDto.Photo)
        };
        var createdEmployee = await _employeeService.CreateAsync(employee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
    }

    // ✅ Get All Employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    // ✅ Get Employee by ID
    [HttpGet("details/{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    // ✅ Update Employee
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] Employee updatedEmployee)
    {
        var employee = await _employeeService.UpdateAsync(id, updatedEmployee);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    // ✅ Delete Employee
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        var result = await _employeeService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    // ✅ Generate Employee ID Card as PDF
    [HttpPost("{id}/generate-id-card")]
    public async Task<IActionResult> GenerateIdCard(Guid id)
    {
        var pdfData = await _idCardService.GeneratePdfAsync(id);
        if (pdfData == null) return NotFound("Employee not found or unable to generate ID card.");

        return File(pdfData, "application/pdf", "EmployeeIdCard.pdf");
    }
}
