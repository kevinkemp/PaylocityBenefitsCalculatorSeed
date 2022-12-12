using Api.Controllers;
using Api.Data;
using Api.Dtos.Dependent;
using Api.Models;
using Api.Services;
using ApiTests.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.System.Controllers
{
    public class TestDependentsController
    {
        [Fact]
        public async Task GetAll_ShouldReturn200Status()
        {
            ///Arrange
            var dependentsService = new Mock<IDependentsService>();
            dependentsService.Setup(m => m.GetAllDependents()).ReturnsAsync(DependentsMockData.GetDependents());

            var sut = new DependentsController(dependentsService.Object);

            ///Act
            var result = await sut.GetAll();

            ///Assert
            //using fluent assertions here
            result.GetType().Should().Be(typeof(ActionResult<ApiResponse<List<GetDependentDto>>>));
            result.Value.Success.Should().BeTrue();
        }

        public static IEnumerable<object[]> NewSpouseDependent()
        {
            yield return new object[]
            { new AddDependentWithEmployeeIdDto
                    {
                        EmployeeId = 1,
                        FirstName = "Marge",
                        LastName = "Simpson",
                        DateOfBirth = new DateTime(1960, 1, 1),
                        Relationship = RelationshipType.Spouse
                    }
            };
        }

        [Theory]
        [MemberData(nameof(NewSpouseDependent))]
        public async Task AddDependent_ShouldReturn200Status(AddDependentWithEmployeeIdDto addSpouse)
        {
            ///Arrange
            var dependentsService = new Mock<IDependentsService>();
            dependentsService.Setup(m => m.AddDependent(addSpouse)).ReturnsAsync(DependentsMockData.AddedDependentWithEmployeeId());

            var sut = new DependentsController(dependentsService.Object);

            ///Act
            var result = await sut.AddDependent(addSpouse);

            ///Assert
            result.GetType().Should().Be(typeof(ActionResult<ApiResponse<AddDependentWithEmployeeIdDto>>));
            result.Value.Success.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(NewSpouseDependent))]
        public async Task AddDependent_ShouldReturn400Status(AddDependentWithEmployeeIdDto addSpouse)
        {
            ///Arrange
            var dependentsService = new Mock<IDependentsService>();
            dependentsService.Setup(m => m.AddDependent(addSpouse)).ReturnsAsync(DependentsMockData.UnableToAddDependentWithEmployeeId());

            var sut = new DependentsController(dependentsService.Object);

            ///Act
            var actionResult = await sut.AddDependent(addSpouse);

            ///Assert
            var result = actionResult.Result as BadRequestObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
        }
    }
}
