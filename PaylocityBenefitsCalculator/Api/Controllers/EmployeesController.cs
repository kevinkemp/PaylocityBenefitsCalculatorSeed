using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Api.Data;
using Api.Services;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeeService;

        public EmployeesController(IEmployeesService employeeService)
        {
            _employeeService = employeeService;
        }

        [SwaggerOperation(Summary = "Get employee by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
        {
            var employeeResult = await _employeeService.Get(id);

            if (employeeResult.Success == false)
            {
                if (Enum.TryParse(employeeResult.Error, out HttpStatusCode statusCode))
                {
                    switch (statusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return EmployeeNotFound(employeeResult.Message);
                    }
                }
            }
            return employeeResult;
        }

        [SwaggerOperation(Summary = "Get all employees")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
        {
            //task: use a more realistic production approach
            //var employees = new List<GetEmployeeDto>
            //{
            //    new()
            //    {
            //        Id = 1,
            //        FirstName = "LeBron",
            //        LastName = "James",
            //        Salary = 75420.99m,
            //        DateOfBirth = new DateTime(1984, 12, 30)
            //    },
            //    new()
            //    {
            //        Id = 2,
            //        FirstName = "Ja",
            //        LastName = "Morant",
            //        Salary = 92365.22m,
            //        DateOfBirth = new DateTime(1999, 8, 10),
            //        Dependents = new List<GetDependentDto>
            //        {
            //            new()
            //            {
            //                Id = 1,
            //                FirstName = "Spouse",
            //                LastName = "Morant",
            //                Relationship = Relationship.Spouse,
            //                DateOfBirth = new DateTime(1998, 3, 3)
            //            },
            //            new()
            //            {
            //                Id = 2,
            //                FirstName = "Child1",
            //                LastName = "Morant",
            //                Relationship = Relationship.Child,
            //                DateOfBirth = new DateTime(2020, 6, 23)
            //            },
            //            new()
            //            {
            //                Id = 3,
            //                FirstName = "Child2",
            //                LastName = "Morant",
            //                Relationship = Relationship.Child,
            //                DateOfBirth = new DateTime(2021, 5, 18)
            //            }
            //        }
            //    },
            //    new()
            //    {
            //        Id = 3,
            //        FirstName = "Michael",
            //        LastName = "Jordan",
            //        Salary = 143211.12m,
            //        DateOfBirth = new DateTime(1963, 2, 17),
            //        Dependents = new List<GetDependentDto>
            //        {
            //            new()
            //            {
            //                Id = 4,
            //                FirstName = "DP",
            //                LastName = "Jordan",
            //                Relationship = Relationship.DomesticPartner,
            //                DateOfBirth = new DateTime(1974, 1, 2)
            //            }
            //        }
            //    }
            //};

            //var response = new ApiResponse<List<GetEmployeeDto>>
            //{
            //    Data = employees,
            //    Success = true
            //};

            //return response;

            var getAllEmployeesResponse = await _employeeService.GetAll();

            return getAllEmployeesResponse;
        }

        [SwaggerOperation(Summary = "Add employee")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddEmployeeDto>>> AddEmployee(AddEmployeeDto newEmployee)
        {
            var addEmployeeResponse = await _employeeService.AddEmployee(newEmployee);

            if (addEmployeeResponse.Success == false)
            {
                //FIX THESE ERRORS!!
                //HttpStatusCode statusCode;


                
                //if (Enum.TryParse(addEmployeeResult.Error, out statusCode))
                //{
                //    switch (statusCode)
                //    {
                //        case HttpStatusCode.NotFound:
                //            return EmployeeNotFound(addEmployeeResult.Message);
                //    }
                //}
            }

            return addEmployeeResponse;

        }

        [SwaggerOperation(Summary = "Update employee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee)
        {
            var updateEmployeeResponse = await _employeeService.UpdateEmployee(id, updatedEmployee);

            return updateEmployeeResponse;
        }

        [SwaggerOperation(Summary = "Delete employee")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> DeleteEmployee(int id)
        {
            var deleteEmployeeResponse = await _employeeService.DeleteEmployee(id);

            return deleteEmployeeResponse;
        }

        private NotFoundObjectResult EmployeeNotFound(string errorMessage)
        {
            return NotFound(new { message = errorMessage });
        }
    }
}
