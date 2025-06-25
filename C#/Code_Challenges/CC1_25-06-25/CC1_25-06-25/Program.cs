using System;


namespace CC1_25_06_25
{
    class Program
    {
        //------------------------------------------Character Removal of the String-----------------------------------------------------
        public static void char_removal()
        {
            Console.WriteLine("Enter the String: ");
            string word1 = Console.ReadLine();

            Console.WriteLine("Enter the position:");
            int pos = int.Parse(Console.ReadLine());
            string remStr = word1.Remove(pos, 2);


            Console.WriteLine($"The character removed string is: {remStr} ");
            Console.ReadLine();
        }

        //-------------------------------------------First and Last Character Exchange in a string---------------------------------------

        public static void Swap()
        {
            Console.WriteLine("Enter a string:");
            string input = Console.ReadLine();

            if (input.Length <= 1)
            {
                Console.WriteLine($"The resultant string is: {input}");
            }
            else
            {
                char first = input[0];
                char last = input[input.Length - 1];
                string middle = input.Substring(1, input.Length - 2);
                string result = last + middle + first;
                Console.WriteLine($"The modified string is : {result}");
            }
            Console.ReadLine();
        }

        //-------------------------------------------Greatest number among the three integers---------------------------------------

        public static void greatest_number()
        {
            Console.WriteLine("Enter three numbers separated by spaces ");
            string[] input = Console.ReadLine().Split(' ');

            int num1 = int.Parse(input[0]);
            int num2 = int.Parse(input[1]);
            int num3 = int.Parse(input[2]);

            if (num1 > num2 && num1 > num3)
            {
                Console.WriteLine($"The greatest integer is {num1}");
            }
            else if (num2 > num1 && num2 > num3)
            {
                Console.WriteLine($"The greatest integer is {num2}");
            }
            else
            {
                Console.WriteLine($"The greatest integer is {num3}");
            }
            Console.ReadLine();

        }
        static void Main(string[] args)
        {
            //Program.char_removal();
            //Program.Swap();
            Program.greatest_number();
        }
    }
}
