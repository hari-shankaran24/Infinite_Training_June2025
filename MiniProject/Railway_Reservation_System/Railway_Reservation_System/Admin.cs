using System;
using System.Data;
using System.Data.SqlClient;

namespace Railway_Reservation_System
{
    public class Admin
    {
        private static readonly string connectionString =
            "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";

        // -------------------------        
        // Admin Register
        // -------------------------
        public static void RegisterAdmin()
        {
            Console.WriteLine("---------- Admin Registration ----------");
            Console.Write("Name: "); string name = Console.ReadLine()?.Trim();
            Console.Write("Phone: "); string phone = Console.ReadLine()?.Trim();
            Console.Write("Email: "); string email = Console.ReadLine()?.Trim();
            Console.Write("Username: "); string username = Console.ReadLine()?.Trim();
            Console.Write("Password: "); string password = Console.ReadLine()?.Trim();
            Console.Write("Address: "); string address = Console.ReadLine()?.Trim();

            // Basic Base64 "hash" - replace with secure hashing in production
            string passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AdminRegister", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@Address", address);

                var outParam = new SqlParameter("@NewAdminID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outParam);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"Admin Registered Successfully. Admin ID: {outParam.Value}");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Registration failed: " + ex.Message);
                }
            }
        }

        // -------------------------
        // Admin Login - returns AdminID or null
        // -------------------------
        public static int? LoginAdmin()
        {
            Console.WriteLine("---------- Admin Login ----------");
            Console.Write("Username: "); string username = Console.ReadLine()?.Trim();
            Console.Write("Password: "); string password = Console.ReadLine()?.Trim();

            string passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AdminLogin", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                con.Open();
                var result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int adminId))
                {
                    Console.WriteLine("Login Successful.");
                    return adminId;
                }
                Console.WriteLine("Login Failed. Invalid credentials.");
                return null;
            }
        }

        // -------------------------
        // View All Bookings
        // -------------------------
        public static void ViewAllBookings()
        {
            Console.WriteLine("---------- All Bookings ----------");
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AdminViewAllBookings", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No bookings found.");
                        return;
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine($"BookingID: {reader["BookingID"]}, CustID: {reader["CustID"]}, TrainNo: {reader["TrainNo"]}, SeatsBooked: {reader["SeatsBooked"]}, TotalCost: {reader["TotalCost"]}, BookingDate: {reader["BookingDate"]}, IsCancelled: {reader["IsCancelled"]}");
                    }
                }
            }
        }

        // -------------------------
        // View All Cancellations
        // -------------------------
        public static void ViewAllCancellations()
        {
            Console.WriteLine("---------- All Cancellations ----------");
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AdminViewAllCancellations", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No cancellations found.");
                        return;
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine($"BookingID: {reader["BookingID"]}, CustID: {reader["CustID"]}, TrainNo: {reader["TrainNo"]}, SeatsBooked: {reader["SeatsBooked"]}, TotalCost: {reader["TotalCost"]}, BookingDate: {reader["BookingDate"]}");
                    }
                }
            }
        }

        // -------------------------
        // View All Trains
        // -------------------------
        public static void ViewAllTrains()
        {
            Console.WriteLine("---------- All Trains ----------");
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AdminViewAllTrains", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No trains found.");
                        return;
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine($"TrainNo: {reader["TrainNo"]}, TrainName: {reader["TrainName"]}, Source: {reader["Source"]}, Destination: {reader["Destination"]}, ClassType: {reader["ClassType"]}, SeatsAvailable: {reader["SeatsAvailable"]}, Price: {reader["Price"]}, Departure: {reader["DepartureDateTime"]}");
                    }
                }
            }
        }

        // -------------------------
        // Add New Train
        // -------------------------
        public static void AddNewTrain()
        {
            Console.WriteLine("---------- Add New Train ----------");
            Console.Write("Train No: "); string trainNo = Console.ReadLine()?.Trim();
            Console.Write("Train Name: "); string trainName = Console.ReadLine()?.Trim();
            Console.Write("Source: "); string source = Console.ReadLine()?.Trim();
            Console.Write("Destination: "); string destination = Console.ReadLine()?.Trim();
            Console.Write("Class Type: "); string classType = Console.ReadLine()?.Trim();

            Console.Write("Seats Available: ");
            if (!int.TryParse(Console.ReadLine(), out int seats) || seats < 0)
            {
                Console.WriteLine("Invalid seats."); return;
            }

            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("Invalid price."); return;
            }

            Console.Write("Departure DateTime (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime departureDateTime))
            {
                Console.WriteLine("Invalid date/time."); return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AdminAddTrain", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrainNo", trainNo);
                cmd.Parameters.AddWithValue("@TrainName", trainName);
                cmd.Parameters.AddWithValue("@Source", source);
                cmd.Parameters.AddWithValue("@Destination", destination);
                cmd.Parameters.AddWithValue("@ClassType", classType);
                cmd.Parameters.AddWithValue("@SeatsAvailable", seats);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@DepartureDateTime", departureDateTime);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Train added successfully.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Failed to add train: " + ex.Message);
                }
            }
        }

        // -------------------------
        // Update Train Route
        // -------------------------
        public static void UpdateTrainRoute()
        {
            Console.WriteLine("---------- Update Train Route ----------");
            Console.Write("Train No: "); string trainNo = Console.ReadLine()?.Trim();
            Console.Write("New Source: "); string newSource = Console.ReadLine()?.Trim();
            Console.Write("New Destination: "); string newDestination = Console.ReadLine()?.Trim();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AdminUpdateTrainRoute", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrainNo", trainNo);
                cmd.Parameters.AddWithValue("@NewSource", newSource);
                cmd.Parameters.AddWithValue("@NewDestination", newDestination);

                con.Open();
                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Train route updated successfully." : "Train not found.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Update failed: " + ex.Message);
                }
            }
        }

        // -------------------------
        // Optional: View All Ticket Reports
        // -------------------------
        public static void ViewAllTicketReports()
        {
            Console.WriteLine("---------- All Ticket Reports ----------");
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AdminViewAllTicketReports", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No ticket reports found.");
                        return;
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine($"BookingID: {reader["BookingID"]}, PassengerID: {reader["PassengerID"]}, PassengerName: {reader["PassengerName"]}, TrainName: {reader["TrainName"]}, Source: {reader["Source"]}, Destination: {reader["Destination"]}, Departure: {reader["DepartureDateTime"]}, TotalCost: {reader["TotalCost"]}, BookingDate: {reader["BookingDate"]}, IsCancelled: {reader["IsCancelled"]}");
                    }
                }
            }
        }
    }
}
