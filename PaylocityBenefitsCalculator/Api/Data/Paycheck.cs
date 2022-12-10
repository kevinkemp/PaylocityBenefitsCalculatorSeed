using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
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

        [Required]
        [Precision(18, 2)]
        public decimal NetPay { get; set; }


        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public int PeriodId { get; set; }

        [ForeignKey("PeriodId")]
        public virtual TimePeriod Period { get; set; }
    }
}
