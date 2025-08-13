using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;


namespace Railway_Reservation_System
{
    public class Book
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";

        // -------------------------
        // Book Tickets 
        // -------------------------
        public static void BookTickets()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                        Book Tickets                          ");
            Console.WriteLine("--------------------------------------------------------------");

            // Ask if existing customer or new
            Console.Write("Are you an existing customer? (Y/N): ");
            string existing = (Console.ReadLine() ?? "").Trim().ToUpper();

            string custId = null;
            string name = null, phone = null, email = null, username = null, password = null, address = null;

            if (existing == "Y")
            {
                // Login existing customer
                Console.Write("Username: "); string u = (Console.ReadLine() ?? "").Trim();
                Console.Write("Password: "); string p = (Console.ReadLine() ?? "").Trim();
                var id = Login.GetCustIdByCredentials(u, p);
                if (id == null)
                {
                    Console.WriteLine("Login failed. Aborting booking.");
                    return;
                }
                custId = id;
                Console.WriteLine($"Using existing customer: {custId}");
            }
            else
            {
                // Collect details but DO NOT register yet
                Console.WriteLine("\n Enter customer details (these will be used to create the customer account AFTER payment confirmation):");
                Console.Write("Name: "); name = (Console.ReadLine() ?? "").Trim();
                Console.Write("Phone: "); phone = (Console.ReadLine() ?? "").Trim();
                Console.Write("Email: "); email = (Console.ReadLine() ?? "").Trim();
                Console.Write("Username: "); username = (Console.ReadLine() ?? "").Trim();
                Console.Write("Password: "); password = (Console.ReadLine() ?? "").Trim();
                Console.Write("Address: "); address = (Console.ReadLine() ?? "").Trim();
            }

            // Train & booking details
            Console.Write("Train No: "); string trainNo = (Console.ReadLine() ?? "").Trim();
            Console.Write("Class Type: "); string classType = (Console.ReadLine() ?? "").Trim();
            Console.Write("Seats to Book: ");
            if (!int.TryParse(Console.ReadLine(), out int seats) || seats <= 0) { Console.WriteLine("Invalid seats."); return; }

            // Validate travel date (not in the past)
            DateTime travelDate;
            while (true)
            {
                Console.Write("Travel Date (yyyy-mm-dd): ");
                string dateInput = Console.ReadLine() ?? "";
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

                break; // Valid date
            }

