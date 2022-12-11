using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Paycheck
{
    public class GetPaycheckDto
    {
        public int PaycheckId { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal GrossPay { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal TotalDeductions { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal MonthlyBaseDeductionPerPaycheck { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal DeductionsPerDependentPerPaycheck { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal AdditionalAnnualDeductionPerPaycheck { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal AdditionalDeductionPerDependentPerPaycheck { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal NetPayPerPaycheck { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime PayDate { get; set; }

        public int EmployeeId { get; set; }
    }
}
