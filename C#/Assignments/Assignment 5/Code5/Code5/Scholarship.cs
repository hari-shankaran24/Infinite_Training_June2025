using System;

namespace Code5
{
    class ScholarshipNotEligible : ApplicationException
    {
        public int ProvidedMarks { get; set; }

        public ScholarshipNotEligible(string message, int marks) : base(message)
        {
            ProvidedMarks = marks;
        }
    }

    class Scholarship
    {
        public double Merit(int marks, double fees)
        {
            if (marks >= 70 && marks <= 80)
                return fees * 0.20;
            else if (marks > 80 && marks <= 90)
                return fees * 0.30;
            else if (marks > 90)
                return fees * 0.50;
            else
                throw new ScholarshipNotEligible("Student is not eligible for scholarship.", marks);
        }
    }

    class Test
    {
        static void Main(string[] args)
        {
            Scholarship s = new Scholarship();

            Console.Write("Enter marks: ");
            int marks = int.Parse(Console.ReadLine());

            Console.Write("Enter fees amount: ");
            double fees = double.Parse(Console.ReadLine());

            try
            {
                double scholarship = s.Merit(marks, fees);
                Console.WriteLine($"Scholarship Amount: {scholarship}");
            }
            catch (ScholarshipNotEligible ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Provided Marks: {ex.ProvidedMarks}");
            }

            Console.ReadLine();
        }
    }
}
