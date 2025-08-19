using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


//namespace Electricity_Bill
//{
//    public partial class ElectricityBoardBilling : Page
//    {
//        private static int remainingBills = 0;

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (Session["User"] == null)
//            {
//                Response.Redirect("Login.aspx"); // redirect to login if not logged in
//            }
//        }

//        protected void btnBegin_Click(object sender, EventArgs e)
//        {
//            if (int.TryParse(txtTotalBills.Text, out int total))
//            {
//                remainingBills = total;
//                lblRemaining.Text = remainingBills.ToString();
//                lblMsg.Text = "You can start entering bills.";
//            }
//            else
//            {
//                lblMsg.Text = "Enter a valid number!";
//            }
//        }

//        protected void btnAddBill_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                string consumerNumber = txtConsumerNumber.Text.Trim();
//                string consumerName = txtConsumerName.Text.Trim();
//                int units;

//                // Validation 1: Consumer Number format
//                if (!System.Text.RegularExpressions.Regex.IsMatch(consumerNumber, @"^EB\d{5}$"))
//                {
//                    throw new FormatException("Invalid Consumer Number. Format: EBXXXXX");
//                }

//                // Validation 2: Units must be >= 0
//                if (!int.TryParse(txtUnits.Text.Trim(), out units))
//                {
//                    lblMsg.Text = "Please enter a valid number for Units.";
//                    return;
//                }
//                if (units < 0)
//                {
//                    lblMsg.Text = "Given units is invalid. Please enter again.";
//                    return;
//                }

//                // Calculate bill
//                decimal billAmount = CalculateBill(units);

//                // Save to DB
//                string cs = ConfigurationManager.ConnectionStrings["ElectricityBillBoardDB"].ConnectionString;
//                using (SqlConnection con = new SqlConnection(cs))
//                {
//                    string query = "INSERT INTO ElectricityBill (consumer_number, consumer_name, units_consumed, bill_amount) " +
//                                   "VALUES (@ConsumerNumber, @ConsumerName, @UnitsConsumed, @BillAmount)";
//                    SqlCommand cmd = new SqlCommand(query, con);
//                    cmd.Parameters.AddWithValue("@ConsumerNumber", consumerNumber);
//                    cmd.Parameters.AddWithValue("@ConsumerName", consumerName);
//                    cmd.Parameters.AddWithValue("@UnitsConsumed", units);
//                    cmd.Parameters.AddWithValue("@BillAmount", billAmount);

//                    con.Open();
//                    cmd.ExecuteNonQuery();
//                }

//                // Show success
//                lblMsg.Text = $"{consumerNumber} {consumerName} {units} Bill Amount : {billAmount}";

//                // Clear fields for next entry
//                txtConsumerNumber.Text = "";
//                txtConsumerName.Text = "";
//                txtUnits.Text = "";
//            }
//            catch (FormatException ex)
//            {
//                lblMsg.Text = ex.Message; // Consumer Number invalid
//            }
//            catch (Exception ex)
//            {
//                lblMsg.Text = "Error: " + ex.Message;
//            }
//        }

//        protected void btnRetrieve_Click(object sender, EventArgs e)
//        {
//            if (!int.TryParse(txtLastN.Text, out int n))
//            {
//                lblMsg.Text = "Enter valid number for N!";
//                return;
//            }

//            string cs = ConfigurationManager.ConnectionStrings["ElectricityBillBoardDB"].ConnectionString;
//            using (SqlConnection con = new SqlConnection(cs))
//            {
//                string query = "SELECT TOP (@N) consumer_number, consumer_name, units_consumed, bill_amount " +
//                               "FROM ElectricityBill ORDER BY bill_id DESC";

//                SqlCommand cmd = new SqlCommand(query, con);
//                cmd.Parameters.AddWithValue("@N", n);

//                SqlDataAdapter da = new SqlDataAdapter(cmd);
//                DataTable dt = new DataTable();
//                da.Fill(dt);

//                gvBills.DataSource = dt;
//                gvBills.DataBind();

//                // Generate summary text
//                string summary = "";
//                foreach (DataRow row in dt.Rows)
//                {
//                    summary += $"<br/>EB Bill for {row["consumer_name"]} is {row["bill_amount"]}";
//                }
//                lblSummary.Text = summary;
//            }
//        }

//        protected void btnLogout_Click(object sender, EventArgs e)
//        {
//            Session.Clear();
//            Response.Redirect("Login.aspx");
//        }

