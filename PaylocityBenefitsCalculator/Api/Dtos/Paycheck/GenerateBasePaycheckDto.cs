namespace Api.Dtos.Paycheck
{
    public class GenerateBasePaycheckDto
    {
        public int EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public bool IncursAdditionalYearlyCost { get; set; }
        public int NumberOfDependents { get; set; }
        public int NumberOfDependentsOverAge { get; set; }
        public decimal BaseCostPerYear { get; set; }
        public decimal DependentsBaseCostPerYear { get; set; }
        public decimal AdditionalYearlyCostByPercentage { get; set; }
        public decimal AdditionalDependentsCostByAgePerYear { get; set; }
    }
}
