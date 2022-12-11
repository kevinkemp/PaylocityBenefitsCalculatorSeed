using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

            return response;
        }

        [SwaggerOperation(Summary = "Get all paychecks for Employee Id")]
        [HttpGet("/All/{id}")]
        public async Task<ActionResult<ApiResponse<List<GetPaycheckDto>>>> GetAllByEmployeeId(int id)
        {
            var response = await _paycheckService.GetPaychecksByEmployeeId(id);

            return response;
        }

        [SwaggerOperation(Summary = "Get all paychecks")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetPaycheckDto>>>> GetAll()
        {
            var response = await _paycheckService.GetAll();

            return response;
        }
    }
}
