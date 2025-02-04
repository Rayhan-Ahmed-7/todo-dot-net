namespace TodoApp.Application.Employees.Interfaces
{
    public interface IGenerateEmployeeIdCardService
    {
        Task<byte[]> GeneratePdfAsync(Guid employeeId);
    }
}