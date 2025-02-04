using TodoApp.Domain.Employees.Entities;

namespace TodoApp.Infrastructure.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
        Task<Employee?> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(Guid id);
    }
}