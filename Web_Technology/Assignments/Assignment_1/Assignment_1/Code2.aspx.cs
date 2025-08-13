using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment_1
{
    public partial class Code2 : System.Web.UI.Page
    {
        private Dictionary<string, (string ImageUrl, string Price)> products = new Dictionary<string, (string, string)>
        {
            { "Pixel 9A", ("~/Images/Pixel-9A.jpg", "₹49,999") },
            { "iphone 16 Pro Max", ("~/Images/iphone-17-pro-Max.jpg", "₹1,65,999") },
            { "Oneplus 14R", ("~/Images/Oneplus.jpeg", "₹70,999") },
            { "Poco X7 Pro ", ("~/Images/Poco.jpeg", "₹28,000") }
        };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlProducts.DataSource = products.Keys;
                ddlProducts.DataBind();
                ddlProducts.Items.Insert(0, new ListItem("-- Select Product --", ""));
            }
        }
 
        protected void ddlProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ddlProducts.SelectedValue;
            if (products.ContainsKey(selected))
            {
                imgProduct.ImageUrl = products[selected].ImageUrl;
                lblPrice.Text = "";
            }
            else
            {
                imgProduct.ImageUrl = "";
                lblPrice.Text = "";
            }
        }
 
        protected void btnGetPrice_Click(object sender, EventArgs e)
        {
            string selected = ddlProducts.SelectedValue;
            if (products.ContainsKey(selected))
            {
                lblPrice.Text = "Price: " + products[selected].Price;
            }
            else
            {
                lblPrice.Text = "Please select a product.";
            }
 
        }
    }
}