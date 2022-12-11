namespace Api
{
    //the values in this class would ideally be obtained from a database, in the future an admin should be able to modify these values
    public static class Constants
    {
        public static class CalculatorValues
        {
            public static readonly DateTime StartDate = new DateTime(2023, 1, 1);
            public static readonly int NumberOfPaychecks = 26;
            public static readonly int MinimumAgeToIncurAdditionalDependentCost = 50;
            public static readonly decimal BaseCost = 1000;
            public static readonly decimal BaseCostPerDependent = 600;
            public static readonly decimal AdditionalYearlyPercentage = 2;
            public static readonly decimal AdditionalCostPerDependent = 200;
            public static readonly decimal MinimumBaseSalaryToIncurAdditionalCost = 80000;

        }
    }
}
