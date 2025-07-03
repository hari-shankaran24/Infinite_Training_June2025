using System;

namespace Code_Challenge_2
{
    class Validator
    {
        public static void CheckNumber(int num)
        {
            if (num < 0)
                throw new Exception("Number is negative.");
        }
    }

    class program
    {
        static void Main()
        {
            Console.Write("Enter a number: ");
            int number = int.Parse(Console.ReadLine());

            try
            {
                Validator.CheckNumber(number);
                Console.WriteLine("Number is valid.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
