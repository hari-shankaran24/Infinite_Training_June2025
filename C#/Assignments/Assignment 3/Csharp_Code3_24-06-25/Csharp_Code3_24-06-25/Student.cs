using System;


namespace Csharp_Code3_24_06_25
{//-----------------------------Student Marks----------------------------------------
    class Student
    {
        protected int RollNo;
        protected string Name;
        protected string Branch;
        protected char Section;
        protected int Semester;
        int[] marks = new int[5];
        string result;
        
        public Student(int roll, string name, string branch, char sec, int sem)
        {
            RollNo = roll;
            Name = name;
            Branch = branch;
            Section = sec;
            Semester = sem;
        }

        public void GetMarks()
        {
            Console.WriteLine("Enter the 5 subjects marks: ");
            for(int i=0; i<5; i++)
            {
                Console.WriteLine($"Enter subject {i + 1} marks: ");
                marks[i] = int.Parse(Console.ReadLine());
            }

        }
        
        public void DisplayResult()
        {
            int total = 0;
            bool fail = false;

            foreach (int j in marks)
            {
                if (j < 35)
                {
                    fail = true;
                    
                }
                total = total + j;
            }
                double average = total/5.0;

                if (fail)
                {
                    result = "Failed in One or more subjects";
                }
                else if (average < 50)
                {
                    result = "Failed, Average is less than 50";
                }
                else
                {
                    result = "Passed";
                }

                Console.WriteLine($"\n Result is {result}");

        }

        
        public void DisplayData()
        {
            Console.WriteLine("\n--- Student Details ---");
            Console.WriteLine($"Roll No   : {RollNo}");
            Console.WriteLine($"Name      : {Name}");
            Console.WriteLine($"Branch    : {Branch}");
            Console.WriteLine($"Section   : {Section}");
            Console.WriteLine($"Semester  : {Semester}");
            Console.WriteLine("Marks      :" + string.Join(", ", marks));
        }
    }

    class Student_Marks
    {
        static void Main()
        {
            Console.Write("Enter Roll No: ");
            int RollNo = int.Parse(Console.ReadLine());

            Console.Write("Enter Name: ");
            string Name = Console.ReadLine();

            Console.Write("Enter Branch: ");
            string Branch = Console.ReadLine();

            Console.Write("Enter Section: ");
            char Section = char.Parse(Console.ReadLine());

            Console.Write("Enter Semester ");
            int Semester = int.Parse(Console.ReadLine());

           

            Student student = new Student(RollNo, Name, Branch, Section, Semester);
            student.GetMarks();
            student.DisplayResult();
            student.DisplayData();

            Console.ReadLine();

        }
    }
}
