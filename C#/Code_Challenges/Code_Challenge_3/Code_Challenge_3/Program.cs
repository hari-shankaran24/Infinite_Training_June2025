using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Challenge_3
{
    class CricketTeam
    {
        public (int, int, double) PointsCalculation(int no_of_matches)
        {
            int[] scores = new int[no_of_matches];
            int sum = 0;

            Console.WriteLine("Enter the Scores: ");
            for (int i = 0; i < no_of_matches; i++)
            {
                Console.Write($"Score of the  match {i + 1}: ");
                scores[i] = int.Parse(Console.ReadLine());
                sum += scores[i];
            }

            double average = (double)sum / no_of_matches;

            return (no_of_matches, sum, average);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CricketTeam team = new CricketTeam();
            Console.WriteLine("Enter the number of matches:");
            int num_matches = int.Parse(Console.ReadLine());
            var result = team.PointsCalculation(num_matches);

            Console.WriteLine($"Number of Matches is : {result.Item1}");
            Console.WriteLine($"Sum of Scores is : {result.Item2}");
            Console.WriteLine($"Average of Scores iss: {result.Item3}");
            Console.ReadLine();
        }
    }
}
