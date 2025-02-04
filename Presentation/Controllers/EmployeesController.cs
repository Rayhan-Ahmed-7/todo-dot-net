using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Employee.Interfaces;
using TodoApp.Domain.Employees.Entities;

[Route("api/employees")]
[ApiController]
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
    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
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
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    // ✅ Update Employee
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] Employee updatedEmployee)
    {
        var employee = await _employeeService.UpdateAsync(id, updatedEmployee);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    // ✅ Delete Employee
    [HttpDelete("{id}")]
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
