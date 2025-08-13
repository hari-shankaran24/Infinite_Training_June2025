<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Code2.aspx.cs" Inherits="Assignment_1.Code2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Product Viewer</title>
<style>
        body {
            margin: 0;
            padding: 0;
            background: linear-gradient(to right, #f0f4f8, #d9e2ec);
            font-family: 'Times New Roman', Times, serif
        }
 
        .container {
            max-width: 500px;
            margin: 60px auto;
            padding: 30px;
            background-color: blanchedalmond;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
            border-radius: 12px;
            text-align: center;
        }
 
        h2 {
            color: saddlebrown;
            margin-bottom: 20px;
        }
 
        .dropdown {
            width: 100%;
            padding: 12px;
            font-size: 16px;
            margin-bottom: 25px;
            border-radius: 6px;
            border: 1px solid silver;
            background-color: white;
            color: black;
            transition: border-color 0.3s ease;
         }
 
        .product-image {
            width: 220px;
            height: 220px;
            object-fit: contain;
            border: 1px solid #ddd;
            border-radius: 8px;
            margin: 20px 0;
        }
 
        .btn {
            background-color: burlywood;
            color: saddlebrown;
            border: none;
            padding: 10px 20px;
            font-size: 16px;
            border-radius: 6px;
            cursor: pointer;
        }
 
        .btn:hover {
            background-color: rosybrown;
        }
 
        .price-label {
            font-size: 20px;
            color: saddlebrown;
            margin-top: 15px;
            font-weight: bold;
        }
</style>
</head>
<body>
<form id="form1" runat="server">
<div class="container">
<h2>Select a Product</h2>
 
    <asp:DropDownList ID="ddlProducts" runat="server" 
       AutoPostBack="true" Class="dropdown" 
       OnSelectedIndexChanged="ddlProducts_SelectedIndexChanged" />
    <asp:Image ID="imgProduct" runat="server" Class="product-image" />
<br />
<asp:Button ID="btnGetPrice" runat="server" 
    Text="Get Price" Class="btn" OnClick="btnGetPrice_Click" />
<br /><br />
<asp:Label ID="lblPrice" runat="server" Class="price-label" />
</div>
</form>
</body>
</html>>