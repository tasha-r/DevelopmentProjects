using System.Collections.Generic;
using Xunit;

namespace CalculatorLambda.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_WhenCalledWithTwoIntegers_AddsNumbersAndReturnsResult()
        {
            var result = Calculator.Add(2, 3);

            Assert.Equal(5, result);
        }

        [Fact]
        public void Add_WhenCalledWithTwoFloats_AddsNumbersAndReturnsResult()
        {
            var result = Calculator.Add((float)2.65, (float)3.1);

            Assert.Equal(5.75, result);
        }

        [Fact]
        public void Add_WhenCalledWithMultipleNumbers_AddsNumbersAndReturnsResult()
        {
            var numbers = new List<float> { 10, (float)5.5, (float)6.2, 2 };

            var result = Calculator.Add(numbers);

            Assert.Equal(23.72, result);
        }

        [Fact]
        public void Subtract_WhenCalledWithTwoIntegers_SubtractsNumbersAndReturnsResult()
        {
            var result = Calculator.Subtract(5, -2);

            Assert.Equal(7, result);
        }

        [Fact]
        public void Subtract_WhenCalledWithTwoFloats_SubtractsNumbersAndReturnsResult()
        {
            var result = Calculator.Subtract((float)6.348, (float)1.74);

            Assert.Equal((float)4.60799980163574, result);
        }

        [Fact]
        public void Multiply_WhenCalledWithTwoIntegers_MultipliesNumbersAndReturnsResult()
        {
            var result = Calculator.Multiply(6, -2);

            Assert.Equal(-12, result);
        }

        [Fact]
        public void Multiply_WhenCalledWithTwoFloats_MultipliesNumbersAndReturnsResult()
        {
            var result = Calculator.Multiply((float)6.5, (float)5.5);

            Assert.Equal(35.75, result);
        }

        [Fact]
        public void Divide_WhenCalledWithTwoIntegers_DividesNumbersAndReturnsResult()
        {
            var result = Calculator.Divide(10, 2);

            Assert.Equal(5, result);
        }

        [Fact]
        public void Divide_WhenCalledWithTwoFloats_DividesNumbersAndReturnsResult()
        {
            var result = Calculator.Divide((float)10.8, (float)5.4);

            Assert.Equal(2, result);
        }
    }
}
