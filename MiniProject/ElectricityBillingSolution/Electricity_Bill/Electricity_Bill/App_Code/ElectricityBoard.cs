using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Electricity.Data;

namespace Electricity_Bill.App_Code
{
    public static class ElectricityBoard
    {
        public static void CalculateBill(ElectricityBill ebill)
        {
            int units = ebill.UnitsConsumed;
            double amount = 0;

            if (units <= 100)
            {
                amount = 0;
            }
            else if (units <= 300)
            {
                amount = (units - 100) * 1.5;
            }
            else if (units <= 600)
            {
                amount = 200 * 1.5 + (units - 300) * 3.5;
            }
            else if (units <= 1000)
            {
                amount = 200 * 1.5 + 300 * 3.5 + (units - 600) * 5.5;
            }
            else
            {
                amount = 200 * 1.5 + 300 * 3.5 + 400 * 5.5 + (units - 1000) * 7.5;
            }

            ebill.BillAmount = amount;
        }

        public static void AddBill(ElectricityBill ebill)
        {
            using (SqlConnection con = DBHandler.GetConnection())
            using (SqlCommand cmd = new SqlCommand("dbo.sp_InsertBill", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cno", SqlDbType.NVarChar, 20) { Value = ebill.ConsumerNumber });
                cmd.Parameters.Add(new SqlParameter("@cname", SqlDbType.NVarChar, 50) { Value = ebill.ConsumerName });
                cmd.Parameters.Add(new SqlParameter("@units", SqlDbType.Int) { Value = ebill.UnitsConsumed });
                cmd.Parameters.Add(new SqlParameter("@amt", SqlDbType.Decimal) { Value = ebill.BillAmount, Precision = 18, Scale = 2 });

                con.Open();
                // optional read new id:
                cmd.ExecuteNonQuery();
            }
        }

        public static List<ElectricityBill> Generate_N_BillDetails(int num)
        {
            var list = new List<ElectricityBill>();
            using (SqlConnection con = DBHandler.GetConnection())
            using (SqlCommand cmd = new SqlCommand("dbo.sp_GetLastNBills", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@n", SqlDbType.Int) { Value = num });

                con.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var eb = new ElectricityBill
                        {
                            ConsumerNumber = rdr.GetString(0),
                            ConsumerName = rdr.GetString(1),
                            UnitsConsumed = rdr.GetInt32(2),
                            BillAmount = Convert.ToDouble(rdr.GetDecimal(3))
                        };
                        list.Add(eb);
                    }
                }
            }
            return list;
        }
    }
}