using Api.Data;
using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services
{
    public class DependentsService : IDependentsService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DependentsService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<ApiResponse<GetDependentDto>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<GetDependentDto>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<AddDependentWithEmployeeIdDto>> AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var addDependent = new Dependent
            {
                EmployeeId = newDependent.EmployeeId,
                FirstName = newDependent.FirstName,
                LastName = newDependent.LastName,
                DateOfBirth = newDependent.DateOfBirth,
                Relationship = newDependent.Relationship
            };

            context.Dependents.Add(addDependent);
            await context.SaveChangesAsync();

            var response = new ApiResponse<AddDependentWithEmployeeIdDto>
            {
                Data = newDependent,
                Success = true
            };

            return response;
        }

        public async Task<ApiResponse<GetDependentDto>> UpdateDependent(int id, UpdateDependentDto updatedDependent)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<GetDependentDto>> DeleteDependent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
