public class Employee
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    public string EmployeeNumber { get; set; }
    public byte[]? Photo { get; set; } // Store photo as bytes (if needed)
}
