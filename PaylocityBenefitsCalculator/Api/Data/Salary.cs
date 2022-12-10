using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    public class Salary
    {
        [Key]
        public int SalaryId { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal BaseAmount { get; set; }

        public string? Note { get; set; }


        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}
