using System;
using System.Collections.Generic;

public class Employee
{
    public int ID;
    public string Name;
    public string Department;
    public double Salary;
}

class Program
{
    static List<Employee> employees = new List<Employee>();

    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\nEmployee Management Menu ");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View All Employees");
            Console.WriteLine("3. Search by ID");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit");
            Console.Write("Enter any one of the above option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1: AddEmployee(); break;
                case 2: ViewEmployees(); break;
                case 3: SearchEmployee(); break;
                case 4: UpdateEmployee(); break;
                case 5: DeleteEmployee(); break;
                case 6: running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }

    static void AddEmployee()
    {
        Employee e = new Employee();
        Console.WriteLine("ID: ");
        e.ID = int.Parse(Console.ReadLine());
        Console.WriteLine("Name: ");
        e.Name = Console.ReadLine();
        Console.WriteLine("Department: ");
        e.Department = Console.ReadLine();
        Console.WriteLine("Salary: ");
        e.Salary = Convert.ToDouble(Console.ReadLine());
        employees.Add(e);
        Console.WriteLine("Employee added.");
    }

    static void ViewEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        foreach (var emp in employees)
        {
            Console.WriteLine($"ID: {emp.ID}, Name: {emp.Name}, Dept: {emp.Department}, Salary: {emp.Salary}");
        }
    }

    static void SearchEmployee()
    {
        Console.Write("Enter ID: ");
        int id = int.Parse(Console.ReadLine());
        var emp = employees.Find(e => e.ID == id);
        if (emp != null)
            Console.WriteLine($"ID: {emp.ID}, Name: {emp.Name}, Dept: {emp.Department}, Salary: {emp.Salary}");
        else
            Console.WriteLine("Employee not found.");
    }

    static void UpdateEmployee()
    {
        Console.Write("Enter ID to update: ");
        int id = int.Parse(Console.ReadLine());
        var emp = employees.Find(e => e.ID == id);
        if (emp != null)
        {
            Console.Write("New Name: ");
            emp.Name = Console.ReadLine();
            Console.Write("New Dept: ");
            emp.Department = Console.ReadLine();
            Console.Write("New Salary: ");
            emp.Salary = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Employee updated.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    static void DeleteEmployee()
    {
        Console.Write("Enter ID to delete: ");
        int id = int.Parse(Console.ReadLine());
        var emp = employees.Find(e => e.ID == id);
        if (emp != null)
        {
            employees.Remove(emp);
            Console.WriteLine("Employee deleted.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }
}
