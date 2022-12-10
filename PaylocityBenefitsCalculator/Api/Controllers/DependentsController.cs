using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Api.Data;
using Api.Services;

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
            throw new NotImplementedException();
        }

        [SwaggerOperation(Summary = "Get all dependents")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        [SwaggerOperation(Summary = "Delete dependent")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> DeleteDependent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
