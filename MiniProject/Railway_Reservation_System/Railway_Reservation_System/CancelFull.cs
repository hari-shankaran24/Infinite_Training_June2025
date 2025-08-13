using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;


namespace Railway_Reservation_System
{
    public class CancelFull
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";

        // -------------------------
        // Cancel Full Booking 
        // -------------------------
        public static void CancelFullBooking()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                       Cancel Booking                         ");
            Console.WriteLine("--------------------------------------------------------------");
            Console.Write("Booking ID: "); string bookingId = Console.ReadLine();
            Console.Write("Registered Phone: "); string phone = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CancelFullBooking", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Full booking cancelled with 50% refund.");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Cancellation failed: " + ex.Message);
                    }
                }
            }
        }
    }
}
