using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Api.Data;
using Api.Services;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DependentsController : ControllerBase
    {
        private readonly IDependentsService _dependentsService;

        public DependentsController(IDependentsService dependentsService)
        {
            _dependentsService = dependentsService;
        }

        [SwaggerOperation(Summary = "Get dependent by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
        {
            var response = await _dependentsService.Get(id);

            if (response.Success == false)
            {
                if (Enum.TryParse(response.Error, out HttpStatusCode statusCode))
                {
                    switch (statusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return DependentNotFound(response.Message);
                    }
                }
            }
            return response;
        }

        [SwaggerOperation(Summary = "Get all dependents")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
        {
            var response = await _dependentsService.GetAll();

            return response;
        }

        [SwaggerOperation(Summary = "Add dependent")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddDependentWithEmployeeIdDto>>> AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            var response = await _dependentsService.AddDependent(newDependent);

            if (response.Success == false)
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

            return response;
        }

        [SwaggerOperation(Summary = "Update dependent")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> UpdateDependent(int id, UpdateDependentDto updatedDependent)
        {
            var updateEmployeeResponse = await _dependentsService.UpdateDependent(id, updatedDependent);

            return updateEmployeeResponse;
        }

        [SwaggerOperation(Summary = "Delete dependent")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> DeleteDependent(int id)
        {
            var deleteEmployeeResponse = await _dependentsService.DeleteDependent(id);

            return deleteEmployeeResponse;
        }

        private NotFoundObjectResult DependentNotFound(string errorMessage)
        {
            return NotFound(new { message = errorMessage });
        }
    }
}
