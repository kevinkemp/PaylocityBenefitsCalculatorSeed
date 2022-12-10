using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IEmployeesService
    {
        Task<ApiResponse<GetEmployeeDto>> Get(int id);

        Task<ApiResponse<List<GetEmployeeDto>>>GetAll();

        Task<ApiResponse<AddEmployeeDto>>AddEmployee(AddEmployeeDto newEmployee);

        Task<ApiResponse<GetEmployeeDto>>DeleteEmployee(int id);

        Task<ApiResponse<GetEmployeeDto>>UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee);
    }
}
