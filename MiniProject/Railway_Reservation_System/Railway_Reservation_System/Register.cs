using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;



namespace Railway_Reservation_System
{
    public class Register
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";

        public static string RegisterUser()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                         Register User                        ");
            Console.WriteLine("--------------------------------------------------------------");
            Console.Write("Name: "); string name = (Console.ReadLine() ?? "").Trim();
            Console.Write("Phone: "); string phone = (Console.ReadLine() ?? "").Trim();
            Console.Write("Email: "); string email = (Console.ReadLine() ?? "").Trim();
            Console.Write("Username: "); string username = (Console.ReadLine() ?? "").Trim();
            Console.Write("Password: "); string password = (Console.ReadLine() ?? "").Trim();
            Console.Write("Address: "); string address = (Console.ReadLine() ?? "").Trim();

            try
            {
                return RegisterUser(name, phone, email, username, password, address);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registration failed: " + ex.Message);
                return null;
            }
        }

        // Calls stored procedure sp_RegisterUser and returns new CustID (or throws)
        public static string RegisterUser(string name, string phone, string email, string username, string password, string address)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // First check if username already exists to provide a friendly message
                using (SqlCommand chk = new SqlCommand("SELECT CustID FROM Customer WHERE Username = @Username AND IsDeleted = 0", con))
                {
                    chk.Parameters.AddWithValue("@Username", username);
                    var exist = chk.ExecuteScalar();
                    if (exist != null)
                        throw new ApplicationException("\n Username already exists. Choose a different username.");
                }

                using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustName", name);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Address", address);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["NewCustID"].ToString();
                        }
                        else
                        {
                            throw new ApplicationException("\n Unknown error during registration.");
                        }
                    }
                }
            }
        }
    }
}
