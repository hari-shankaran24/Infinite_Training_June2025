using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeApp
{
    class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime DOB { get; set; }  // Date of Birth
        public DateTime DOJ { get; set; }  // Date of Joining
        public string City { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create and populate the list of employees
            List<Employee> empList = new List<Employee>()
            {
                new Employee { EmployeeID = 1001, FirstName = "Malcolm", LastName = "Daruwalla", Title = "Manager", DOB = DateTime.ParseExact("16/11/1984", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("08/06/2011", "dd/MM/yyyy", null), City = "Mumbai" },
                new Employee { EmployeeID = 1002, FirstName = "Asdin", LastName = "Dhalla", Title = "AsstManager", DOB = DateTime.ParseExact("20/08/1984", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("07/07/2012", "dd/MM/yyyy", null), City = "Mumbai" },
                new Employee { EmployeeID = 1003, FirstName = "Madhavi", LastName = "Oza", Title = "Consultant", DOB = DateTime.ParseExact("14/11/1987", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("12/04/2015", "dd/MM/yyyy", null), City = "Pune" },
                new Employee { EmployeeID = 1004, FirstName = "Saba", LastName = "Shaikh", Title = "SE", DOB = DateTime.ParseExact("03/06/1990", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("02/02/2016", "dd/MM/yyyy", null), City = "Pune" },
                new Employee { EmployeeID = 1005, FirstName = "Nazia", LastName = "Shaikh", Title = "SE", DOB = DateTime.ParseExact("08/03/1991", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("02/02/2016", "dd/MM/yyyy", null), City = "Mumbai" },
                new Employee { EmployeeID = 1006, FirstName = "Amit", LastName = "Pathak", Title = "Consultant", DOB = DateTime.ParseExact("07/11/1989", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("08/08/2014", "dd/MM/yyyy", null), City = "Chennai" },
                new Employee { EmployeeID = 1007, FirstName = "Vijay", LastName = "Natrajan", Title = "Consultant", DOB = DateTime.ParseExact("02/12/1989", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("01/06/2015", "dd/MM/yyyy", null), City = "Mumbai" },
                new Employee { EmployeeID = 1008, FirstName = "Rahul", LastName = "Dubey", Title = "Associate", DOB = DateTime.ParseExact("11/11/1993", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("06/11/2014", "dd/MM/yyyy", null), City = "Chennai" },
                new Employee { EmployeeID = 1009, FirstName = "Suresh", LastName = "Mistry", Title = "Associate", DOB = DateTime.ParseExact("12/08/1992", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("03/12/2014", "dd/MM/yyyy", null), City = "Chennai" },
                new Employee { EmployeeID = 1010, FirstName = "Sumit", LastName = "Shah", Title = "Manager", DOB = DateTime.ParseExact("12/04/1991", "dd/MM/yyyy", null), DOJ = DateTime.ParseExact("02/01/2016", "dd/MM/yyyy", null), City = "Pune" }
            };

            // 1. Employees who joined before 1 Jan 2015
            Console.WriteLine("Employees who joined before 1/1/2015:");
            var joinedbfor = empList.Where(e => e.DOJ < new DateTime(2015, 1, 1));
            DisplayEmployee(joinedbfor);
            Console.WriteLine();

            // 2. Employees born after 1 Jan 1990
            Console.WriteLine("Employees whose DOB is after 1/1/1990:");
            var dob = empList.Where(e => e.DOB > new DateTime(1990, 1, 1));
            DisplayEmployee(dob);
            Console.WriteLine();

            // 3. Employees with Title Consultant or Associate
            Console.WriteLine("Employees who are Consultant or Associate:");
            var ConsandAssc = empList.Where(e => e.Title == "Consultant" || e.Title == "Associate");
            DisplayEmployee(ConsandAssc);
            Console.WriteLine();

            // 4. Total number of employees
            Console.WriteLine("Total number of employees:");
            Console.WriteLine(empList.Count);
            Console.WriteLine();

            // 5. Total employees from Chennai
            Console.WriteLine("Total employees belonging to Chennai:");
            Console.WriteLine(empList.Count(e => e.City == "Chennai"));
            Console.WriteLine();

            // 6. Highest employee ID
            Console.WriteLine("Highest employee ID:");
            Console.WriteLine(empList.Max(e => e.EmployeeID));
            Console.WriteLine();

            // 7. Total employees joined after 1 Jan 2015
            Console.WriteLine("Total employees joined after 1/1/2015:");
            Console.WriteLine(empList.Count(e => e.DOJ > new DateTime(2015, 1, 1)));
            Console.WriteLine();

            // 8. Total employees whose designation is NOT Associate
            Console.WriteLine("Total employees who is not Associate:");
            Console.WriteLine(empList.Count(e => !string.Equals(e.Title, "Associate", StringComparison.OrdinalIgnoreCase)));
            Console.WriteLine();

            // 9. Total employees grouped by City
            Console.WriteLine("Total number of employees grouped by City:");
            var empcity = empList.GroupBy(e => e.City)
                                        .Select(g => new { City = g.Key, Count = g.Count() });
            foreach (var group in empcity)
            {
                Console.WriteLine($"{group.City}: {group.Count}");
            }
            Console.WriteLine();

            // 10. Total employees grouped by City and Title
            Console.WriteLine("Total number of employees grouped by City and title:");
            var empCitTitle = empList.GroupBy(e => new { e.City, e.Title })
                                             .Select(g => new { g.Key.City, g.Key.Title, Count = g.Count() });
            foreach (var group in empCitTitle)
            {
                Console.WriteLine($"{group.City} - {group.Title}: {group.Count}");
            }
            Console.WriteLine();

            // 11. Youngest employee(s) and their count
            Console.WriteLine("Youngest employee(s) details and count:");
            var maxDOB = empList.Max(e => e.DOB);
            var youngestEmp = empList.Where(e => e.DOB == maxDOB);
            Console.WriteLine($"Date of Birth: {maxDOB:dd/MM/yyyy}");
            Console.WriteLine($"Count: {youngestEmp.Count()}");
            DisplayEmployee(youngestEmp);

            Console.ReadLine();
        }

        public static void DisplayEmployee(IEnumerable<Employee> employeeList)
        {
            foreach (var e in employeeList)
            {
                Console.WriteLine($"Employee Id: {e.EmployeeID}, First Name: {e.FirstName}, Last Name: {e.LastName}, Title: {e.Title}, DOB: {e.DOB:dd-MM-yyyy}, DOJ: {e.DOJ:dd-MM-yyyy}, City:{e.City}");
            }
        }
    }
}
