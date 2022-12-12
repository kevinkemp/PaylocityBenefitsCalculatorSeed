using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Api.Data;
using Api.Services;
using System.Net;
using Azure;

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
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Get all employees")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
        {
            var response = await _employeeService.GetAll();

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Add employee")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddEmployeeDto>>> AddEmployee(AddEmployeeDto newEmployee)
        {
            var response = await _employeeService.AddEmployee(newEmployee);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;

        }

        [SwaggerOperation(Summary = "Update employee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee)
        {
            var response = await _employeeService.UpdateEmployee(id, updatedEmployee);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Delete employee")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> DeleteEmployee(int id)
        {
            var response = await _employeeService.DeleteEmployee(id);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        private ObjectResult HttpErrorGenerator(HttpStatusCode statusCode, string errorMessage)
        {
            if (statusCode == HttpStatusCode.BadRequest)
                return BadRequest(new { message = errorMessage });

            if (statusCode == HttpStatusCode.NotFound)
                return NotFound(new { message = errorMessage });

            return Problem(errorMessage);
        }
    }
}
