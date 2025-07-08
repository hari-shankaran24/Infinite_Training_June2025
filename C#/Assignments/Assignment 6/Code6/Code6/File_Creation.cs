using System;
using System.IO;

namespace Code6
{
    class File_Creation
    {
        public void FileArr()
        {
            int NoOfLines;

            while (true)
            {
                Console.Write("Enter how many lines to write: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out NoOfLines) && NoOfLines > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter a number greater than 0.");
                }
            }

            string[] userIP = new string[NoOfLines];

            for (int i = 0; i < NoOfLines; i++)
            {
                Console.Write($"Enter Line {i + 1}: ");
                userIP[i] = Console.ReadLine();
            }

            string filePath = "Result.txt"; // updated extension to .txt

            try
            {
                File.WriteAllLines(filePath, userIP);
                Console.WriteLine($"\nYour Input is written in '{filePath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in writing to file: " + ex.Message);
            }
        }

        static void Main()
        {
            File_Creation fc = new File_Creation();
            fc.FileArr();

            Console.ReadLine(); 
        }
    }
}
