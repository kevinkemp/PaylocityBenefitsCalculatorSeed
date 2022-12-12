using Api.Data;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Api.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IDependentsService _dependentsService;
        private readonly IPaycheckService _paycheckService;

        public EmployeesService(IServiceScopeFactory scopeFactory, IDependentsService dependentsService, IPaycheckService paycheckService)
        {
            _scopeFactory = scopeFactory;
            _dependentsService = dependentsService;
            _paycheckService = paycheckService;
        }

        public async Task<ApiResponse<GetEmployeeDto>> Get(int id)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var employee = await context.Employees
                .Include(e => e.Dependents)
                .AsNoTracking()
                .Where(e => e.EmployeeId == id)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var error = new ApiResponse<GetEmployeeDto>
                {
                    Data = null,
                    Success = false,
                    Message = $"Employee with Id: {id} does not exist.",
                    Error = Constants.ErrorCode.EmployeeNotFound
                };

                return error;
            }

            var dto = new GetEmployeeDto
            {
                Id = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
                Dependents = employee.Dependents?
                .Select(d => new GetDependentDto
                {
                    Id = d.DependentId,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    DateOfBirth = d.DateOfBirth,
                    Relationship = d.Relationship
                })
                .ToList() ?? new List<GetDependentDto>()

            };

            var response = new ApiResponse<GetEmployeeDto>
            {
                Data = dto,
                Success = true
            };

            return response;
        }

        public async Task<ApiResponse<List<GetEmployeeDto>>> GetAll()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var employees = await context.Employees
                .Include(e => e.Dependents)
                .AsNoTracking()
                .ToListAsync();

            if (!employees.Any())
            {
                var error = new ApiResponse<List<GetEmployeeDto>>
                {
                    Success = false,
                    Error = Constants.ErrorCode.EmployeeNotFound,
                    Message = "No Employees found"
                };

                return error;
            }

            var listDtos = employees.Select(e => new GetEmployeeDto
            {
                Id = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Salary = e.Salary,
                DateOfBirth = e.DateOfBirth,
                Dependents = e.Dependents?
                .Select(d => new GetDependentDto
                {
                    Id = d.DependentId,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    DateOfBirth = d.DateOfBirth,
                    Relationship = d.Relationship
                })
                .ToList() ?? new List<GetDependentDto>()
            }).ToList();

            var response = new ApiResponse<List<GetEmployeeDto>>
            {
                Data = listDtos,
                Success = true
            };

            return response;
        }

        public async Task<ApiResponse<GetEmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var employee = await context.Employees
                .Include(e => e.Dependents)
                .Where(e => e.EmployeeId == id)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var error = new ApiResponse<GetEmployeeDto>
                {
                    Data = null,
                    Success = false,
                    Message = $"Employee with Id: {id} does not exist.",
                    Error = Constants.ErrorCode.EmployeeNotFound
                };

                return error;
            }

            //could add checks if modified then update, if not ignore
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Salary = updatedEmployee.Salary;

            //this 80k should be an enum or constant in a shared
            if (employee.Salary > 80000)
                employee.IncursAdditionalYearlyCost = true;

            await context.SaveChangesAsync();

            //if salary was updated, recalculate paychecks, if added checks in ln 148 could add this logic there
            if (employee.Salary != updatedEmployee.Salary)
                await _paycheckService.GeneratePaychecksForEmployeeId(id);

            var dto = new GetEmployeeDto
            {
                Id = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
                Dependents = employee.Dependents?
                .Select(d => new GetDependentDto
                {
                    Id = d.DependentId,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    DateOfBirth = d.DateOfBirth,
                    Relationship = d.Relationship
                })
                .ToList() ?? new List<GetDependentDto>()
            };

            var response = new ApiResponse<GetEmployeeDto>
            {
                Data = dto,
                Success = true
            };

            return response;
        }

        public async Task<ApiResponse<GetEmployeeDto>> DeleteEmployee(int id)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var employee = await context.Employees
                .Where(e => e.EmployeeId == id)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var error = new ApiResponse<GetEmployeeDto>
                {
                    Data = null,
                    Success = false,
                    Message = $"Employee with Id: {id} does not exist.",
                    Error = Constants.ErrorCode.EmployeeNotFound
                };

                return error;
            }

            context.Employees.Remove(employee);

            //DO I HAVE TO DELETE PAYCHECKS TOO?

            await context.SaveChangesAsync();

            var dto = new GetEmployeeDto
            {
                Id = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
                Dependents = new List<GetDependentDto>()
            };

            var response = new ApiResponse<GetEmployeeDto>
            {
                Data = dto,
                Success = true
            };

            return response;
        }

        public async Task<ApiResponse<AddEmployeeDto>> AddEmployee(AddEmployeeDto newEmployee)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var addEmp = new Employee
            {
                FirstName = newEmployee.FirstName,
                LastName = newEmployee.LastName,
                DateOfBirth = newEmployee.DateOfBirth,
                Salary = newEmployee.Salary
            };

            //this 80k should be an enum or constant in a shared
            if (newEmployee.Salary > 80_000.0m)
                addEmp.IncursAdditionalYearlyCost = true;

            context.Employees.Add(addEmp);
            await context.SaveChangesAsync();

            //MAYBE THIS LOGIC IN CONTROLLER?
            if (newEmployee.Dependents != null)
            {
                foreach (var dependent in newEmployee.Dependents)
                {
                    var newDependent = new AddDependentWithEmployeeIdDto
                    {
                        EmployeeId = addEmp.EmployeeId,
                        FirstName = dependent.FirstName,
                        LastName = dependent.LastName,
                        DateOfBirth = dependent.DateOfBirth,
                        Relationship = dependent.Relationship,
                    };

                    var dependentServiceResponse = await _dependentsService.AddDependent(newDependent);

                    if (dependentServiceResponse.Success == false)
                    {
                        var error = new ApiResponse<AddEmployeeDto>
                        {
                            Data = null,
                            Success = false,
                            Message = dependentServiceResponse.Message,
                            Error = dependentServiceResponse.Error
                        };

                        return error;
                    }
                }
            }
            //when you add a dependent it regenerates paychecks, this else prevents hitting that method twice(once when adding dependent, again after adding employee)
            else
            {
                await _paycheckService.GeneratePaychecksForEmployeeId(addEmp.EmployeeId);
            }

            var response = new ApiResponse<AddEmployeeDto>
            {
                Data = newEmployee,
                Success = true,
            };

            return response;
        }

        //SHOULD THIS BE STATIC? IS THIS NEEDED IF IM DOING VALIDATIONS AT MODEL LEVEL?
        //private static AddEmployeeValidationResult ValidateNewEmployee(AddEmployeeDto newEmployee)
        //{
        //    var res = new AddEmployeeValidationResult()
        //    {
        //        Valid = true,
        //    };

        //    var age = DateTime.Today.Year - newEmployee.DateOfBirth.Year;

        //    if (string.IsNullOrEmpty(newEmployee.FirstName))
        //        res.Fields.Add(nameof(newEmployee.FirstName));

        //    if (string.IsNullOrEmpty(newEmployee.LastName))
        //        res.Fields.Add(nameof(newEmployee.LastName));

        //    if (newEmployee.Salary < 0)
        //        res.Fields.Add(nameof(newEmployee.Salary));

        //    if (age <= 17 )
        //        res.Fields.Add(nameof(newEmployee.DateOfBirth));

        //    if (res.Fields.Any())
        //        res.Valid = false;

        //    return res;
        //}
    }
}