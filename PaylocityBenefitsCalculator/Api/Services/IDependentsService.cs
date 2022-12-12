using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services
{
    public interface IDependentsService
    {
        Task<ApiResponse<GetDependentDto>>GetDependentById(int id);
        Task<ApiResponse<List<GetDependentDto>>>GetAllDependents();
        Task<ApiResponse<AddDependentWithEmployeeIdDto>>AddDependent(AddDependentWithEmployeeIdDto newDependent);
        Task<ApiResponse<GetDependentDto>>UpdateDependent(int id, UpdateDependentDto updatedDependent);
        Task<ApiResponse<GetDependentDto>>DeleteDependent(int id);
    }
}
