using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code2
{
    class String
    {
        //------------------------------Returns the length of the string---------------------------
        public static void str()
        {
            Console.WriteLine("Enter a word: ");
            string word = Console.ReadLine();

            int length = word.Length;
            Console.WriteLine($"The length of the word is: {length}");
        }
//---------------------------------------Reverse the string------------------------------------------
        public static void rev()
        {
            Console.Write("Enter a word: ");
            string word = Console.ReadLine();

            string reversed = "";
            for (int i = word.Length - 1; i >= 0; i--)
            {
                reversed += word[i];
            }

            Console.WriteLine($"The reversed word is: {reversed}");
        }

        //-----------------------------------Checks if the 2 words are same------------------------------------------

        public static void same_word()
        {
            Console.Write("Enter the first word: ");
            string word1 = Console.ReadLine();

            Console.Write("Enter the second word: ");
            string word2 = Console.ReadLine();

            if (word1.Equals(word2))
            {
                Console.WriteLine("The words are the same.");
            }
            else
            {
                Console.WriteLine("The words are different.");


            }
            Console.ReadLine();


        }
    }
}
