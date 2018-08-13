using System;
using System.Collections.Generic;

namespace CalculatorLambda
{
    public class Calculator
    {
        public static decimal Add(decimal num_1, decimal num_2)
        {
            return num_1 + num_2;
        }

        public static decimal Add(List<decimal> numbers)
        {
            var total = (decimal)0;

            foreach (var number in numbers)
            {
                total += number;
            }

            return total;
        }

        public static decimal Subtract(decimal num_1, decimal num_2)
        {
            return num_1 - num_2;
        }

        public static decimal Multiply(decimal num_1, decimal num_2)
        {
            return num_1 * num_2;
        }

        public static decimal Divide(decimal num_1, decimal num_2)
        {
            return num_1 / num_2;
        }
    }
}
