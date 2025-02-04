using System;
using System.Threading.Tasks;

public interface IGenerateEmployeeIdCardService
{
    Task<byte[]> GeneratePdfAsync(Guid employeeId);
}
