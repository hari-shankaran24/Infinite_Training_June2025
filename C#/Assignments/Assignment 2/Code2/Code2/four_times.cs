using System;


namespace Code2
{
    class four_times
    {
       public static void rows() //Prints the given number for given number of times with and without blankspaces in each separate rows
        {
            Console.WriteLine("Enter a number: ");
            int num = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of times to be printed: ");
            int times = int.Parse(Console.ReadLine());


            for(int i=0; i<times; i++)
            {
                Console.WriteLine("{0} {0} {0} {0}", num);
                Console.WriteLine("{0}{0}{0}{0}", num);
            }
            Console.ReadLine();

        }
    }
}
