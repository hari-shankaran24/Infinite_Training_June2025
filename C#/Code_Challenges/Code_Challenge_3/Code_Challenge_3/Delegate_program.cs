using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Challenge_3
{
    public delegate int Calc_Op(int a, int b);

    class Delegate_program
    {
        static int Perform_Op(int a, int b, Calc_Op operation)
        {
            return operation(a, b);
        }

        static void Main(string[] args)
        {
            Calc_Op add = (a, b) => a + b;
            Calc_Op sub = (a, b) => a - b;
            Calc_Op mul = (a, b) => a * b;

            Console.Write("Enter the number of times to perform the operations: ");
            int times = int.Parse(Console.ReadLine());

            for (int i = 0; i < times; i++)
            {
                Console.Write("\nEnter the first number: ");
                int num1 = int.Parse(Console.ReadLine());

                Console.Write("Enter the second number: ");
                int num2 = int.Parse(Console.ReadLine());

                Console.WriteLine($"Addition: {Perform_Op(num1, num2, add)}");
                Console.WriteLine($"Subtraction: {Perform_Op(num1, num2, sub)}");
                Console.WriteLine($"Multiplication: {Perform_Op(num1, num2, mul)}");
            }

            Console.ReadLine();
        }
    }
}
