using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Electricity_Bill
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text.Trim();

            // fetch connection string from Web.config
            string cs = ConfigurationManager.ConnectionStrings["ElectricityBillBoardDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                string query = "SELECT COUNT(1) FROM AdminUsers WHERE Username=@Username AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                int count = (int)cmd.ExecuteScalar();

                if (count == 1)
                {
                    Session["User"] = username;
                    Response.Redirect("ElectricityBoardBilling.aspx");
                }
                else
                {
                    lblMsg.Text = "Invalid username or password!";
                }
            }
        }
    }
}