            // Fetch price per seat and check seats exist
            decimal pricePerSeat;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT SeatsAvailable, Price FROM TrainDetails WHERE TrainNo=@TrainNo AND ClassType=@ClassType", con))
                {
                    cmd.Parameters.AddWithValue("@TrainNo", trainNo);
                    cmd.Parameters.AddWithValue("@ClassType", classType);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.Read())
                        {
                            Console.WriteLine("Train/Class not found.");
                            return;
                        }
                        int seatsAvailable = Convert.ToInt32(rdr["SeatsAvailable"]);
                        pricePerSeat = Convert.ToDecimal(rdr["Price"]);
                        if (seats > seatsAvailable)
                        {
                            Console.WriteLine($"Not enough seats available, Added to Waiting List. Available: {seatsAvailable}");
                            return;
                        }
                    }
                }
            }

            // Collect passenger details (we will insert them only AFTER booking is done)
            var passengers = new (string Name, int Age, string Gender, string Berth)[seats];
            for (int i = 0; i < seats; i++)
            {
                Console.WriteLine($"\nEnter details for Passenger {i + 1}:");
                Console.Write("Name: "); string pname = (Console.ReadLine() ?? "").Trim();
                Console.Write("Age: "); if (!int.TryParse(Console.ReadLine(), out int age)) { Console.WriteLine("Invalid age."); return; }
                Console.Write("Gender: "); string gender = (Console.ReadLine() ?? "").Trim();
                Console.Write("Berth Allotment: "); string berth = (Console.ReadLine() ?? "").Trim();
                passengers[i] = (pname, age, gender, berth);
            }

            decimal totalCost = pricePerSeat * seats;
            Console.WriteLine($"\nTotal Cost: {totalCost:0.00}");

            // Simulate payment
            Console.Write("Proceed to payment? (Y/N): ");
            if ((Console.ReadLine() ?? "").Trim().ToUpper() != "Y")
            {
                Console.WriteLine("Booking aborted before payment. No customer or passenger records were saved.");
                return;
            }

            // Payment confirmed -> now register customer if needed (only now we persist customer)
            if (string.IsNullOrEmpty(custId))
            {
                try
                {
                    custId = Register.RegisterUser(name, phone, email, username, password, address);
                    Console.WriteLine($"Customer registered: {custId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to register customer after payment: " + ex.Message);
                    return;
                }
            }

            // Generate BookingID
            string bookingId;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Get next BookingID
                using (SqlCommand cmd = new SqlCommand("GetNextBookingID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var scalar = cmd.ExecuteScalar();
                    if (scalar == null)
                    {
                        Console.WriteLine("Failed to generate Booking ID.");
                        return;
                    }
                    bookingId = scalar.ToString();
                }

                // Start transaction so booking + passenger inserts are atomic
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        // Call sp_BookTickets (this proc itself uses a transaction internally; calling within an outer transaction is safe)
                        using (SqlCommand cmdBook = new SqlCommand("sp_BookTickets", con, transaction))
                        {
                            cmdBook.CommandType = CommandType.StoredProcedure;
                            cmdBook.Parameters.AddWithValue("@BookingID", bookingId);
                            cmdBook.Parameters.AddWithValue("@CustID", custId);
                            cmdBook.Parameters.AddWithValue("@TrainNo", trainNo);
                            cmdBook.Parameters.AddWithValue("@ClassType", classType);
                            cmdBook.Parameters.AddWithValue("@SeatsBooked", seats);
                            cmdBook.Parameters.AddWithValue("@TotalCost", totalCost);
                            cmdBook.Parameters.AddWithValue("@TravelDate", travelDate.Date);
                            cmdBook.ExecuteNonQuery();
                        }

                        // Now insert passengers (booking exists)
                        for (int i = 0; i < seats; i++)
                        {
                            // Generate PassengerID
                            string passengerId;
                            using (SqlCommand cmdPid = new SqlCommand("GetNextPassengerID", con, transaction))
                            {
                                cmdPid.CommandType = CommandType.StoredProcedure;
                                var sc = cmdPid.ExecuteScalar();
                                if (sc == null) throw new ApplicationException("Failed to generate Passenger ID.");
                                passengerId = sc.ToString();
                            }

                            using (SqlCommand cmdIns = new SqlCommand("sp_InsertPassenger", con, transaction))
                            {
                                cmdIns.CommandType = CommandType.StoredProcedure;
                                cmdIns.Parameters.AddWithValue("@PassengerID", passengerId);
                                cmdIns.Parameters.AddWithValue("@BookingID", bookingId);
                                cmdIns.Parameters.AddWithValue("@PassengerName", passengers[i].Name);
                                cmdIns.Parameters.AddWithValue("@Age", passengers[i].Age);
                                cmdIns.Parameters.AddWithValue("@Gender", passengers[i].Gender);
                                cmdIns.Parameters.AddWithValue("@BerthAllotment", passengers[i].Berth);
                                cmdIns.ExecuteNonQuery();
                            }

                            Console.WriteLine($"Passenger inserted. Passenger ID: {passengerId}");
                        }

                        // All good -> commit transaction
                        transaction.Commit();
                        Console.WriteLine($"\nBooking successful. Your Booking ID is {bookingId}");
                    }
                    catch (SqlException ex)
                    {
                        try { transaction.Rollback(); } catch { /* ignore */ }
                        Console.WriteLine("Booking failed and rolled back: " + ex.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        try { transaction.Rollback(); } catch { /* ignore */ }
                        Console.WriteLine("Booking failed and rolled back: " + ex.Message);
                        return;
                    }
                } // end transaction
            } // end connection
        }

    }
}
