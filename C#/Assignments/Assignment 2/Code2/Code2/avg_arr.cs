using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code2
{
    class avg_arr
    {


        //-----------------------------------------------------------Array First Program------------------------------------------------------
        public static void array()
        {
            Console.WriteLine("Enter the number of elements in the array: ");
            int size = int.Parse(Console.ReadLine());
            int[] elements = new int[size];

            for (int i = 0; i < size; i++)
            {
                Console.WriteLine("Enter the array elements: ");
                elements[i] = int.Parse(Console.ReadLine());
            }

            for (int j = 0; j < size; j++)
            {
                Console.Write(elements[j] + " ");
            }



            int min_of_array = elements.Min();
            int max_of_array = elements.Max();
            double avg_of_array = elements.Average();

            Console.WriteLine("\n Avgerage value of the array is:" + avg_of_array);
            Console.WriteLine("Minimum value of the array is :" + min_of_array);
            Console.WriteLine("Maximum value of the array is : " + max_of_array);
            Console.ReadLine();

        }
        //-------------------------------------------------------------Array Second Program---------------------------------------------------------

        public static void ten_marks()
        {

            int[] marks = new int[10];

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Enter the marks {i + 1}:  ");
                marks[i] = int.Parse(Console.ReadLine());
            }

            int total = marks.Sum();
            double average = marks.Average();
            int minimum = marks.Min();
            int maximum = marks.Max();
            Array.Sort(marks);


            Console.WriteLine($" \n The Total marks is: {total}");
            Console.WriteLine($" The Average of the marks is: {average}");
            Console.WriteLine($" The Minimum of the marks is: {minimum}");
            Console.WriteLine($" The Maximum of the marks is: { maximum}");
            Console.WriteLine(" The Sorted array in ascending order is : ");

            foreach (int i in marks)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine("\n The Sorted array in descending order is : ");

            Array.Reverse(marks);
            foreach (int i in marks)
            {
                Console.Write(i + " ");
            }

            Console.ReadLine();

        }

        //-------------------------------------------------------------Array Third Program---------------------------------------------------------


        public static void arr_copy()
        {
            Console.Write("Enter the number of elements in the array: ");
            int size = int.Parse(Console.ReadLine());

            int[] array_org = new int[size];
            int[] array_copy = new int[size];

            for (int i = 0; i < size; i++)
            {
                Console.Write($"Enter element {i + 1}: ");
                array_org[i] = int.Parse(Console.ReadLine());
            }

            for (int i = 0; i < size; i++)
            {
                array_copy[i] = array_org[i];
            }

            Console.WriteLine("Copied array elements:");
            for (int i = 0; i < size; i++)
            {
                Console.Write(array_copy[i] + " ");
            }

            Console.ReadLine();


        }
    }
}
