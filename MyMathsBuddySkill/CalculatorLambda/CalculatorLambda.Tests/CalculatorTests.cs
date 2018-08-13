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
            var result = Calculator.Add((decimal)2.65, (decimal)3.1);

            Assert.Equal((decimal)5.75, result);
        }

        [Fact]
        public void Add_WhenCalledWithMultipleNumbers_AddsNumbersAndReturnsResult()
        {
            var numbers = new List<decimal> { 10, (decimal)5.5, (decimal)6.2, 2 };

            var result = Calculator.Add(numbers);

            Assert.Equal((decimal)23.7, result);
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
            var result = Calculator.Subtract((decimal)6.34, (decimal)1.742);

            Assert.Equal((decimal)4.598, result);
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
            var result = Calculator.Multiply((decimal)6.5, (decimal)5.5);

            Assert.Equal((decimal)35.75, result);
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
            var result = Calculator.Divide((decimal)10.8, (decimal)5.4);

            Assert.Equal(2, result);
        }
    }
}
