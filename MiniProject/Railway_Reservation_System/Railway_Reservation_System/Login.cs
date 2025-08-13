using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;

namespace Railway_Reservation_System
{
    public class Login
    {
        static string connectionString = "Data Source=ICS-LT-8F8LQ73;Initial Catalog=Railways;User ID=sa;Password=Hari@123456789";

        // -------------------------
        // Login  - returns CustID or null
        // -------------------------
       public static string LoginUser()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("                           Login                              ");
            Console.WriteLine("--------------------------------------------------------------");
            Console.Write("Username: "); string username = (Console.ReadLine() ?? "").Trim();
            Console.Write("Password: "); string password = (Console.ReadLine() ?? "").Trim();

            return GetCustIdByCredentials(username, password);
        }

        // Query sp_LoginUser to get CustID (or null)
        public static string GetCustIdByCredentials(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_LoginUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["CustID"].ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

    }
}
