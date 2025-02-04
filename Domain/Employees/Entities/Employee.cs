namespace TodoApp.Domain.Employees.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string JobTitle { get; set; }
        public required string Department { get; set; }
        public required string EmployeeNumber { get; set; }
        public byte[]? Photo { get; set; } // Store photo as bytes (if needed)
    }
}