//        private decimal CalculateBill(int units)
//        {
//            decimal amount = 0;

//            if (units <= 100)
//            {
//                amount = 0; // Free
//            }
//            else if (units <= 300)
//            {
//                amount = (units - 100) * 1.5m;
//            }
//            else if (units <= 600)
//            {
//                amount = (200 * 1.5m) + (units - 300) * 3.5m;
//            }
//            else if (units <= 1000)
//            {
//                amount = (200 * 1.5m) + (300 * 3.5m) + (units - 600) * 5.5m;
//            }
//            else // > 1000
//            {
//                amount = (200 * 1.5m) + (300 * 3.5m) + (400 * 5.5m) + (units - 1000) * 7.5m;
//            }

//            return amount;
//        }
//    }
//}

namespace Electricity_Bill
{
    public partial class ElectricityBoardBilling : Page
    {
        private static int totalBills = 0; // Total number of bills to be added
        private static int billsEntered = 0; // Number of bills that have been entered

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx"); // redirect to login if not logged in
            }

            if (!IsPostBack)
            {
                // Reset counters when page is first loaded
                billsEntered = 0;
            }
        }

        protected void btnBegin_Click(object sender, EventArgs e)
        {
            // Check if the entered number is a valid integer
            if (int.TryParse(txtTotalBills.Text, out int total))
            {
                totalBills = total;  // Set total number of bills to be entered
                lblRemaining.Text = totalBills.ToString();
                lblMsg.Text = "You can start entering bills.";

                // Disable the textbox and button after entering total bills, to prevent changes
                txtTotalBills.Enabled = false;
                btnBegin.Enabled = false;
            }
            else
            {
                lblMsg.Text = "Enter a valid number for total bills!";
            }
        }

        protected void btnAddBill_Click(object sender, EventArgs e)
        {
            // If total bills are added, no more bills can be entered
            if (billsEntered >= totalBills)
            {
                lblMsg.Text = "All bills have been added. You cannot add more.";
                return;
            }

            try
            {
                string consumerNumber = txtConsumerNumber.Text.Trim();
                string consumerName = txtConsumerName.Text.Trim();
                int units;

                // Validate Consumer Number and Units
                if (!System.Text.RegularExpressions.Regex.IsMatch(consumerNumber, @"^EB\d{5}$"))
                {
                    lblMsg.Text = "Invalid Consumer Number. Format: EBXXXXX";
                    return;
                }

                // Check if the Consumer Number already exists
                if (CheckIfConsumerExists(consumerNumber))
                {
                    lblMsg.Text = "Customer with this Consumer Number already exists. Please enter a new valid Consumer Number.";
                    // Clear only the Consumer Number field for re-entry
                    txtConsumerNumber.Text = "";
                    txtConsumerNumber.Focus();
                    return;
                }

                if (!int.TryParse(txtUnits.Text.Trim(), out units))
                {
                    lblMsg.Text = "Please enter a valid number for Units.";
                    return;
                }

                if (units < 0)
                {
                    lblMsg.Text = "Units cannot be negative.";
                    return;
                }

                // Calculate the bill
                decimal billAmount = CalculateBill(units);

                // Save the bill to the database
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

                // Increase the entered bills counter
                billsEntered++;

                // Show success message
                lblMsg.Text = $"Bill for {consumerName} (Consumer Number: {consumerNumber}) added successfully. " +
                              $"Bill Amount: {billAmount:C}. {totalBills - billsEntered} bills remaining.";

                // Update the remaining bills label
                lblRemaining.Text = (totalBills - billsEntered).ToString();

                // Clear fields for next entry
                txtConsumerName.Text = "";
                txtUnits.Text = "";

                // If all bills are entered, disable the Add Bill button
                if (billsEntered >= totalBills)
                {
                    lblMsg.Text = "All bills have been entered.";
                    btnAddBill.Enabled = false; // Disable the add button
                }

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

        // Method to check if a consumer number already exists in the database
        private bool CheckIfConsumerExists(string consumerNumber)
        {
            bool exists = false;
            string cs = ConfigurationManager.ConnectionStrings["ElectricityBillBoardDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT COUNT(1) FROM ElectricityBill WHERE consumer_number = @ConsumerNumber";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ConsumerNumber", consumerNumber);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    exists = true;
                }
            }

            return exists;
        }
    }
}