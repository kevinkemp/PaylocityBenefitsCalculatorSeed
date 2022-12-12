using Api.Data;
using Api.Dtos.Paycheck;
using System.Net;

namespace Api.Services
{
    public class CalculatorService : ICalculatorService
    {
        public CalculatorService()
        {

        }

        public decimal GetBaseCostPerYear()
        {
            //base cost * 12 months
            return Constants.CalculatorValues.BaseCost * 12;
        }

        public decimal GetDependentsBaseCostPerYear(int numberOfDependents)
        {
            return numberOfDependents * Constants.CalculatorValues.BaseCostPerDependent * 12;
        }

        public decimal GetAdditionalYearlyCostByPercentage(decimal baseSalary)
        {
            return (Constants.CalculatorValues.AdditionalYearlyPercentage * baseSalary) / 100;
        }

        //maybe a better name for this one
        public decimal GetAdditionalDependentsCostByAgePerYear(int numberOfDependentsOverAge)
        {
            return numberOfDependentsOverAge * Constants.CalculatorValues.AdditionalCostPerDependent * 12;
        }

        //could add a year datetime to target -> 2025
        public List<Paycheck> GeneratePaychecksForYear(GenerateBasePaycheckDto paycheckDto)
        {
            decimal totalDeductions = paycheckDto.BaseCostPerYear + paycheckDto.DependentsBaseCostPerYear + paycheckDto.AdditionalYearlyCostByPercentage + paycheckDto.AdditionalDependentsCostByAgePerYear;
            decimal totalDeductionsPerPaycheck = totalDeductions / Constants.CalculatorValues.NumberOfPaychecks;
            decimal baseCostPerPaycheck = paycheckDto.BaseCostPerYear / Constants.CalculatorValues.NumberOfPaychecks;
            decimal netPayPerPaycheck = (paycheckDto.BaseSalary - totalDeductions) / Constants.CalculatorValues.NumberOfPaychecks;
            decimal grossPayPerPaycheck = paycheckDto.BaseSalary / Constants.CalculatorValues.NumberOfPaychecks;
            decimal baseCostPerDependentPerPaycheck = paycheckDto.DependentsBaseCostPerYear / Constants.CalculatorValues.NumberOfPaychecks; 
            decimal additionalYearlyDeductionPerPaycheck = paycheckDto.AdditionalYearlyCostByPercentage / Constants.CalculatorValues.NumberOfPaychecks;
            decimal additionalDeductionPerDependentPerPaycheck = paycheckDto.AdditionalDependentsCostByAgePerYear / Constants.CalculatorValues.NumberOfPaychecks;

            //careful with 0/26 DUNNO IF THESE ARE NECESSARY?>??
            //if (paycheckDto.DependentsBaseCostPerYear > 0)
            //baseCostPerDependentPerPaycheck = paycheckDto.DependentsBaseCostPerYear / Constants.CalculatorValues.NumberOfPaychecks;

            //if (paycheckDto.AdditionalYearlyCostByPercentage > 0)
            //additionalYearlyDeductionPerPaycheck = paycheckDto.AdditionalYearlyCostByPercentage / Constants.CalculatorValues.NumberOfPaychecks;

            //if (paycheckDto.AdditionalDependentsCostByAgePerYear > 0)
            //additionalDeductionPerDependentPerPaycheck = paycheckDto.AdditionalDependentsCostByAgePerYear / Constants.CalculatorValues.NumberOfPaychecks;

            //using jan first 2023 for example, ideally this value can be set by admin 
            var startDate = Constants.CalculatorValues.StartDate;

            //calculation could be much more complex than this, years dont have same days, numberofpaychecks could be odd number, etc
            var cadenceInDays = 365 / Constants.CalculatorValues.NumberOfPaychecks; //this should output 14 so payments are every 14 days

            var paychecks = new List<Paycheck>();

            //this 26 should be a constant or setting!
            for (int i = 0; i < 26; i++)
            {
                var paycheck = new Paycheck
                {
                    EmployeeId = paycheckDto.EmployeeId,
                    GrossPayPerPaycheck = grossPayPerPaycheck,
                    TotalDeductionsPerPaycheck = totalDeductionsPerPaycheck,
                    MonthlyBaseDeductionPerPaycheck = baseCostPerPaycheck,
                    NetPayPerPaycheck = netPayPerPaycheck,
                    DeductionsPerDependentPerPaycheck = baseCostPerDependentPerPaycheck,
                    AdditionalYearlyDeductionPerPaycheck = additionalYearlyDeductionPerPaycheck,
                    AdditionalDeductionPerDependentPerPaycheck = additionalDeductionPerDependentPerPaycheck,
                    StartDate = startDate,
                    PayDate = startDate.AddDays(cadenceInDays)
                };

                paychecks.Add(paycheck);

                startDate = paycheck.PayDate;
            }

            return paychecks;
        }

    }
}
//if these were CPU-bound heavy calculation methods we could use Task.Run to achieve parallelism(using more threads) not asynchronicity(using less threads)
//Task.Run executes the method on a thread pool thread and returns a Task representing the completion of that method.

//class MyService
//{
//    public int CalculateMandelbrot()
//    {
//        // Tons of work to do in here!
//        for (int i = 0; i != 10000000; ++i)
//            ;
//        return 42;
//    }
//}

//private async void MyButton_Click(object sender, EventArgs e)
//{
//    await Task.Run(() => myService.CalculateMandelbrot());
//}