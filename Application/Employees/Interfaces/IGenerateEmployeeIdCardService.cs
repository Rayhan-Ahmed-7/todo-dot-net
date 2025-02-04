namespace TodoApp.Application.Employee.Interfaces
{
    public interface IGenerateEmployeeIdCardService
    {
        Task<byte[]> GeneratePdfAsync(Guid employeeId);
    }
}