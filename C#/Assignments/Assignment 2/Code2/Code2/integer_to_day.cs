using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code2
{
    class integer_to_day
    {
        public static void day()
        {
            Console.WriteLine("Enter a number between 1 to 7: ");
            int num = int.Parse(Console.ReadLine());

            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            
            for(int i=0; i<=7; i++)
            {
                if (i == num)
                {
                    Console.WriteLine(days[i - 1]);
                    break;

                }
            }
            Console.ReadLine();
        
        }


    }
}
