//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;

//namespace Railways_1
//{
//    class mark
//    {
//        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";

//        static void Main(string[] args)
//        {
//            bool exit = false;

//            while (!exit)
//            {
//                Console.WriteLine("--------------------------------------------------------------");
//                Console.WriteLine("\n --------------Railway Reservation System------------------ ");
//                Console.WriteLine("--------------------------------------------------------------");
//                Console.WriteLine("1. Sign Up");
//                Console.WriteLine("2. Sign In");
//                Console.WriteLine("3. Search Trains");
//                Console.WriteLine("4. Book Tickets");
//                Console.WriteLine("5. Cancel Full Booking");
//                Console.WriteLine("6. Cancel Selected Passengers");
//                Console.WriteLine("7. Ticket Report");
//                Console.WriteLine("8. Soft Delete User");
//                Console.WriteLine("9. Exit");
//                Console.Write("Choose an option: ");
//                string choice = Console.ReadLine();

//                switch (choice)
//                {
//                    case "1":
//                        {
//                            var newCust = RegisterUserInteractive();
//                            if (!string.IsNullOrEmpty(newCust))
//                                Console.WriteLine($"\nRegistration successful. \n Your Customer ID: {newCust}");
//                            break;
//                        }
//                    case "2":
//                        {
//                            var custId = LoginUserInteractive();
//                            if (!string.IsNullOrEmpty(custId))
//                                Console.WriteLine($"\n Login successful. CustID: {custId}");
//                            else
//                                Console.WriteLine("\n Invalid credentials or account deleted.");
//                            break;
//                        }
//                    case "3": SearchTrains(); break;
//                    case "4": BookTickets(); break;
//                    case "5": CancelFullBooking(); break;
//                    case "6": CancelSelectedPassengers(); break;
//                    case "7": TicketReport(); break;
//                    case "8": SoftDeleteUser(); break;
//                    case "9": exit = true; break;
//                    default: Console.WriteLine("Invalid choice."); break;
//                }
//            }
//        }

//        // -------------------------
//        // Registration (interactive, returns new CustID or null)
//        // -------------------------
//        static string RegisterUserInteractive()
//        {
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.WriteLine("\n--- Register ---");
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.Write("Name: "); string name = (Console.ReadLine() ?? "").Trim();
//            Console.Write("Phone: "); string phone = (Console.ReadLine() ?? "").Trim();
//            Console.Write("Email: "); string email = (Console.ReadLine() ?? "").Trim();
//            Console.Write("Username: "); string username = (Console.ReadLine() ?? "").Trim();
//            Console.Write("Password: "); string password = (Console.ReadLine() ?? "").Trim();
//            Console.Write("Address: "); string address = (Console.ReadLine() ?? "").Trim();

//            try
//            {
//                return RegisterUser(name, phone, email, username, password, address);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Registration failed: " + ex.Message);
//                return null;
//            }
//        }

//        // Calls stored procedure sp_RegisterUser and returns new CustID (or throws)
//        static string RegisterUser(string name, string phone, string email, string username, string password, string address)
//        {
//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();

//                // First check if username already exists to provide a friendly message
//                using (SqlCommand chk = new SqlCommand("SELECT CustID FROM Customer WHERE Username = @Username AND IsDeleted = 0", con))
//                {
//                    chk.Parameters.AddWithValue("@Username", username);
//                    var exist = chk.ExecuteScalar();
//                    if (exist != null)
//                        throw new ApplicationException("\n Username already exists. Choose a different username.");
//                }

//                using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@CustName", name);
//                    cmd.Parameters.AddWithValue("@Phone", phone);
//                    cmd.Parameters.AddWithValue("@Email", email);
//                    cmd.Parameters.AddWithValue("@Username", username);
//                    cmd.Parameters.AddWithValue("@Password", password);
//                    cmd.Parameters.AddWithValue("@Address", address);

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            return reader["NewCustID"].ToString();
//                        }
//                        else
//                        {
//                            throw new ApplicationException("\n Unknown error during registration.");
//                        }
//                    }
//                }
//            }
//        }

