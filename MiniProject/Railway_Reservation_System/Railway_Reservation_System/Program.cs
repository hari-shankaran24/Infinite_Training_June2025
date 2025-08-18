using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Railway_Reservation_System
{
    class mark
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("                 Railway Reservation System                   ");
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("1. Sign Up");
                Console.WriteLine("2. Sign In");

                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1": // Sign Up
                        {
                            var newCust = Register.RegisterUser();
                            if (!string.IsNullOrEmpty(newCust))
                            {
                                Console.WriteLine($"\nRegistration successful.\nYour Customer ID: {newCust}");
                                SignUpMenu(); // Call signup-specific menu
                            }
                            break;
                        }
                    case "2": // Sign In
                        {
                            var custId = Login.LoginUser();
                            if (!string.IsNullOrEmpty(custId))
                            {
                                Console.WriteLine($"\nLogin successful. CustID: {custId}");
                                SignInMenu(); // Call signin-specific menu
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid credentials or account deleted.");
                            }
                            break;
                        }
                    case "3": // Exit
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                Console.ResetColor(); // Reset to default color
            }
        }

        static void SignUpMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n ------------------------------------------------------------");
                Console.WriteLine("                           Sign Up Menu                        ");
                Console.WriteLine(" ------------------------------------------------------------");
                Console.WriteLine("1. Search Trains");
                Console.WriteLine("2. Book Tickets");
                Console.WriteLine("3. Cancel Full Booking");
                Console.WriteLine("4. Cancel Selected Passengers");
                Console.WriteLine("5. Ticket Report");
                Console.WriteLine("6. Soft Delete User");
                Console.WriteLine("7. Download Ticket");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Search.SearchTrains(); break;
                    case "2": Book.BookTickets(); break;
                    case "3": CancelFull.CancelFullBooking(); break;
                    case "4": CancelSel.CancelSelectedPassengers(); break;
                    case "5": Report.TicketReport(); break;
                    case "6": SoftDeleteUser(); break;
                    case "7": DownloadTicket(); break;
                    case "8": back = true; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            Console.ResetColor(); // Reset to default color
        }

        static void SignInMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n ------------------------------------------------------------");
                Console.WriteLine("                           Sign In Menu                        ");
                Console.WriteLine(" ------------------------------------------------------------");
                Console.WriteLine("1. Search Trains");
                Console.WriteLine("2. View Ticket");
                Console.WriteLine("3. Cancel Full Booking");
                Console.WriteLine("4. Cancel Selected Passengers");
                Console.WriteLine("5. Ticket Report");
                Console.WriteLine("6. Soft Delete User");
                Console.WriteLine("7. Download Ticket");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Search.SearchTrains(); break;
                    case "2": Report.TicketReport(); break;
                    case "3": CancelFull.CancelFullBooking(); break;
                    case "4": CancelSel.CancelSelectedPassengers(); break;
                    case "5": Report.TicketReport(); break;
                    case "6": SoftDeleteUser(); break;
                    case "7": DownloadTicket(); break;
                    case "8": back = true; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
                Console.ResetColor(); // Reset to default color
            }
        }

        // -------------------------
        // Download Ticket 
        // -------------------------

        static void DownloadTicket()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                         Download Ticket                      ");
            Console.WriteLine("--------------------------------------------------------------");
            Console.Write("Booking ID: ");
            string bookingId = Console.ReadLine();

            try
            {
                string ticketText = GetTicketDetails(bookingId);
                if (string.IsNullOrEmpty(ticketText))
                {
                    Console.WriteLine("No ticket found for the provided Booking ID.");
                    return;
                }

                SaveTicketToFile(ticketText, bookingId);
                Console.WriteLine($"Ticket Downloaded: Ticket_{bookingId}.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to download ticket: " + ex.Message);
            }
        }

        static string GetTicketDetails(string bookingId)
        {
            StringBuilder sb = new StringBuilder();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TicketReport", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;

                        bool first = true;
                        while (reader.Read())
                        {
                            if (first)
                            {
                                sb.AppendLine("-------------------- TICKET --------------------");
                                sb.AppendLine($"BookingID   : {reader["BookingID"]}");
                                sb.AppendLine($"BookingDate : {reader["BookingDate"]}");
                                sb.AppendLine($"TotalCost   : {reader["TotalCost"]}");
                                sb.AppendLine($"TravelDate  : {reader["TravelDate"]}");
                                sb.AppendLine($"Train       : {reader["TrainName"]}");
                                sb.AppendLine($"From        : {reader["Source"]}");
                                sb.AppendLine($"To          : {reader["Destination"]}");
                                sb.AppendLine($"Departure   : {reader["DepartureDateTime"]}");
                                sb.AppendLine("");
                                sb.AppendLine("Passengers:");
                                sb.AppendLine("PassengerID | Name | Age | Gender | Berth | Status");
                                first = false;
                            }

                            bool isCancelled = reader["IsCancelled"] != DBNull.Value && (bool)reader["IsCancelled"];
                            string status = isCancelled ? "Cancelled" : "Confirmed";

                            sb.AppendLine($"{reader["PassengerID"]} | {reader["PassengerName"]} | {reader["Age"]} | {reader["Gender"]} | {reader["BerthAllotment"]} | {status}");
                        }
                    }
                }
            }

            sb.AppendLine("-----------------------------------------------");
            return sb.ToString();
        }

        static void SaveTicketToFile(string ticketText, string bookingId)
        {
            string downloadFolder = @"C:\Users\harish\Desktop\Workspace\MiniProject\Railway_Reservation_System\Tickets";

            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }

            string filePath = Path.Combine(downloadFolder, $"Ticket_{bookingId}.txt");

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(ticketText);
                sw.Flush();
            }
        }

        // -------------------------
        // SoftDeleteUser
        // -------------------------
        static void SoftDeleteUser()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                         Delete User                          ");
            Console.WriteLine("--------------------------------------------------------------");
            Console.Write("Customer ID: "); string custId = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_SoftDeleteUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustID", custId);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("User marked as deleted.");
                }
            }
        }
    }

 


}
