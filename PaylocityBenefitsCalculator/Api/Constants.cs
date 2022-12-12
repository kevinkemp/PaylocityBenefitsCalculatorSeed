using System.Net;

namespace Api
{
    public static class Constants
    {
        //the values in this class would ideally be obtained from a database, in the future an admin should be able to modify these values
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

        public static class ErrorCode
        {
            public static readonly string InvalidParameter = "INVALID_PARAMETER";
            public static readonly string DependentNotFound = "DEPENDENT_NOT_FOUND";
            public static readonly string EmployeeNotFound = "EMPLOYEE_NOT_FOUND";
            public static readonly string PaycheckNotFound = "PAYCHECK_NOT_FOUND";
        }

        public static class ErrorDictionary
        {
            public static Dictionary<string, HttpStatusCode> GetHttpError()
            {
                return new Dictionary<string, HttpStatusCode>()
                {
                    { ErrorCode.InvalidParameter, HttpStatusCode.BadRequest },
                    { ErrorCode.DependentNotFound, HttpStatusCode.NotFound },
                    { ErrorCode.EmployeeNotFound, HttpStatusCode.NotFound },
                };
            }
        }
    }
}
