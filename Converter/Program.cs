using System;
using System.Linq;

namespace Converter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите число");
                    var result = ConvertToDouble(Console.ReadLine());
                    Console.WriteLine($"Число: {result}");
                    Console.WriteLine("-----------------------");
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Пустая строка!");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Строка должна содержать еще символы, кроме знака '+' или '-' ");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Слишком большое число для типа double");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }


        public static double ConvertToDouble(string value)
        {
            if (value is null || value.Length == 0) throw new ArgumentNullException(nameof(value));
            bool isNegative = false;
            int startChar = 0;
            switch (value[0])
            {
                case '-':
                    if (value.Length == 1) throw new ArgumentException("Надо еще цифр");
                    startChar = 1;
                    isNegative = true;
                    break;
                case '+':
                    if (value.Length == 1) throw new ArgumentException("Надо еще цифр");
                    break;
            }
            var splitedArray = value.Split(new char[] { ',', '.' }).ToArray();
            double leftPartSum = 0.0;
            double rightPartSum = 0.0;
            var leftPart = splitedArray[0];
            var rate = 0;
            for (int i = leftPart.Length - 1; i >= startChar; i--)
            {
                if (leftPart[i] < '0')
                    if (leftPartSum == double.PositiveInfinity || leftPartSum == double.NegativeInfinity) throw new OverflowException();
                leftPartSum += Math.Pow(10, rate) * CharToInt(leftPart[i]);
                rate++;
            }

            if (splitedArray.Length > 1)
            {
                var rightPath = splitedArray[splitedArray.Length - 1];
                for (int i = 0; i < rightPath.Length; i++)
                {
                    if (rightPartSum == double.PositiveInfinity || rightPartSum == double.NegativeInfinity) throw new OverflowException();
                    rightPartSum += (CharToInt(rightPath[i])) * Math.Pow(0.1, i + 1);
                }
            }
            double result = leftPartSum + rightPartSum;
            if (result == double.PositiveInfinity || result == double.NegativeInfinity) throw new OverflowException();
            if (isNegative)
            {
                result *= -1;
            }

            return result;
        }

        private static int CharToInt(char c) =>
           CanConvertToInt(c) ? c - '0' : throw new FormatException("Входная строка имеет не верный формат");


        private static bool CanConvertToInt(char c) =>
            c >= '0' && c <= '9'
            ? true
            : false;
    }
}
