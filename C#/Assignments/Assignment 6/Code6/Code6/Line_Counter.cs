using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Code6
{
    class Line_Counter
    {
        public void CountLine()
        {
            string filePath;

            while (true)
            {
                Console.Write("Enter the path of the file: ");
                filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    break; 
                }
                else
                {
                    Console.WriteLine("File not found");
                }
            }


            string[] lines = File.ReadAllLines(filePath);

            int lineCount = lines.Length;

            Console.WriteLine($"The file '{filePath}' has {lineCount} lines.");
        }

        static void Main()
        {
            Line_Counter lc = new Line_Counter();
            lc.CountLine();

            Console.ReadLine();
        }
    }
}
