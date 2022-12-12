using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaycheckController : ControllerBase
    {
        private readonly IPaycheckService _paycheckService;

        public PaycheckController(IPaycheckService paycheckService)
        {
            _paycheckService = paycheckService;
        }

        [SwaggerOperation(Summary = "Get paycheck by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int id)
        {
            var response = await _paycheckService.GetPaycheck(id);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Get all paychecks for Employee Id")]
        [HttpGet("/EmployeeId/{id}")]
        public async Task<ActionResult<ApiResponse<List<GetPaycheckDto>>>> GetAllByEmployeeId(int id)
        {
            var response = await _paycheckService.GetPaychecksByEmployeeId(id);

            if (response.Success == false)
            {
                var statusCode = Constants.ErrorDictionary.GetHttpError()[response.Error];

                return HttpErrorGenerator(statusCode, response.Message);
            }

            return response;
        }

        [SwaggerOperation(Summary = "Get all paychecks")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetPaycheckDto>>>> GetAll()
        {
            var response = await _paycheckService.GetAll();

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
