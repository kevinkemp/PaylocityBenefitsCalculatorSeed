using Api.Data;

namespace Api.Models
{
    public class AddEmployeeValidationResult
    {
        //bool Valid
        //List<string> fields
        //should validate that dependents are OK to add, if not return with error invalid dependents
        public bool Valid { get; set; }
        public List<string> Fields { get; set; } = new List<string>();

    }
}
