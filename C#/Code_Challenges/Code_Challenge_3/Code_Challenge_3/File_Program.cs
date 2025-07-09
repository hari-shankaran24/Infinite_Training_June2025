using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Code_Challenge_3
{
    class File_Program
    {
        static void Main()
        {
            string filePath = "example.txt";
            string textToAppend = "Hi, The text got appended!!!";

            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine(textToAppend);
                    }
                    Console.WriteLine("Text is appended to the existing file.");
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(filePath))
                    {
                        sw.WriteLine(textToAppend);
                    }
                    Console.WriteLine("File created and text is written to it.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            Console.ReadLine();
        }

    }
}
