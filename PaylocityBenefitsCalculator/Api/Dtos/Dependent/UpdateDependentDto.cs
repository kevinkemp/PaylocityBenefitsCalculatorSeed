using Api.Models;
using Api.Data;

namespace Api.Dtos.Dependent
{
    public class UpdateDependentDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public RelationshipType Relationship { get; set; }
    }
}
