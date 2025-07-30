using System;
using System.Data;
using System.Data.SqlClient;

namespace Code_Challenge_1
{
    class Update_Sal
    {
        static void Main(string[] args)
        {
            UpdateEmployeeSalary();
            Console.ReadLine();
        }

        static void UpdateEmployeeSalary()
        {
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                con = new SqlConnection("Data Source=ICS-LT-8F8LQ73;Initial Catalog=AssessmentsDB;User ID=sa;Password=Hari@123456789");
                con.Open();

                Console.Write("Enter Employee ID to update salary: ");
                int empId = int.Parse(Console.ReadLine());

                cmd = new SqlCommand("UpdateSal", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmpId", empId);

                SqlParameter paramUpdatedSal = new SqlParameter("@UpdatedSalary", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 2,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramUpdatedSal);

                cmd.ExecuteNonQuery();

                Console.WriteLine("\n-------------Salary Updated-------------");
                Console.WriteLine($"EmpId: {empId}");
                Console.WriteLine($"Updated Salary: {paramUpdatedSal.Value}\n");

                DisplayEmployeeDetails(con, empId);
            }
            catch (SqlException se)
            {
                Console.WriteLine("SQL error occurred: " + se.Message);
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Input format is wrong: " + fe.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        static void DisplayEmployeeDetails(SqlConnection con, int empId)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT EmpId, Name, Salary, Gender FROM Employee_Details WHERE EmpId = @EmpId", con))
            {
                cmd.Parameters.AddWithValue("@EmpId", empId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        Console.WriteLine("---------- Employee Details -----------");
                        Console.WriteLine($"EmpId: {dr["EmpId"]}");
                        Console.WriteLine($"Name: {dr["Name"]}");
                        Console.WriteLine($"Salary: {dr["Salary"]}");
                        Console.WriteLine($"Gender: {dr["Gender"]}");
                        Console.WriteLine("--------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found.");
                    }
                }
            }
        }
    }
}
