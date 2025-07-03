using System;


/*Question 1 1. Create an Abstract class Student with  Name, StudentId, Grade as members and also an abstract method Boolean Ispassed(grade) which takes grade as an input and checks whether student passed the course or not. 
 * Create 2 Sub classes Undergraduate and Graduate that inherits all members of the student and overrides Ispassed(grade) method or the UnderGrad class, if the grade is above 70.0, then isPassed returns true, otherwise it returns false. For the Grad class, if the grade is above 80.0, then isPassed returns true, otherwise returns false.
Test the above by creating appropriate objects */

namespace Code_Challenge_2
{
    public abstract class Student
    {
        public string Name { get; set; }
        public int StudentID { get; set; }
        public double Grade { get; set; }
        public abstract bool IsPassed(double grade);
    }

    public class UnderGraduate : Student
    {
        public override bool IsPassed(double grade)
        {
            return grade > 70.0;
        }
    }

    public class Graduate : Student
    {
        public override bool IsPassed(double grade)
        {
            return grade > 80.0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the Student type (Undergraduate or Graduate):");
            string type = Console.ReadLine();

            Console.WriteLine("Enter the Student Name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter the Student ID:");
            int id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Student Grade:");
            double grade = double.Parse(Console.ReadLine());

            Student student;
            string studentType;

            if (type.ToLower() == "undergraduate")
            {
                student = new UnderGraduate();
                studentType = "Undergraduate";
            }
            else if (type.ToLower() == "graduate")
            {
                student = new Graduate();
                studentType = "Graduate";
            }
            else
            {
                Console.WriteLine("Invalid Student Type");
                return;
            }

            student.Name = name;
            student.StudentID = id;
            student.Grade = grade;

            Console.WriteLine($"\nStudent Type: {studentType}");
            Console.WriteLine($"Student Name: {student.Name}");
            Console.WriteLine($"Student ID: {student.StudentID}");
            Console.WriteLine($"Grade: {student.Grade}");
            Console.WriteLine($"Passed: {student.IsPassed(student.Grade)}");

            Console.ReadLine();
        }
    }
}
