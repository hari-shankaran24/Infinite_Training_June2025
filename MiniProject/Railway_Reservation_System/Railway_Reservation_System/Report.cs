using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;


namespace Railway_Reservation_System
{
    public class Report
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";
        // -------------------------
        // TicketReport 
        // -------------------------
        public static void TicketReport()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                         Ticket Report                        ");
            Console.WriteLine("--------------------------------------------------------------");
            Console.Write("Booking ID: ");
            string bookingId = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TicketReport", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n--- Ticket Details ---");
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No records found.");
                            return;
                        }

                        while (reader.Read())
                        {
                            string status = (reader["IsCancelled"] != DBNull.Value && (bool)reader["IsCancelled"])
                                            ? "Cancelled"
                                            : "Not Cancelled";

                            Console.WriteLine($"BookingID: {reader["BookingID"]} | Date: {reader["BookingDate"]} | Cost: {reader["TotalCost"]}");
                            Console.WriteLine($"Train: {reader["TrainName"]} | From: {reader["Source"]} | To: {reader["Destination"]} | Departure: {reader["DepartureDateTime"]}");
                            Console.WriteLine($"PassengerID: {reader["PassengerID"]} | Name: {reader["PassengerName"]} | Age: {reader["Age"]} | Gender: {reader["Gender"]} | Berth: {reader["BerthAllotment"]} | Ticket_Status: {status}");
                            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
                        }
                    }
                }
            }
        }
    }
}
