using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data
{
    public class Dependent
    {
        [Key]
        public int DependentId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public RelationshipType Relationship { get; set; }


        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;
    }

    public enum RelationshipType
    {
        None,
        Spouse,
        DomesticPartner,
        Child
    }
}
