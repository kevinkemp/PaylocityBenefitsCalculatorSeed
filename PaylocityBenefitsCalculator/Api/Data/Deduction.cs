using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    public class Deduction
    {
        [Key]
        public int DeductionId { get; set; }

        [Required]
        public DeductionType Type { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal Amount { get; set; }


        public int EmployeeId {get;set;}

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }  
    }

    public enum DeductionType
    {
        Base,
        DependentMultiplier,
        AnnualAdditionalPercentage,
        MonthlyAdditionalPerDependent
    }
}
