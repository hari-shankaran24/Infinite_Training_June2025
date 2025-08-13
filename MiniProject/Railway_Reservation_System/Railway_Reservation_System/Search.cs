using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;


namespace Railway_Reservation_System
{
    public class Search
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";


        // -------------------------
        // Search Trains 
        // -------------------------
        public static void SearchTrains()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                       Search Trains                          ");
            Console.WriteLine("--------------------------------------------------------------");
            Console.Write("Source: "); string source = Console.ReadLine();
            Console.Write("Destination: "); string destination = Console.ReadLine();

            string dateInput;
            DateTime travelDate = DateTime.MinValue; // default initialization

            while (true)
            {
                Console.Write("Travel Date (yyyy-mm-dd) : ");
                dateInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(dateInput))
                {
                    // Allow blank date (means user wants to see all trains)
                    break;
                }

                if (!DateTime.TryParse(dateInput, out travelDate))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    continue;
                }

                if (travelDate.Date < DateTime.Today)
                {
                    Console.WriteLine("Travel date cannot be in the past. Please enter a valid date.");
                    continue;
                }

                break; // Valid non-past date
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_SearchTrains", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Source", source);
                    cmd.Parameters.AddWithValue("@Destination", destination);

                    if (string.IsNullOrWhiteSpace(dateInput))
                        cmd.Parameters.AddWithValue("@TravelDate", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@TravelDate", travelDate);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("\nTrainNo | TrainName | ClassType | Seats | Price");
                        while (reader.Read())
                        {
                            // Removed DepartureDateTime from output
                            Console.WriteLine($"{reader["TrainNo"]} | {reader["TrainName"]} | {reader["ClassType"]} | {reader["SeatsAvailable"]} | {reader["Price"]}");
                        }
                    }
                }
            }
        }


    }
}
