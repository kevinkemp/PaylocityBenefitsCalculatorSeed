using Api.Data;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class DependentsService : IDependentsService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IPaycheckService _paycheckService;

        public DependentsService(IServiceScopeFactory scopeFactory, IPaycheckService paycheckService)
        {
            _scopeFactory = scopeFactory;
            _paycheckService = paycheckService;
        }
        public async Task<ApiResponse<GetDependentDto>> Get(int id)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var dependent = await context.Dependents
                .AsNoTracking()
                .Where(e => e.DependentId == id)
                .FirstOrDefaultAsync();

            if (dependent == null)
            {
                var error = new ApiResponse<GetDependentDto>
                {
                    Data = null,
                    Success = false,
                    Message = $"Dependent with Id: {id} does not exist.",
                    Error = "404"
                };

                return error;
            }

            var dto = new GetDependentDto
            {
                Id = dependent.DependentId,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship
            };

            var response = new ApiResponse<GetDependentDto>
            {
                Data = dto,
                Success = true
            };

            return response;
        }

        public async Task<ApiResponse<List<GetDependentDto>>> GetAll()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var dependents = await context.Dependents
                .AsNoTracking()
                .ToListAsync();

            if (!dependents.Any())
            {
                //RETURN NO EMPLOYEES FOUDN HERE 
            }

            var listDtos = dependents.Select(d => new GetDependentDto
            {
                Id = d.DependentId,
                FirstName = d.FirstName,
                LastName = d.LastName,
                DateOfBirth = d.DateOfBirth,
                Relationship = d.Relationship
            }).ToList();

            var response = new ApiResponse<List<GetDependentDto>>
            {
                Data = listDtos,
                Success = true
            };

            return response;
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

            //recalculate paychecks
            await _paycheckService.GeneratePaychecksForEmployeeId(addDependent.EmployeeId);

            var response = new ApiResponse<AddDependentWithEmployeeIdDto>
            {
                Data = newDependent,
                Success = true
            };

            return response;
        }

        public async Task<ApiResponse<GetDependentDto>> UpdateDependent(int id, UpdateDependentDto updatedDependent)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var dependent = await context.Dependents
                .Where(d => d.DependentId == id)
                .FirstOrDefaultAsync();

            if (dependent == null)
            {
                var error = new ApiResponse<GetDependentDto>
                {
                    Data = null,
                    Success = false,
                    Message = $"Dependent with Id: {id} does not exist.",
                    Error = "404"
                };

                return error;
            }

            dependent.FirstName = updatedDependent.FirstName;
            dependent.LastName = updatedDependent.LastName;
            dependent.DateOfBirth = updatedDependent.DateOfBirth;
            dependent.Relationship = updatedDependent.Relationship;

            await context.SaveChangesAsync();

            //recalculate paychecks
            await _paycheckService.GeneratePaychecksForEmployeeId(dependent.EmployeeId);

            var dto = new GetDependentDto
            {
                Id = dependent.EmployeeId,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship
            };

            var response = new ApiResponse<GetDependentDto>
            {
                Data = dto,
                Success = true
            };

            return response;
        }

        public async Task<ApiResponse<GetDependentDto>> DeleteDependent(int id)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var dependent = await context.Dependents
                .Where(d => d.DependentId == id)
                .FirstOrDefaultAsync();

            if (dependent == null)
            {
                var error = new ApiResponse<GetDependentDto>
                {
                    Data = null,
                    Success = false,
                    Message = $"Dependent with Id: {id} does not exist.",
                    Error = "404"
                };

                return error;
            }

            context.Dependents.Remove(dependent);
            await context.SaveChangesAsync();

            //recalculate paychecks
            await _paycheckService.GeneratePaychecksForEmployeeId(dependent.EmployeeId);

            var dto = new GetDependentDto
            {
                Id = dependent.DependentId,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship
            };

            var response = new ApiResponse<GetDependentDto>
            {
                Data = dto,
                Success = true
            };

            return response;
        }
    }
}
