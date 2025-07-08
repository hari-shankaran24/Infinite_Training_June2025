using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter how many numbers you want to input: ");
            int count = int.Parse(Console.ReadLine());

            int[] numbers = new int[count];

            for (int i = 0; i < count; i++)
            {
                Console.Write($"Enter number {i + 1}: ");
                numbers[i] = int.Parse(Console.ReadLine());
            }

            var result = numbers
                .Where(n => n * n > 20)
                .Select(n => $"{n} - {n * n}");

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
