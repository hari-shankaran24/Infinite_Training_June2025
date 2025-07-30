using System;
using System.Data;
using System.Data.SqlClient;

namespace Code_Challenge_1
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertEmployee();
            Console.ReadLine();
        }

        static void InsertEmployee()
        {
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                con = new SqlConnection("Data Source=ICS-LT-8F8LQ73;Initial Catalog=AssessmentsDB;User ID=sa;Password=Hari@123456789");
                con.Open();

                Console.Write("Enter Employee Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Salary: ");
                decimal givenSal = decimal.Parse(Console.ReadLine());

                Console.Write("Enter Gender: ");
                string gender = Console.ReadLine();

                cmd = new SqlCommand("InsertEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@GivenSal", givenSal);
                cmd.Parameters.AddWithValue("@Gender", gender);

                SqlParameter paramEmpId = new SqlParameter("@GeneratedEmpId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramEmpId);

                SqlParameter paramCalculatedSal = new SqlParameter("@CalculatedSal", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 2,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramCalculatedSal);

                cmd.ExecuteNonQuery();

                Console.WriteLine("\n-------------New Inserted Data-----------");
                Console.WriteLine("EmpId : " + paramEmpId.Value);
                Console.WriteLine("Salary after 10% deduction: " + paramCalculatedSal.Value);
                Console.WriteLine();
            }
            catch (SqlException se)
            {
                Console.WriteLine("SQL error occured: " + se.Message);
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
    }
}