//        // -------------------------
//        // Login (interactive) - returns CustID or null
//        // -------------------------
//        static string LoginUserInteractive()
//        {
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.WriteLine("\n--- Login ---");
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.Write("Username: "); string username = (Console.ReadLine() ?? "").Trim();
//            Console.Write("Password: "); string password = (Console.ReadLine() ?? "").Trim();

//            return GetCustIdByCredentials(username, password);
//        }

//        // Query sp_LoginUser to get CustID (or null)
//        static string GetCustIdByCredentials(string username, string password)
//        {
//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();
//                using (SqlCommand cmd = new SqlCommand("sp_LoginUser", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@Username", username);
//                    cmd.Parameters.AddWithValue("@Password", password);

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            return reader["CustID"].ToString();
//                        }
//                        else
//                        {
//                            return null;
//                        }
//                    }
//                }
//            }
//        }

//        // -------------------------
//        // Search Trains 
//        // -------------------------
//        static void SearchTrains()
//        {
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.WriteLine("\n--- Search Trains ---");
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.Write("Source: "); string source = Console.ReadLine();
//            Console.Write("Destination: "); string destination = Console.ReadLine();
//            Console.Write("Travel Date (yyyy-mm-dd) or enter to skip: "); string date = Console.ReadLine();

//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();
//                using (SqlCommand cmd = new SqlCommand("sp_SearchTrains", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@Source", source);
//                    cmd.Parameters.AddWithValue("@Destination", destination);
//                    if (string.IsNullOrWhiteSpace(date))
//                        cmd.Parameters.AddWithValue("@TravelDate", DBNull.Value);
//                    else
//                        cmd.Parameters.AddWithValue("@TravelDate", DateTime.Parse(date));

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        Console.WriteLine("\nTrainNo | TrainName | ClassType | Seats | Price | Departure");
//                        while (reader.Read())
//                        {
//                            Console.WriteLine($"{reader["TrainNo"]} | {reader["TrainName"]} | {reader["ClassType"]} | {reader["SeatsAvailable"]} | {reader["Price"]} | {reader["DepartureDateTime"]}");
//                        }
//                    }
//                }
//            }
//        }

//        // -------------------------
//        // Book Tickets 
//        // -------------------------
//        static void BookTickets()
//        {
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.WriteLine("\n--- Book Tickets ---");
//            Console.WriteLine("--------------------------------------------------------------");

//            // Ask if existing customer or new
//            Console.Write("Are you an existing customer? (Y/N): ");
//            string existing = (Console.ReadLine() ?? "").Trim().ToUpper();

//            string custId = null;
//            string name = null, phone = null, email = null, username = null, password = null, address = null;

//            if (existing == "Y")
//            {
//                // Login existing customer
//                Console.Write("Username: "); string u = (Console.ReadLine() ?? "").Trim();
//                Console.Write("Password: "); string p = (Console.ReadLine() ?? "").Trim();
//                var id = GetCustIdByCredentials(u, p);
//                if (id == null)
//                {
//                    Console.WriteLine("Login failed. Aborting booking.");
//                    return;
//                }
//                custId = id;
//                Console.WriteLine($"Using existing customer: {custId}");
//            }
//            else
//            {
//                // Collect details but DO NOT register yet
//                Console.WriteLine("\n Enter customer details (these will be used to create the customer account AFTER payment confirmation):");
//                Console.Write("Name: "); name = (Console.ReadLine() ?? "").Trim();
//                Console.Write("Phone: "); phone = (Console.ReadLine() ?? "").Trim();
//                Console.Write("Email: "); email = (Console.ReadLine() ?? "").Trim();
//                Console.Write("Username: "); username = (Console.ReadLine() ?? "").Trim();
//                Console.Write("Password: "); password = (Console.ReadLine() ?? "").Trim();
//                Console.Write("Address: "); address = (Console.ReadLine() ?? "").Trim();
//            }

