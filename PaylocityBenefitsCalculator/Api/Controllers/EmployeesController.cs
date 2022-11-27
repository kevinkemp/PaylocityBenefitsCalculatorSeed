﻿using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private EmployeesService _employeesService;
        
        public EmployeesController(EmployeesService employeesService) { 
            _employeesService = employeesService;
        }


        [SwaggerOperation(Summary = "Get employee by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
        {
            GetEmployeeDto employee = await _employeesService.GetEmployeeById(id);

            var result = new ApiResponse<GetEmployeeDto>
            {
                Data = employee,
                Success = true
            };

            return result;
        }

        [SwaggerOperation(Summary = "Get all employees")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
        {
           IEnumerable<GetEmployeeDto> employees = await _employeesService.GetAllEmployees();
            
            var result = new ApiResponse<List<GetEmployeeDto>>
            {
                Data = employees.ToList(),
                Success = true
            };
            
            return result;
        }

        [SwaggerOperation(Summary = "Add employee")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<List<AddEmployeeDto>>>> AddEmployee(AddEmployeeDto newEmployee)
        {
            IEnumerable<AddEmployeeDto> employees = await _employeesService.AddEmployee(newEmployee);

            var result = new ApiResponse<List<AddEmployeeDto>>
            {
                Data = employees.ToList(),
                Success = true
            };

            return result;
        }

        [SwaggerOperation(Summary = "Update employee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee)
        {
            GetEmployeeDto employee = await _employeesService.UpdateEmployee(id, updatedEmployee);
            
            var result = new ApiResponse<GetEmployeeDto>
            {
                Data = employee,
                Success = true
            };

            return result;
        }

        [SwaggerOperation(Summary = "Delete employee")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> DeleteEmployee(int id)
        {
            IEnumerable<GetEmployeeDto> employees = await _employeesService.DeleteEmployee(id);

            var result = new ApiResponse<List<GetEmployeeDto>>
            {
                Data = employees.ToList(),
                Success = true
            };

            return result;
        }
    }
}
