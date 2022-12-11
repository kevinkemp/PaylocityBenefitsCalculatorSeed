using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data
{
    public class Paycheck
    {
        [Key]
        public int PaycheckId { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal GrossPay { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal TotalDeductions { get; set; }


        //DO I NEED MONTHLY BASE DEDUCTION HERE?
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

        //maybe not needed!!
        //public bool Paid { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;
    }
}
