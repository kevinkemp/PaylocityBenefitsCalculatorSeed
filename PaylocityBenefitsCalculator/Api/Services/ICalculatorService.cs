using Api.Data;
using Api.Dtos.Paycheck;

namespace Api.Services
{
    public interface ICalculatorService
    {
        decimal GetBaseCostPerYear();
        decimal GetDependentsBaseCostPerYear(int numberOfDependents);
        decimal GetAdditionalYearlyCostByPercentage(decimal baseSalary);
        decimal GetAdditionalDependentsCostByAgePerYear(int numberOfDependentsOverAge);
        List<Paycheck> GeneratePaychecksForYear(GenerateBasePaycheckDto paycheckDto);
    }
}
