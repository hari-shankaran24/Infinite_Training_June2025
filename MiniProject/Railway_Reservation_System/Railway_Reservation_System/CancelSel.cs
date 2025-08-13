using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;


namespace Railway_Reservation_System
{
    public class CancelSel
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";


        // -------------------------
        // Cancel Selected Passengers 
        // -------------------------
        public static void CancelSelectedPassengers()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                Cancel Selected Passengers                    ");
            Console.WriteLine("--------------------------------------------------------------");
            Console.Write("Booking ID: "); string bookingId = Console.ReadLine();
            Console.Write("Passenger IDs (comma-separated, e.g. P00001,P00002): ");
            string idsInput = Console.ReadLine();
            var tokens = idsInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
            if (tokens.Length == 0) { Console.WriteLine("No Passenger IDs provided."); return; }

            Console.Write("Confirm Cancellation? (Y/N): ");
            if (Console.ReadLine().Trim().ToUpper() != "Y") { Console.WriteLine("Cancellation aborted by user."); return; }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CancelPassengersByPassengerIDs", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PassengerIDs", string.Join(",", tokens));
                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine($"Tickets cancelled and 50% of the amount refunded: {string.Join(",", tokens)}");
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
