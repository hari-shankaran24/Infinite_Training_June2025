using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code7
{
    class Filter
    {
        static void Main()
        {
            List<string> words = new List<string>();

            Console.Write("Enter the words: ");
            int count = int.Parse(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Console.Write($"Enter word {i + 1}: ");
                string word = Console.ReadLine();
                words.Add(word.ToLower());
            }

            var result = words
                .Where(w => w.StartsWith("a") && w.EndsWith("m"));

            Console.WriteLine("\nWords starting with 'a' and ending with 'm':");
            foreach (var word in result)
            {
                Console.WriteLine(word);
            }

            Console.ReadLine();
        }
    }
}
