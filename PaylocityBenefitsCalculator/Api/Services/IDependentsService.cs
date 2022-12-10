using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services
{
    public interface IDependentsService
    {
        Task<ApiResponse<GetDependentDto>>Get(int id);
        Task<ApiResponse<List<GetDependentDto>>>GetAll();
        Task<ApiResponse<AddDependentWithEmployeeIdDto>>AddDependent(AddDependentWithEmployeeIdDto newDependent);
        Task<ApiResponse<GetDependentDto>>UpdateDependent(int id, UpdateDependentDto updatedDependent);
        Task<ApiResponse<GetDependentDto>>DeleteDependent(int id);
    }
}
