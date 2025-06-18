using System;


namespace Code1
{
    class Program
    {
        static int num1, num2;

        static void Equal_Or_Not() //Checks whether the given 2 integers are equal or not
        {
            Console.Write("Enter the First integer: ");
            num1 = int.Parse(Console.ReadLine());

            Console.Write("Enter the Second integer: ");
            num2 = int.Parse(Console.ReadLine());   

            if (num1 == num2)
            {
                Console.WriteLine("The two integers are equal");
            }
            else
            {
                Console.WriteLine("The two integers are not equal");
            }

            Console.ReadLine();
        }

        static void Pos_or_Neg() // Checks whether the given number is positive or negative
        {
            Console.WriteLine("Enter a Number: ");
            int num1 = int.Parse(Console.ReadLine());

            if (num1 < 0)
            {
                Console.WriteLine("The given number is Negative ");
            }
            else if (num1 == 0)
            {
                Console.WriteLine("The given number is Zero");
            }
            else
            {
                Console.WriteLine("The given number is Positive");
            }
            Console.ReadLine();
        }

        static void Arithmetic_Operation() //Does the arithmetic operation of the given 2 numbers
        {

                Console.Write("Enter the First integer: ");
                num1 = int.Parse(Console.ReadLine());

                Console.Write("Enter the Second integer: ");
                num2 = int.Parse(Console.ReadLine());

                Console.Write("Enter the operation (+, -, *, / )");
                char op = char.Parse(Console.ReadLine());

                switch (op)
                {
                    case '+':
                        Console.WriteLine($"{num1} + {num2} = {num1 + num2}");
                        break;

                    case '-':
                        Console.WriteLine($"{num1} - {num2} = {num1 - num2}");
                        break;

                    case '*':
                        Console.WriteLine($"{num1} * {num2} = {num1 * num2}");
                        break;

                    case '/':
                        if (num2 != 0)
                            Console.WriteLine($"{num1} / {num2} = {num1 / (double)num2}");
                        else
                            Console.WriteLine("Division by zero is not allowed.");
                        break;

                    default:
                        Console.WriteLine("Invalid operation.");
                        break;
                }

                Console.ReadLine(); 
            }

        static void Main(string[] args)
        {
            Program.Equal_Or_Not();
            Program.Pos_or_Neg();
            Program.Arithmetic_Operation();
        }
    }

      
    }

