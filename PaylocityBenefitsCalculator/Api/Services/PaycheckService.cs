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

        public PaycheckService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
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
                    GrossPay = p.GrossPay,
                    TotalDeductions = p.TotalDeductions,
                    MonthlyBaseDeductionPerPaycheck = p.MonthlyBaseDeductionPerPaycheck,
                    DeductionsPerDependentPerPaycheck = p.DeductionsPerDependentPerPaycheck,
                    AdditionalAnnualDeductionPerPaycheck = p.AdditionalAnnualDeductionPerPaycheck,
                    AdditionalDeductionPerDependentPerPaycheck = p.AdditionalDeductionPerDependentPerPaycheck,
                    NetPayPerPaycheck = p.NetPayPerPaycheck,
                    StartDate = p.StartDate,
                    PayDate = p.PayDate,
                    EmployeeId = p.PaycheckId
                })
                .SingleOrDefaultAsync();

            var response = new ApiResponse<GetPaycheckDto>
            {
                Data = paycheck,
                Success = true
            };

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
                    GrossPay = p.GrossPay,
                    TotalDeductions = p.TotalDeductions,
                    MonthlyBaseDeductionPerPaycheck = p.MonthlyBaseDeductionPerPaycheck,
                    DeductionsPerDependentPerPaycheck = p.DeductionsPerDependentPerPaycheck,
                    AdditionalAnnualDeductionPerPaycheck = p.AdditionalAnnualDeductionPerPaycheck,
                    AdditionalDeductionPerDependentPerPaycheck = p.AdditionalDeductionPerDependentPerPaycheck,
                    NetPayPerPaycheck = p.NetPayPerPaycheck,
                    StartDate = p.StartDate,
                    PayDate = p.PayDate,
                    EmployeeId = p.EmployeeId
                })
                .ToListAsync();

            var response = new ApiResponse<List<GetPaycheckDto>>
            {
                Data = paychecks,
                Success = true
            };

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
                    GrossPay = p.GrossPay,
                    TotalDeductions = p.TotalDeductions,
                    MonthlyBaseDeductionPerPaycheck = p.MonthlyBaseDeductionPerPaycheck,
                    DeductionsPerDependentPerPaycheck = p.DeductionsPerDependentPerPaycheck,
                    AdditionalAnnualDeductionPerPaycheck = p.AdditionalAnnualDeductionPerPaycheck,
                    AdditionalDeductionPerDependentPerPaycheck = p.AdditionalDeductionPerDependentPerPaycheck,
                    NetPayPerPaycheck = p.NetPayPerPaycheck,
                    StartDate = p.StartDate,
                    PayDate = p.PayDate,
                    EmployeeId = p.EmployeeId
                })
                .ToListAsync();

            var response = new ApiResponse<List<GetPaycheckDto>>
            {
                Data = paychecks,
                Success = true
            };

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
                    Error = "404"
                };

                return error;
            }

            //if paychecks exist, remove them
            var paychecks = await context.Paychecks
                .Where(p => p.EmployeeId == id)
                .ToListAsync();

            if(paychecks.Any())
            {
                context.Paychecks.RemoveRange(paychecks);
                
                await context.SaveChangesAsync();
            }

            //generate new paychecks
            decimal baseCost = DeductBaseCost();
            decimal additionalAnnualCost = 0;
            decimal dependentsCost = 0;
            decimal additionalDependentsCost = 0;

            if (employee.IncursAdditionalAnnualCost)
                additionalAnnualCost = DeductAnnualBenefitCost(employee.Salary);
            
            if(employee.Dependents?.Any() == true)
            {
                dependentsCost = DeductDependentsCost(employee.Dependents.Count);
                //this 50 should be a setting! what if age changes?
                additionalDependentsCost = DeductDependentsBenefitsByAge(employee.Dependents, 50);
            }

            var totalDeductions = baseCost + additionalAnnualCost + additionalDependentsCost + dependentsCost;


            //all the foillowing 26s should be in settings
            var paycheckBase = new Paycheck
            {
                GrossPay = employee.Salary,
                EmployeeId = id,
                TotalDeductions = totalDeductions,
                MonthlyBaseDeductionPerPaycheck = baseCost / 26,
                NetPayPerPaycheck = (employee.Salary - totalDeductions) / 26
            };

            //careful with 0/26
            if (dependentsCost > 0)
                paycheckBase.DeductionsPerDependentPerPaycheck = dependentsCost / 26;

            if (additionalAnnualCost > 0)
                paycheckBase.AdditionalAnnualDeductionPerPaycheck = additionalAnnualCost / 26;

            if (additionalDependentsCost > 0)
                paycheckBase.AdditionalDeductionPerDependentPerPaycheck = additionalDependentsCost / 26;

            //couldve used DTO here
            //eventually would be nice for admin to set startDate and endDate from gui
            //using 1/1/2022 - 12/31/2022
            var startDate = new DateTime(2022, 1, 1);
            var newPaychecks = GenerateNewPaychecks(paycheckBase, startDate);

            await context.Paychecks.AddRangeAsync(newPaychecks);

            await context.SaveChangesAsync();

            var paycheckIds = newPaychecks
                .Select(np => np.PaycheckId)
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

        //ILL BE ASKED WHY STATIC? dont access instance data
        private static decimal DeductBaseCost()
        {
            //this 1000 value should be an enum that can be calculated, in case base cost per employee changes
            return 1000 * 12;
        }

        private static decimal DeductDependentsCost(int amountOfDependents)
        {
            return amountOfDependents * 600 * 12;
        }

        //again this 2 should be a constant or setting that an admin can change
        private static decimal DeductAnnualBenefitCost(decimal baseSalary)
        {
            return (2 * baseSalary) / 100;
        }

        private static decimal DeductDependentsBenefitsByAge(List<Dependent> dependents, int age)
        {
            var count = 0;

            foreach(var dependent in dependents)
            {
                if (DateTime.Today.Year - dependent.DateOfBirth.Year > age)
                    count++;
            }

            return count * 200 * 12;
        }

        private static List<Paycheck> GenerateNewPaychecks(Paycheck paycheckBase, DateTime startDate)
        {
            //from startingdate add 2 weeks, do this 26 times
            var paychecks = new List<Paycheck>();

            //this 26 should be a constant or setting!
            for(int i = 0; i < 26; i++)
            {
                var paycheck = new Paycheck
                {
                    GrossPay = paycheckBase.GrossPay,
                    EmployeeId = paycheckBase.EmployeeId,
                    TotalDeductions = paycheckBase.TotalDeductions,
                    MonthlyBaseDeductionPerPaycheck = paycheckBase.MonthlyBaseDeductionPerPaycheck,
                    NetPayPerPaycheck = paycheckBase.NetPayPerPaycheck,
                    DeductionsPerDependentPerPaycheck = paycheckBase.DeductionsPerDependentPerPaycheck,
                    AdditionalAnnualDeductionPerPaycheck = paycheckBase.AdditionalAnnualDeductionPerPaycheck,
                    AdditionalDeductionPerDependentPerPaycheck = paycheckBase.AdditionalDeductionPerDependentPerPaycheck,
                    StartDate = startDate,
                    PayDate = startDate.AddDays(14)
                };

                paychecks.Add(paycheck);

                startDate = startDate.AddDays(14);
            }

            return paychecks;
        }
    }
}
