using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Api.Data;
using Api.Services;
using System.Net;
using Azure;

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
            var response = await _dependentsService.GetDependentById(id);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Get all dependents")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
        {
            var response = await _dependentsService.GetAllDependents();

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Add dependent")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddDependentWithEmployeeIdDto>>> AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            var response = await _dependentsService.AddDependent(newDependent);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Update dependent")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> UpdateDependent(int id, UpdateDependentDto updatedDependent)
        {
            var response = await _dependentsService.UpdateDependent(id, updatedDependent);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Delete dependent")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> DeleteDependent(int id)
        {
            var response = await _dependentsService.DeleteDependent(id);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        //this could go in a seperate errors controller
        private ObjectResult HttpErrorGenerator(HttpStatusCode statusCode, string errorMessage)
        {
            if (statusCode == HttpStatusCode.BadRequest)
                return BadRequest(new { message = errorMessage });

            if(statusCode ==  HttpStatusCode.NotFound)
                return NotFound( new { message = errorMessage });

            return Problem(errorMessage);
        }
    }
}
