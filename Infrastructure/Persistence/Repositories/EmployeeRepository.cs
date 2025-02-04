using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Employees.Entities;
using TodoApp.Infrastructure.Interfaces;
using TodoApp.Infrastructure.Persistence.DbContexts;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _context.Employees.FindAsync(id);
    }

    public async Task<Employee?> UpdateAsync(Employee employee)
    {
        var existingEmployee = await _context.Employees.FindAsync(employee.Id);
        if (existingEmployee == null) return null;

        _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
        await _context.SaveChangesAsync();
        return existingEmployee;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }
}
