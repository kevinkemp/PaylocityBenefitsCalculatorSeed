using Api.Dtos.Paycheck;
using Api.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;

using Xunit;

namespace ApiTests.System.Services
{
    public class TestCalculatorService
    {
        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        public void GetDependentsBaseCostPerYear_WithNumberOfDependents_ReturnsValidDecimal(int numberOfDependents)
        {
            ///Arrange
            var calculatorService = new CalculatorService();

            ///Act
            var result = calculatorService.GetDependentsBaseCostPerYear(numberOfDependents);

            ///Assert
            result.Should().BePositive();
        }

        [Theory]
        [InlineData("100000.00")]
        public void GetAdditionalYearlyCostByPercentage_WithPositiveBaseSalary_ReturnsValidDecimal(string deci)
        {
            ///Arrange
            var baseSalary = Convert.ToDecimal(deci);
            var calculatorService = new CalculatorService();

            ///Act
            var result = calculatorService.GetAdditionalYearlyCostByPercentage(baseSalary);

            ///Assert
            result.Should().BePositive();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void GetAdditionalDependentsCostByAgePerYear_WithNumberOfDependents_ReturnsValidDecimal(int numberOfDependentsOverAge)
        {
            ///Arrange
            var calculatorService = new CalculatorService();

            ///Act
            var result = calculatorService.GetAdditionalDependentsCostByAgePerYear(numberOfDependentsOverAge);

            ///Assert
            result.Should().BePositive();
        }

        public static IEnumerable<object[]> ValidBasePaycheck()
        {
            yield return new object[]
            { new GenerateBasePaycheckDto
                    {
                        EmployeeId = 1,
                        BaseSalary = 100000,
                        IncursAdditionalYearlyCost = true,
                        NumberOfDependents = 3,
                        NumberOfDependentsOverAge = 2,
                        BaseCostPerYear = 12000,
                        DependentsBaseCostPerYear = 21600,
                        AdditionalYearlyCostByPercentage = 2000,
                        AdditionalDependentsCostByAgePerYear = 4800
                    }
            };
        }

        [Theory]
        [MemberData(nameof(ValidBasePaycheck))]
        public void GeneratePaychecksForYear_WithValidBasePaycheck_ReturnsListOfPaychecks(GenerateBasePaycheckDto paycheckDto)
        {
            ///Arrange
            var calculatorService = new CalculatorService();

            ///Act
            var result = calculatorService.GeneratePaychecksForYear(paycheckDto);
            decimal totalGrossPay = 0;
            foreach(var paycheck in result)
            {
                totalGrossPay = totalGrossPay + paycheck.GrossPayPerPaycheck;
            }

            //some decimals are off if i dont round, shouldve created a Helper or Extension to deal with these rounding issues
            totalGrossPay = decimal.Round(totalGrossPay, 2, MidpointRounding.AwayFromZero);

            ///Assert
            result.Should().NotBeEmpty()
                .And.HaveCount(26);

            totalGrossPay.Should().Be(paycheckDto.BaseSalary);
        }
    }
}
