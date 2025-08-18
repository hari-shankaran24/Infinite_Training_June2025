using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Electricity_Bill
{
    public partial class ElectricityBoardBilling : Page
    {
        private static int remainingBills = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx"); // redirect to login if not logged in
            }
        }

        protected void btnBegin_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtTotalBills.Text, out int total))
            {
                remainingBills = total;
                lblRemaining.Text = remainingBills.ToString();
                lblMsg.Text = "You can start entering bills.";
            }
            else
            {
                lblMsg.Text = "Enter a valid number!";
            }
        }

        protected void btnAddBill_Click(object sender, EventArgs e)
        {
            try
            {
                string consumerNumber = txtConsumerNumber.Text.Trim();
                string consumerName = txtConsumerName.Text.Trim();
                int units;

                // Validation 1: Consumer Number format
                if (!System.Text.RegularExpressions.Regex.IsMatch(consumerNumber, @"^EB\d{5}$"))
                {
                    throw new FormatException("Invalid Consumer Number. Format: EBXXXXX");
                }

                // Validation 2: Units must be >= 0
                if (!int.TryParse(txtUnits.Text.Trim(), out units))
                {
                    lblMsg.Text = "Please enter a valid number for Units.";
                    return;
                }
                if (units < 0)
                {
                    lblMsg.Text = "Given units is invalid. Please enter again.";
                    return;
                }

                // Calculate bill
                decimal billAmount = CalculateBill(units);

                // Save to DB
                string cs = ConfigurationManager.ConnectionStrings["ElectricityBillBoardDB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "INSERT INTO ElectricityBill (consumer_number, consumer_name, units_consumed, bill_amount) " +
                                   "VALUES (@ConsumerNumber, @ConsumerName, @UnitsConsumed, @BillAmount)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ConsumerNumber", consumerNumber);
                    cmd.Parameters.AddWithValue("@ConsumerName", consumerName);
                    cmd.Parameters.AddWithValue("@UnitsConsumed", units);
                    cmd.Parameters.AddWithValue("@BillAmount", billAmount);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                // Show success
                lblMsg.Text = $"{consumerNumber} {consumerName} {units} Bill Amount : {billAmount}";

                // Clear fields for next entry
                txtConsumerNumber.Text = "";
                txtConsumerName.Text = "";
                txtUnits.Text = "";
            }
            catch (FormatException ex)
            {
                lblMsg.Text = ex.Message; // Consumer Number invalid
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtLastN.Text, out int n))
            {
                lblMsg.Text = "Enter valid number for N!";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["ElectricityBillBoardDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT TOP (@N) consumer_number, consumer_name, units_consumed, bill_amount " +
                               "FROM ElectricityBill ORDER BY bill_id DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@N", n);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBills.DataSource = dt;
                gvBills.DataBind();

                // Generate summary text
                string summary = "";
                foreach (DataRow row in dt.Rows)
                {
                    summary += $"<br/>EB Bill for {row["consumer_name"]} is {row["bill_amount"]}";
                }
                lblSummary.Text = summary;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        private decimal CalculateBill(int units)
        {
            decimal amount = 0;

            if (units <= 100)
            {
                amount = 0; // Free
            }
            else if (units <= 300)
            {
                amount = (units - 100) * 1.5m;
            }
            else if (units <= 600)
            {
                amount = (200 * 1.5m) + (units - 300) * 3.5m;
            }
            else if (units <= 1000)
            {
                amount = (200 * 1.5m) + (300 * 3.5m) + (units - 600) * 5.5m;
            }
            else // > 1000
            {
                amount = (200 * 1.5m) + (300 * 3.5m) + (400 * 5.5m) + (units - 1000) * 7.5m;
            }

            return amount;
        }
    }
}