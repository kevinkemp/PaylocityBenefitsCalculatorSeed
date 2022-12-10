using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }


        public virtual List<Dependent>? Dependents { get; set; }
        public virtual List<Paycheck>? Paychecks { get; set; }

        public int? SalaryId { get; set; }

        [ForeignKey("SalaryId")]
        public virtual Salary Salary { get; set; }
    }
}
