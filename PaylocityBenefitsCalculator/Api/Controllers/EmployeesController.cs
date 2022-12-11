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
            var response = await _employeeService.Get(id);

            if (response.Success == false)
            {
                if (Enum.TryParse(response.Error, out HttpStatusCode statusCode))
                {
                    switch (statusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return EmployeeNotFound(response.Message);
                    }
                }
            }
            return response;
        }

        [SwaggerOperation(Summary = "Get all employees")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
        {
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
