using Api.Models;
using Api.Dtos.Paycheck;
using Api.Data;
using Microsoft.Extensions.Hosting;
using Api.Dtos.Employee;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class PaycheckService : IPaycheckService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICalculatorService _calculatorService;

        public PaycheckService(IServiceScopeFactory scopeFactory, ICalculatorService calculatorService)
        {
            _scopeFactory = scopeFactory;
            _calculatorService = calculatorService;
        }

        public async Task<ApiResponse<GetPaycheckDto>> GetPaycheck(int id)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var paycheck = await context.Paychecks
                .AsNoTracking()
                .Where(p => p.PaycheckId == id)
                .Select(p => new GetPaycheckDto
                {
                    PaycheckId = p.PaycheckId,
                    GrossPayPerPaycheck = p.GrossPayPerPaycheck,
                    TotalDeductionsPerPaycheck = p.TotalDeductionsPerPaycheck,
                    MonthlyBaseDeductionPerPaycheck = p.MonthlyBaseDeductionPerPaycheck,
                    DeductionsPerDependentPerPaycheck = p.DeductionsPerDependentPerPaycheck,
                    AdditionalYearlyDeductionPerPaycheck = p.AdditionalYearlyDeductionPerPaycheck,
                    AdditionalDeductionPerDependentPerPaycheck = p.AdditionalDeductionPerDependentPerPaycheck,
                    NetPayPerPaycheck = p.NetPayPerPaycheck,
                    StartDate = p.StartDate,
                    PayDate = p.PayDate,
                    EmployeeId = p.EmployeeId
                })
                .SingleOrDefaultAsync();

            var response = new ApiResponse<GetPaycheckDto>();

            if(paycheck == null)
            {
                response.Data = null; //DO I NEED THIS????
                response.Success = false;
                response.Message = $"Paycheck with Id {id} not found";
                response.Error = Constants.ErrorCode.PaycheckNotFound;

                return response;
            }

            response.Success = true;
            response.Data = paycheck;

            return response;
        }

        public async Task<ApiResponse<List<GetPaycheckDto>>> GetPaychecksByEmployeeId(int id)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var paychecks = await context.Paychecks
                .AsNoTracking()
                .Where(p => p.EmployeeId == id)
                .Select(p => new GetPaycheckDto
                {
                    PaycheckId = p.PaycheckId,
                    GrossPayPerPaycheck = p.GrossPayPerPaycheck,
                    TotalDeductionsPerPaycheck = p.TotalDeductionsPerPaycheck,
                    MonthlyBaseDeductionPerPaycheck = p.MonthlyBaseDeductionPerPaycheck,
                    DeductionsPerDependentPerPaycheck = p.DeductionsPerDependentPerPaycheck,
                    AdditionalYearlyDeductionPerPaycheck = p.AdditionalYearlyDeductionPerPaycheck,
                    AdditionalDeductionPerDependentPerPaycheck = p.AdditionalDeductionPerDependentPerPaycheck,
                    NetPayPerPaycheck = p.NetPayPerPaycheck,
                    StartDate = p.StartDate,
                    PayDate = p.PayDate,
                })
                .ToListAsync();

            var response = new ApiResponse<List<GetPaycheckDto>>();

            if (!paychecks.Any())
            {
                response.Data = null; //DO I NEED THIS????
                response.Success = false;
                response.Message = $"No Paychecks found.";
                response.Error = Constants.ErrorCode.PaycheckNotFound;

                return response;
            }

            response.Success = true;
            response.Data = paychecks;

            return response;
        }

        public async Task<ApiResponse<List<GetPaycheckDto>>> GetAll()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var paychecks = await context.Paychecks
                .AsNoTracking()
                .Select(p => new GetPaycheckDto
                {
                    PaycheckId = p.PaycheckId,
                    GrossPayPerPaycheck = p.GrossPayPerPaycheck,
                    TotalDeductionsPerPaycheck = p.TotalDeductionsPerPaycheck,
                    MonthlyBaseDeductionPerPaycheck = p.MonthlyBaseDeductionPerPaycheck,
                    DeductionsPerDependentPerPaycheck = p.DeductionsPerDependentPerPaycheck,
                    AdditionalYearlyDeductionPerPaycheck = p.AdditionalYearlyDeductionPerPaycheck,
                    AdditionalDeductionPerDependentPerPaycheck = p.AdditionalDeductionPerDependentPerPaycheck,
                    NetPayPerPaycheck = p.NetPayPerPaycheck,
                    StartDate = p.StartDate,
                    PayDate = p.PayDate,
                    EmployeeId = p.EmployeeId
                })
                .ToListAsync();

            var response = new ApiResponse<List<GetPaycheckDto>>();

            if (!paychecks.Any())
            {
                response.Data = null; //DO I NEED THIS????
                response.Success = false;
                response.Message = $"No Paychecks found.";
                response.Error = Constants.ErrorCode.PaycheckNotFound;

                return response;
            }

            response.Success = true;
            response.Data = paychecks;

            return response;
        }

        public async Task<ApiResponse<GeneratedPaychecksDto>> GeneratePaychecksForEmployeeId(int id)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<PayrollDbContext>();

            var employee = await context.Employees
                .Include(e => e.Dependents)
                .AsNoTracking()
                .Where(e => e.EmployeeId == id)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var error = new ApiResponse<GeneratedPaychecksDto>
                {
                    Data = null,
                    Success = false,
                    Message = $"Employee with Id: {id} does not exist.",
                    Error = Constants.ErrorCode.EmployeeNotFound
                };

                return error;
            }

            //if paychecks exist, remove them
            var paychecks = await context.Paychecks
                .Where(p => p.EmployeeId == id)
                .ToListAsync();

            if (paychecks.Any())
            {
                context.Paychecks.RemoveRange(paychecks);

                await context.SaveChangesAsync();
            }


            var basePaycheckDto = new GenerateBasePaycheckDto
            {
                EmployeeId = employee.EmployeeId,
                BaseSalary = employee.Salary,
            };

            if (employee.Salary > Constants.CalculatorValues.MinimumBaseSalaryToIncurAdditionalCost)
                basePaycheckDto.IncursAdditionalYearlyCost = true;

            if (employee.Dependents?.Any() == true)
            {
                basePaycheckDto.NumberOfDependents = employee.Dependents.Count;

                var numberOfDependentsOverAge = 0;

                foreach (var dependent in employee.Dependents)
                {
                    if ((DateTime.Today.Year - dependent.DateOfBirth.Year) > Constants.CalculatorValues.MinimumAgeToIncurAdditionalDependentCost)
                        numberOfDependentsOverAge++;
                }

                basePaycheckDto.NumberOfDependentsOverAge = numberOfDependentsOverAge;
            }

            //fill base paycheck with calculations paychecks
            CalculateBasePaycheck(basePaycheckDto);

            //generate paychecks for a year, nice-to-have: specify a year
            var paychecksForYear = _calculatorService.GeneratePaychecksForYear(basePaycheckDto);

            //save new paychecks
            await context.Paychecks.AddRangeAsync(paychecksForYear);
            await context.SaveChangesAsync();

            var paycheckIds = paychecksForYear
                .Select(p => p.PaycheckId)
                .ToList();

            var dto = new GeneratedPaychecksDto
            {
                EmployeeId = employee.EmployeeId,
                PaycheckIds = paycheckIds
            };

            var response = new ApiResponse<GeneratedPaychecksDto>
            {
                Data = dto,
                Success = true
            };

            return response;
        }

        private void CalculateBasePaycheck(GenerateBasePaycheckDto basePaycheckDto)
        {
            basePaycheckDto.BaseCostPerYear = _calculatorService.GetBaseCostPerYear();

            //only do these calculations if i have to
            if (basePaycheckDto.IncursAdditionalYearlyCost)
                basePaycheckDto.AdditionalYearlyCostByPercentage = _calculatorService.GetAdditionalYearlyCostByPercentage(basePaycheckDto.BaseSalary);

            if (basePaycheckDto.NumberOfDependents > 0) 
                basePaycheckDto.DependentsBaseCostPerYear = _calculatorService.GetDependentsBaseCostPerYear(basePaycheckDto.NumberOfDependents);

            if (basePaycheckDto.NumberOfDependentsOverAge > 0)
                basePaycheckDto.AdditionalDependentsCostByAgePerYear = _calculatorService.GetAdditionalDependentsCostByAgePerYear(basePaycheckDto.NumberOfDependentsOverAge);
        }
    }
}
