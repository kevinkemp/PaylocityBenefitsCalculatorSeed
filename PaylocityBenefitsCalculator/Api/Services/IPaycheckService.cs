using Api.Dtos.Paycheck;
using Api.Models;

namespace Api.Services
{
    public interface IPaycheckService
    {
        Task<ApiResponse<GetPaycheckDto>> GetPaycheck(int id);
        Task<ApiResponse<List<GetPaycheckDto>>> GetPaychecksByEmployeeId(int id);
        Task<ApiResponse<List<GetPaycheckDto>>> GetAll();
        Task<ApiResponse<GeneratedPaychecksDto>> GeneratePaychecksForEmployeeId(int id);
    }
}
