using System;


namespace Code1
{
    class Program
    {
        static int num1, num2;
        //-----------------------------------------Number is Equal or Not---------------------------------------
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
        //------------------------------------------Number is Positive or Negative-------------------------------------------
        static void Pos_or_Neg() // Checks whether the given number is positive or negative
        {
            Console.WriteLine("Enter a Number: ");
            num1 = int.Parse(Console.ReadLine());

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
        //-----------------------------------------Arithmetic Operation--------------------------------------
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


        //-----------------------------------------Multiplication table of a number--------------------------------------
        static void Mul_Tab()
        {
            Console.WriteLine("Enter the number: ");
            num2 = int.Parse(Console.ReadLine());

            for(int i=0; i<=10; i++)
            {
                Console.WriteLine($"{num2} * {i} = {num2 * i}");
            }
            Console.ReadLine();
        }

        //-----------------------------------------Multiplication table of a number--------------------------------------
            static void Triple_sum()
        {
            Console.WriteLine("Enter the first number: ");
            num1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the second number: ");
            num2 = int.Parse(Console.ReadLine());

            int sum = num1 + num2;

            if (num1 == num2)
            {
                Console.WriteLine($"The Sum is: {sum}");
                Console.WriteLine($"The triple of the sum is {sum * 3}");
            }
            Console.ReadLine();

        }


        static void Main(string[] args)
        {
            //Program.Equal_Or_Not();
            //Program.Pos_or_Neg();
            //Program.Arithmetic_Operation();
            //Program.Mul_Tab();
            Program.Triple_sum();
        }

    }

      
    }

