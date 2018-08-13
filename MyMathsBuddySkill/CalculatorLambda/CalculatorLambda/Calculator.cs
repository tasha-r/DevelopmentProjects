using System;
using System.Collections.Generic;

namespace CalculatorLambda
{
    public class Calculator
    {
        public static float Add(float num_1, float num_2)
        {
            return num_1 + num_2;
        }

        public static float Add(List<float> numbers)
        {
            var total = (float)0;

            foreach (var number in numbers)
            {
                total += number;
            }

            return total;
        }

        public static float Subtract(float num_1, float num_2)
        {
            return num_1 - num_2;
        }

        public static float Multiply(float num_1, float num_2)
        {
            return num_1 * num_2;
        }

        public static float Divide(float num_1, float num_2)
        {
            return num_1 / num_2;
        }
    }
}