//            // Train & booking details
//            Console.Write("Train No: "); string trainNo = (Console.ReadLine() ?? "").Trim();
//            Console.Write("Class Type: "); string classType = (Console.ReadLine() ?? "").Trim();
//            Console.Write("Seats to Book: ");
//            if (!int.TryParse(Console.ReadLine(), out int seats) || seats <= 0) { Console.WriteLine("Invalid seats."); return; }
//            Console.Write("Travel Date (yyyy-mm-dd): ");
//            if (!DateTime.TryParse(Console.ReadLine(), out DateTime travelDate)) { Console.WriteLine("Invalid date."); return; }

//            // Fetch price per seat and check seats exist
//            decimal pricePerSeat;
//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();
//                using (SqlCommand cmd = new SqlCommand("SELECT SeatsAvailable, Price FROM TrainDetails WHERE TrainNo=@TrainNo AND ClassType=@ClassType", con))
//                {
//                    cmd.Parameters.AddWithValue("@TrainNo", trainNo);
//                    cmd.Parameters.AddWithValue("@ClassType", classType);
//                    using (var rdr = cmd.ExecuteReader())
//                    {
//                        if (!rdr.Read())
//                        {
//                            Console.WriteLine("Train/Class not found.");
//                            return;
//                        }
//                        int seatsAvailable = Convert.ToInt32(rdr["SeatsAvailable"]);
//                        pricePerSeat = Convert.ToDecimal(rdr["Price"]);
//                        if (seats > seatsAvailable)
//                        {
//                            Console.WriteLine($"Not enough seats available, Added to Waiting List. Available: {seatsAvailable}");
//                            return;
//                        }
//                    }
//                }
//            }

//            // Collect passenger details (we will insert them only AFTER booking is done)
//            var passengers = new (string Name, int Age, string Gender, string Berth)[seats];
//            for (int i = 0; i < seats; i++)
//            {
//                Console.WriteLine($"\nEnter details for Passenger {i + 1}:");
//                Console.Write("Name: "); string pname = (Console.ReadLine() ?? "").Trim();
//                Console.Write("Age: "); if (!int.TryParse(Console.ReadLine(), out int age)) { Console.WriteLine("Invalid age."); return; }
//                Console.Write("Gender: "); string gender = (Console.ReadLine() ?? "").Trim();
//                Console.Write("Berth Allotment: "); string berth = (Console.ReadLine() ?? "").Trim();
//                passengers[i] = (pname, age, gender, berth);
//            }

//            decimal totalCost = pricePerSeat * seats;
//            Console.WriteLine($"\nTotal Cost: {totalCost:0.00}");

//            // Simulate payment
//            Console.Write("Proceed to payment? (Y/N): ");
//            if ((Console.ReadLine() ?? "").Trim().ToUpper() != "Y")
//            {
//                Console.WriteLine("Booking aborted before payment. No customer or passenger records were saved.");
//                return;
//            }

//            // Payment confirmed -> now register customer if needed (only now we persist customer)
//            if (string.IsNullOrEmpty(custId))
//            {
//                try
//                {
//                    custId = RegisterUser(name, phone, email, username, password, address);
//                    Console.WriteLine($"Customer registered: {custId}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Failed to register customer after payment: " + ex.Message);
//                    return;
//                }
//            }

//            // Generate BookingID
//            string bookingId;
//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();

//                // Get next BookingID
//                using (SqlCommand cmd = new SqlCommand("GetNextBookingID", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    var scalar = cmd.ExecuteScalar();
//                    if (scalar == null)
//                    {
//                        Console.WriteLine("Failed to generate Booking ID.");
//                        return;
//                    }
//                    bookingId = scalar.ToString();
//                }

//                // Start transaction so booking + passenger inserts are atomic
//                using (SqlTransaction transaction = con.BeginTransaction())
//                {
//                    try
//                    {
//                        // Call sp_BookTickets (this proc itself uses a transaction internally; calling within an outer transaction is safe)
//                        using (SqlCommand cmdBook = new SqlCommand("sp_BookTickets", con, transaction))
//                        {
//                            cmdBook.CommandType = CommandType.StoredProcedure;
//                            cmdBook.Parameters.AddWithValue("@BookingID", bookingId);
//                            cmdBook.Parameters.AddWithValue("@CustID", custId);
//                            cmdBook.Parameters.AddWithValue("@TrainNo", trainNo);
//                            cmdBook.Parameters.AddWithValue("@ClassType", classType);
//                            cmdBook.Parameters.AddWithValue("@SeatsBooked", seats);
//                            cmdBook.Parameters.AddWithValue("@TotalCost", totalCost);
//                            cmdBook.Parameters.AddWithValue("@TravelDate", travelDate.Date);
//                            cmdBook.ExecuteNonQuery();
//                        }

