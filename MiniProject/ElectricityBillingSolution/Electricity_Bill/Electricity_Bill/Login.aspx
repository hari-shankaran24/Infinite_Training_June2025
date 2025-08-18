<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Electricity_Bill.Login" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Styles.css" rel="stylesheet" />

    <style>
        .login-container {
            width: 400px;
            margin: 100px auto; 
            padding: 30px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
            text-align: center;
            font-size: 18px; 
        }

        .login-container h2 {
            font-size: 24px; 
            margin-bottom: 30px;
            text-align:center;
        }

        .login-container label, 
        .login-container input {
            display: block;
            width: 100%;
            margin-bottom: 20px;
            text-align: left;
        }

        .login-container input[type="text"],
        .login-container input[type="password"] {
            padding: 12px;
            font-size: 18px;
            box-sizing: border-box;
            margin-bottom: 20px;
        }

        .login-container input[type="submit"],
        .login-container input[type="button"],
        .login-container .aspNet-Button {
           padding: 12px;
           font-size: 18px;  
           width: auto;      
           margin: 0 auto;  
        }

        .login-container .message {
            margin-top: 15px;
            font-size: 16px;
        }

        /* Red text for error message */
        .login-container .error-message {
            color: red;
            font-size: 16px;
        }
    </style>

    <div class="login-container">
        <h2>Login</h2>

        <asp:Label ID="lblUser" runat="server" Text="Username:"></asp:Label>
        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>

        <asp:Label ID="lblPass" runat="server" Text="Password:"></asp:Label>
        <asp:TextBox ID="txtPass" runat="server" TextMode="Password"></asp:TextBox>

        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" CssClass="aspNet-Button" />

        <asp:Label ID="lblMsg" runat="server" CssClass="error-message"></asp:Label>
    </div>
</asp:Content>