using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEmployeeService
{
    Task<Employee> CreateAsync(Employee employee);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(Guid id);
    Task<Employee?> UpdateAsync(Guid id, Employee updatedEmployee);
    Task<bool> DeleteAsync(Guid id);
}