//                        // Now insert passengers (booking exists)
//                        for (int i = 0; i < seats; i++)
//                        {
//                            // Generate PassengerID
//                            string passengerId;
//                            using (SqlCommand cmdPid = new SqlCommand("GetNextPassengerID", con, transaction))
//                            {
//                                cmdPid.CommandType = CommandType.StoredProcedure;
//                                var sc = cmdPid.ExecuteScalar();
//                                if (sc == null) throw new ApplicationException("Failed to generate Passenger ID.");
//                                passengerId = sc.ToString();
//                            }

//                            using (SqlCommand cmdIns = new SqlCommand("sp_InsertPassenger", con, transaction))
//                            {
//                                cmdIns.CommandType = CommandType.StoredProcedure;
//                                cmdIns.Parameters.AddWithValue("@PassengerID", passengerId);
//                                cmdIns.Parameters.AddWithValue("@BookingID", bookingId);
//                                cmdIns.Parameters.AddWithValue("@PassengerName", passengers[i].Name);
//                                cmdIns.Parameters.AddWithValue("@Age", passengers[i].Age);
//                                cmdIns.Parameters.AddWithValue("@Gender", passengers[i].Gender);
//                                cmdIns.Parameters.AddWithValue("@BerthAllotment", passengers[i].Berth);
//                                cmdIns.ExecuteNonQuery();
//                            }

//                            Console.WriteLine($"Passenger inserted. Passenger ID: {passengerId}");
//                        }

//                        // All good -> commit transaction
//                        transaction.Commit();
//                        Console.WriteLine($"\nBooking successful. Your Booking ID is {bookingId}");
//                    }
//                    catch (SqlException ex)
//                    {
//                        try { transaction.Rollback(); } catch { /* ignore */ }
//                        Console.WriteLine("Booking failed and rolled back: " + ex.Message);
//                        return;
//                    }
//                    catch (Exception ex)
//                    {
//                        try { transaction.Rollback(); } catch { /* ignore */ }
//                        Console.WriteLine("Booking failed and rolled back: " + ex.Message);
//                        return;
//                    }
//                } // end transaction
//            } // end connection
//        }

//        // -------------------------
//        // Cancel Full Booking 
//        // -------------------------
//        static void CancelFullBooking()
//        {
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.WriteLine("\n--- Cancel Full Booking ---");
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.Write("Booking ID: "); string bookingId = Console.ReadLine();
//            Console.Write("Registered Phone: "); string phone = Console.ReadLine();

//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();
//                using (SqlCommand cmd = new SqlCommand("sp_CancelFullBooking", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
//                    cmd.Parameters.AddWithValue("@Phone", phone);
//                    try
//                    {
//                        cmd.ExecuteNonQuery();
//                        Console.WriteLine("Full booking cancelled with 50% refund.");
//                    }
//                    catch (SqlException ex)
//                    {
//                        Console.WriteLine("Cancellation failed: " + ex.Message);
//                    }
//                }
//            }
//        }

//        // -------------------------
//        // Cancel Selected Passengers 
//        // -------------------------
//        static void CancelSelectedPassengers()
//        {
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.WriteLine("\n--- Cancel Selected Passengers ---");
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.Write("Booking ID: "); string bookingId = Console.ReadLine();
//            Console.Write("Passenger IDs (comma-separated, e.g. P00001,P00002): ");
//            string idsInput = Console.ReadLine();
//            var tokens = idsInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
//            if (tokens.Length == 0) { Console.WriteLine("No Passenger IDs provided."); return; }

//            Console.Write("Confirm Cancellation? (Y/N): ");
//            if (Console.ReadLine().Trim().ToUpper() != "Y") { Console.WriteLine("Cancellation aborted by user."); return; }

