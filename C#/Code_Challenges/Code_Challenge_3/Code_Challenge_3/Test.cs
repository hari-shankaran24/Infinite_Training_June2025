using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Challenge_3
{
    class Box
    {
        public double Length { get; set; }
        public double Breadth { get; set; }

        public Box(double len, double brth)
        {
            Length = len;
            Breadth = brth;

        }

        public static Box Add(Box box1, Box box2)
        {
            double newLen = box1.Length + box2.Length;
            double newBrth = box1.Breadth + box2.Breadth;

            return new Box(newLen, newBrth);
        }
        public void Display()
        {
            Console.WriteLine($"Length: {Length}, Breadth: {Breadth}");
        }


    }

    class Test
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the dimensions for Box 1:");
            Console.Write("Length: ");
            double length1 = double.Parse(Console.ReadLine());
            Console.Write("Breadth: ");
            double breadth1 = double.Parse(Console.ReadLine());

            Box box1 = new Box(length1, breadth1);

            Console.WriteLine("Enter the dimensions for Box 2:");
            Console.Write("Length: ");
            double length2 = double.Parse(Console.ReadLine());
            Console.Write("Breadth: ");
            double breadth2 = double.Parse(Console.ReadLine());

            Box box2 = new Box(length2, breadth2);

            Box box3 = Box.Add(box1, box2);

            Console.WriteLine("Dimensions of the third Box is :");
            box3.Display();
            Console.ReadLine();
        }
    }

}
