using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code7
{
    class Concession
    {
        public static void CalculateConcession(string name, int age, double totalFare)
        {
            Console.WriteLine($"\nPassenger: {name}");

            if (age <= 5)
            {
                Console.WriteLine("Little Champs - Free Ticket");
            }
            else if (age > 60)
            {
                double discountedFare = totalFare * 0.7;
                Console.WriteLine($"Senior Citizen - Fare after 30% concession: {discountedFare}");
            }
            else
            {
                Console.WriteLine($"Ticket Booked - Fare: {totalFare}");
            }
        }
    }
    class Travel
    {
        const double TotalFare = 1000;

        static void Main()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = int.Parse(Console.ReadLine());

            Concession.CalculateConcession(name, age, TotalFare);

            Console.ReadLine();
        }
    }
}
