using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;


namespace Electricity.Data
{
    public static class DBHandler
    {
        public static SqlConnection GetConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["ElectricityBillBoardDB"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("Connection string 'ElectricityBillBoardDB' not found in Web.config.");

            return new SqlConnection(cs);
        }
    }
}
