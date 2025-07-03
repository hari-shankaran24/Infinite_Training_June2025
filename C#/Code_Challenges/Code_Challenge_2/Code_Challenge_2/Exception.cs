using System;

namespace Code_Challenge_2
{
    class NumberValidator
    {
        static void CheckNumber(int num)
        {
            if (num < 0)
                throw new ArgumentException("Number is negative.");
        }

        static void Main()
        {
            Console.Write("Enter a number: ");
            int number = int.Parse(Console.ReadLine());

            try
            {
                CheckNumber(number);
                Console.WriteLine("Number is valid.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