//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();
//                using (SqlCommand cmd = new SqlCommand("sp_CancelPassengersByPassengerIDs", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@PassengerIDs", string.Join(",", tokens));
//                    try
//                    {
//                        cmd.ExecuteNonQuery();
//                        Console.WriteLine($"Tickets cancelled: {string.Join(",", tokens)}");
//                    }
//                    catch (SqlException ex)
//                    {
//                        Console.WriteLine("Cancellation failed: " + ex.Message);
//                    }
//                }
//            }
//        }

//        // -------------------------
//        // TicketReport 
//        // -------------------------
//        //static void TicketReport()
//        //{
//        //    Console.WriteLine("\n--- Ticket Report ---");
//        //    Console.Write("Booking ID: "); string bookingId = Console.ReadLine();

//        //    using (SqlConnection con = new SqlConnection(connectionString))
//        //    {
//        //        con.Open();
//        //        using (SqlCommand cmd = new SqlCommand("sp_TicketReport", con))
//        //        {
//        //            cmd.CommandType = CommandType.StoredProcedure;
//        //            cmd.Parameters.AddWithValue("@BookingID", bookingId);

//        //            using (SqlDataReader reader = cmd.ExecuteReader())
//        //            {
//        //                Console.WriteLine("\n--- Ticket Details ---");
//        //                if (!reader.HasRows) { Console.WriteLine("No records found."); return; }
//        //                while (reader.Read())
//        //                {
//        //                    Console.WriteLine($"BookingID: {reader["BookingID"]} | Date: {reader["BookingDate"]} | Cost: {reader["TotalCost"]}");
//        //                    Console.WriteLine($"Train: {reader["TrainName"]} | From: {reader["Source"]} | To: {reader["Destination"]} | Departure: {reader["DepartureDateTime"]}");
//        //                    Console.WriteLine($"PassengerID: {reader["PassengerID"]} | Name: {reader["PassengerName"]} | Age: {reader["Age"]} | Gender: {reader["Gender"]} | Berth: {reader["BerthAllotment"]} | Cancelle: {reader["IsCancelled"]}");

//        //                }
//        //            }
//        //        }
//        //    }
//        //}

//        static void TicketReport()
//        {
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.WriteLine("\n--- Ticket Report ---");
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.Write("Booking ID: ");
//            string bookingId = Console.ReadLine();

//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();
//                using (SqlCommand cmd = new SqlCommand("sp_TicketReport", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@BookingID", bookingId);

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
//                        Console.WriteLine("\n--- Ticket Details ---");
//                        if (!reader.HasRows)
//                        {
//                            Console.WriteLine("No records found.");
//                            return;
//                        }

//                        while (reader.Read())
//                        {
//                            string status = (reader["IsCancelled"] != DBNull.Value && (bool)reader["IsCancelled"])
//                                            ? "Cancelled"
//                                            : "Not Cancelled";

//                            Console.WriteLine($"BookingID: {reader["BookingID"]} | Date: {reader["BookingDate"]} | Cost: {reader["TotalCost"]}");
//                            Console.WriteLine($"Train: {reader["TrainName"]} | From: {reader["Source"]} | To: {reader["Destination"]} | Departure: {reader["DepartureDateTime"]}");
//                            Console.WriteLine($"PassengerID: {reader["PassengerID"]} | Name: {reader["PassengerName"]} | Age: {reader["Age"]} | Gender: {reader["Gender"]} | Berth: {reader["BerthAllotment"]} | Ticket_Status: {status}");
//                            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
//                        }
//                    }
//                }
//            }
//        }


//        // -------------------------
//        // SoftDeleteUser
//        // -------------------------
//        static void SoftDeleteUser()
//        {
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.WriteLine("\n--- Soft Delete User ---");
//            Console.WriteLine("--------------------------------------------------------------");
//            Console.Write("Customer ID: "); string custId = Console.ReadLine();

//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                con.Open();
//                using (SqlCommand cmd = new SqlCommand("sp_SoftDeleteUser", con))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@CustID", custId);
//                    cmd.ExecuteNonQuery();
//                    Console.WriteLine("User marked as deleted.");
//                }
//            }
//        }
//    }
//}
