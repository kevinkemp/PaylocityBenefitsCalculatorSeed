using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api;
using Api.Data;
using Api.Dtos.Dependent;
using Api.Models;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components.Web;

namespace ApiTests.MockData
{
    public class DependentsMockData
    {

        public static AddDependentWithEmployeeIdDto NewDomesticPartnerDependent()
        {
            return new AddDependentWithEmployeeIdDto
            {
                EmployeeId = 1,
                FirstName = "Marge",
                LastName = "Simpson",
                DateOfBirth = new DateTime(1960, 1, 1),
                Relationship = RelationshipType.DomesticPartner
            };
        }

        public static ApiResponse<AddDependentWithEmployeeIdDto> AddedDependentWithEmployeeId()
        {
            var dto = new AddDependentWithEmployeeIdDto
            {
                EmployeeId = 1,
                FirstName = "Marge",
                LastName = "Simpson",
                DateOfBirth = new DateTime(1960, 1, 1),
                Relationship = RelationshipType.Spouse

            };
            return new ApiResponse<AddDependentWithEmployeeIdDto>
            {
                Data = dto,
                Success = true
            };
        }

        public static ApiResponse<AddDependentWithEmployeeIdDto> UnableToAddDependentWithEmployeeId()
        {
            return new ApiResponse<AddDependentWithEmployeeIdDto>
            {
                Data = null,
                Success = false,
                Message = $"Employee Id: 1 already has a Spouse or Domestic Partner",
                Error = Constants.ErrorCode.InvalidParameter
            };
        }

        public static ApiResponse<List<GetDependentDto>> GetDependents()
        {
            var dependentsList = new List<GetDependentDto>();

            var child1 = new GetDependentDto()
            {
                Id = 1,
                FirstName = "child1",
                LastName = "child1last",
                DateOfBirth = DateTime.Today,
                Relationship = RelationshipType.Child
            };

            var child2 = new GetDependentDto()
            {
                Id = 2,
                FirstName = "child2",
                LastName = "child2last",
                DateOfBirth = DateTime.Today,
                Relationship = RelationshipType.Child
            };

            var spouse = new GetDependentDto()
            {
                Id = 3,
                FirstName = "spouse",
                LastName = "spouselast",
                DateOfBirth = DateTime.Today,
                Relationship = RelationshipType.Spouse
            };

            dependentsList.Add(child1);
            dependentsList.Add(child2);
            dependentsList.Add(spouse);

            return new ApiResponse<List<GetDependentDto>>
            {
                Data = dependentsList,
                Success = true
            };
        }
    }
}
