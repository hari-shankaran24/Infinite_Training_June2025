using System;


namespace Code2
{
    class Program
    {
        static void Swap() //Swaps two numbers
        {
            Console.WriteLine("Enter num1: ");
            int num1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the num2: ");
            int num2 = int.Parse(Console.ReadLine());

            Console.WriteLine($"Before swapping \n num1 = {num1} \n num2 = {num2} \n  ");
            int temp;
            temp = num1;
            num1 = num2;
            num2 = temp;

            Console.WriteLine($"After swapping \n num1 = {num1} \n num2 = {num2} \n ");
            Console.ReadLine();
        }


        static void Main(string[] args)
        {
            // Program.Swap();
            //four_times.rows();
            //integer_to_day.day();
            //avg_arr.array();
            //avg_arr.ten_marks();
            //avg_arr.arr_copy();
            String.str();
            String.rev();
            String.same_word();

        }
    }
}
