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
                Console.WriteLine("3. Admin Register");
                Console.WriteLine("4. Admin Login");
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
                    case "3": // Admin Register
                        {
                            var adminId = AdminAuth.RegisterAdmin();
                            if (!string.IsNullOrEmpty(adminId))
                                Console.WriteLine($"\nAdmin Registered Successfully. Admin ID: {adminId}");
                            break;
                        }
                    case "4": // Admin Login
                        {
                            var adminId = AdminAuth.LoginAdmin();
                            if (!string.IsNullOrEmpty(adminId))
                            {
                                Console.WriteLine($"\nAdmin Login Successful. Admin ID: {adminId}");
                                AdminMenu();
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid admin credentials.");
                            }
                            break;
                        }
                    case "5": // Exit
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

        // Admin Menu after successful admin login
        static void AdminMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n ------------------------------------------------------------");
                Console.WriteLine("                           Admin Menu                          ");
                Console.WriteLine(" ------------------------------------------------------------");
                Console.WriteLine("1. View All Bookings");
                Console.WriteLine("2. View All Cancellations");
                Console.WriteLine("3. View All Trains");
                Console.WriteLine("4. Add New Train");
                Console.WriteLine("5. Update Train Route");
                Console.WriteLine("6. View All Ticket Reports");
                Console.WriteLine("7. Logout");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AdminOperations.ViewAllBookings(); break;
                    case "2": AdminOperations.ViewAllCancellations(); break;
                    case "3": AdminOperations.ViewAllTrains(); break;
                    case "4": AdminOperations.AddNewTrain(); break;
                    case "5": AdminOperations.UpdateTrainRoute(); break;
                    case "6": AdminOperations.ViewAllTicketReports(); break;
                    case "7": back = true; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            Console.ResetColor();
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
            string downloadFolder = @"C:\Users\harish\Desktop\Workspace\MiniProject\Tickets";

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

    // AdminAuth class for Admin Register & Login
    public static class AdminAuth
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";

        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static string RegisterAdmin()
        {
            Console.WriteLine("--------- Admin Registration ---------");

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = ReadPassword();

            Console.Write("Address: ");
            string address = Console.ReadLine();

            string passwordHash = HashPassword(password);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_AdminRegister", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@Address", address);

                        var outputIdParam = new SqlParameter("@AdminID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdParam);

                        cmd.ExecuteNonQuery();

                        int adminId = (int)outputIdParam.Value;
                        return adminId.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Registration failed: " + ex.Message);
                    return null;
                }
            }
        }

        public static string LoginAdmin()
        {
            Console.WriteLine("--------- Admin Login ---------");

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = ReadPassword();

            string passwordHash = HashPassword(password);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_AdminLogin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        var outputIdParam = new SqlParameter("@AdminID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdParam);

                        cmd.ExecuteNonQuery();

                        int adminId = (int)outputIdParam.Value;

                        return adminId > 0 ? adminId.ToString() : null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Login failed: " + ex.Message);
                    return null;
                }
            }
        }

        // Mask password input
        private static string ReadPassword()
        {
            StringBuilder pwd = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pwd.Append(key.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pwd.Length > 0)
                    {
                        pwd.Remove(pwd.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return pwd.ToString();
        }
    }

    // Placeholder AdminOperations class for admin menu actions (you'll implement these)
    public static class AdminOperations
    {
        public static void ViewAllBookings()
        {
            Console.WriteLine("[Admin] Viewing all bookings...");
            // Implement logic here
        }

        public static void ViewAllCancellations()
        {
            Console.WriteLine("[Admin] Viewing all cancellations...");
            // Implement logic here
        }

        public static void ViewAllTrains()
        {
            Console.WriteLine("[Admin] Viewing all trains...");
            // Implement logic here
        }

        public static void AddNewTrain()
        {
            Console.WriteLine("[Admin] Adding new train...");
            // Implement logic here
        }

        public static void UpdateTrainRoute()
        {
            Console.WriteLine("[Admin] Updating train route...");
            // Implement logic here
        }

        public static void ViewAllTicketReports()
        {
            Console.WriteLine("[Admin] Viewing all ticket reports...");
            // Implement logic here
        }
    }
}
