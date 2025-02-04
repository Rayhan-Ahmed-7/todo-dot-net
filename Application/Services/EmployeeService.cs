using TodoApp.Application.Employees.Interfaces;

namespace TodoApp.Application.Todo.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            return await _employeeRepository.AddAsync(employee);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<Employee?> UpdateAsync(Guid id, Employee updatedEmployee)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            employee.FullName = updatedEmployee.FullName;
            employee.JobTitle = updatedEmployee.JobTitle;
            employee.Department = updatedEmployee.Department;
            employee.EmployeeNumber = updatedEmployee.EmployeeNumber;
            employee.Photo = updatedEmployee.Photo;

            return await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}
