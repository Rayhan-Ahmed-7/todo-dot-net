
using TodoApp.Domain.Employees.Entities;

namespace TodoApp.Application.Employees.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> CreateAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
        Task<Employee?> UpdateAsync(Guid id, Employee updatedEmployee);
        Task<bool> DeleteAsync(Guid id);
    }
}