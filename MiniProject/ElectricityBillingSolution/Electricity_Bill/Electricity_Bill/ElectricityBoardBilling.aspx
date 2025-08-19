

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ElectricityBoardBilling.aspx.cs" Inherits="Electricity_Bill.ElectricityBoardBilling" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Electricity Board - Billing Automation</title>
    <link href="Styles.css" rel="stylesheet" />
    <style type="text/css">
        /* Global reset and layout styles */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            display: flex;
            justify-content: center;
            align-items: flex-start;
            min-height: 100vh;
            padding: 20px;
        }

        .page-container {
            max-width: 1000px; /* Limiting the max-width of the content */
            width: 100%;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
            padding: 20px;
        }

        /* Title Style */
        h1 {
            font-size: 32px;     
            margin-top: 20px;    
            color: #333;         
            font-weight: bold;   
        }

        /* Section Styles */
        .section {
            margin: 20px 0;
        }

        .section h2 {
            font-size: 22px;
            margin-bottom: 10px;
        }

        /* Input Row Styling */
        .input-row {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            margin-bottom: 15px;
        }

        .input-text {
            padding: 10px;
            font-size: 16px;
            margin-right: 10px;
            width: calc(33% - 10px); /
            min-width: 200px;
        }

        .input-row span {
            display: block;
            margin-top: 10px;
        }

        /* Button Styling */
        .btn {
            padding: 10px 20px;
            font-size: 16px;
            cursor: pointer;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 5px;
            width: 100%;
            margin-top: 10px;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        .logout-btn {
            background-color: #f44336;
        }

        /* GridView Styling */
        .gridview {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }

        .gridview th, .gridview td {
            padding: 12px;
            text-align: center;
            border: 1px solid #ddd;
        }

        .gridview th {
            background-color: #f2f2f2;
        }

        /* Message and Summary Labels */
        .msg-label, .summary-label {
            font-size: 16px;
            font-weight: bold;
            margin-top: 15px;
            color: red;
        }

        .summary-label {
            color: green;
        }

        /* Logout Section */
        .logout-section {
            text-align: center;
            margin-top: 30px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
            <!-- Page Title -->
            <h1>Electricity Board - Billing Automation</h1>

            <!-- Section 1: Enter Number of Bills -->
            <div class="section">
                <h2>1) Enter Number of Bills</h2>
                <div class="input-row">
                    <asp:TextBox ID="txtTotalBills" runat="server" CssClass="input-text"></asp:TextBox>
                    <asp:Button ID="btnBegin" runat="server" Text="Begin" OnClick="btnBegin_Click" CssClass="btn" />
                    <span>Remaining: <asp:Label ID="lblRemaining" runat="server" Text="0"></asp:Label></span>
                </div>
            </div>

            <!-- Section 2: Add Customer Bill -->
            <div class="section">
                <h2>2) Add Each Customer's Details</h2>
                <div class="input-row">
                    <asp:TextBox ID="txtConsumerNumber" runat="server" CssClass="input-text" Placeholder="Enter Consumer Number"></asp:TextBox>
                    <asp:TextBox ID="txtConsumerName" runat="server" CssClass="input-text" Placeholder="Enter Consumer Name"></asp:TextBox>
                    <asp:TextBox ID="txtUnits" runat="server" CssClass="input-text" Placeholder="Enter Units Consumed"></asp:TextBox>
                    <asp:Button ID="btnAddBill" runat="server" Text="Add Bill" OnClick="btnAddBill_Click" CssClass="btn" />
                </div>
                <asp:Label ID="lblMsg" runat="server" CssClass="msg-label"></asp:Label>
            </div>

            <!-- Section 3: Retrieve Last N Bills -->
            <div class="section">
                <h2>3) Retrieve the Last 'N' Bills from the Database</h2>
                <div class="input-row">
                    <asp:TextBox ID="txtLastN" runat="server" CssClass="input-text"></asp:TextBox>
                    <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" OnClick="btnRetrieve_Click" CssClass="btn" />
                    <br />
                    <br />
                </div>

                <!-- GridView for Bills -->
                <asp:GridView ID="gvBills" runat="server" AutoGenerateColumns="False" CssClass="gridview" Height="135px" Width="439px">
                    <Columns>
                        <asp:BoundField DataField="consumer_number" HeaderText="Consumer #" />
                        <asp:BoundField DataField="consumer_name" HeaderText="Consumer Name" />
                        <asp:BoundField DataField="units_consumed" HeaderText="Units" />
                        <asp:BoundField DataField="bill_amount" HeaderText="Bill Amount" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblSummary" runat="server" CssClass="summary-label"></asp:Label>
            </div>

            <!-- Section 4: Logout Button -->
            <div class="logout-section">
                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" CssClass="btn logout-btn" />
            </div>
        </div>
    </form>
</body>
</html>
