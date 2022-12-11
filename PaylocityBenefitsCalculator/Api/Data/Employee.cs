using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Data
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

        [Precision(18, 2)]
        public decimal Salary { get; set; }

        public bool IncursAdditionalYearlyCost { get; set; }

        //could add Active bool, maybe employee terminated

        //could add a more unique identifier like SSN to prevent dupes

        //SHOULD I REMOVE VIRTUALS? DO I NEED LAZY LOADING?
        public virtual List<Dependent>? Dependents { get; set; }
        public virtual List<Paycheck>? Paychecks { get; set; }
    }
}
