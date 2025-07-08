using System;
using System.Collections.Generic;
using System.Linq;

namespace Code7
{
    class EmpData
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCity { get; set; }
        public double EmpSalary { get; set; }

        public void Display()
        {
            Console.WriteLine($"ID: {EmpId}, Name: {EmpName}, City: {EmpCity}, Salary: {EmpSalary}");
        }
    }

    class Emp
    {
        static void Main()
        {
            List<EmpData> employees = new List<EmpData>();

            Console.Write("Enter number of employees: ");
            int count = int.Parse(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\nEnter details for Employee {i + 1}:");

                Console.Write("EmpId: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("EmpName: ");
                string name = Console.ReadLine();

                Console.Write("EmpCity: ");
                string city = Console.ReadLine();

                Console.Write("EmpSalary: ");
                double salary = double.Parse(Console.ReadLine());

                employees.Add(new EmpData
                {
                    EmpId = id,
                    EmpName = name,
                    EmpCity = city,
                    EmpSalary = salary
                });
            }

            Console.WriteLine("\nAll Employees:");
            foreach (var emp in employees)
                emp.Display();

            Console.WriteLine("\nEmployees with Salary > 45000:");
            foreach (var emp in employees.Where(e => e.EmpSalary > 45000))
                emp.Display();

            Console.WriteLine("\nEmployees from Bangalore:");
            foreach (var emp in employees.Where(e => e.EmpCity.ToLower() == "bangalore"))
                emp.Display();

            Console.WriteLine("\nEmployees sorted by Name:");
            foreach (var emp in employees.OrderBy(e => e.EmpName))
                emp.Display();

            Console.ReadLine();
        }
    }
}
