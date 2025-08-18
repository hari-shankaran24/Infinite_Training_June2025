<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ElectricityBoardBilling.aspx.cs" Inherits="Electricity_Bill.ElectricityBoardBilling" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Electricity Board - Billing Automation</title>
    <link href="Styles.css" rel="stylesheet" />
    <style type="text/css">
        .section {
            margin-left: 520px;
        }
        .gridview {}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
            <h1 style="margin-left: 525px">Electricity Board - Billing Automation</h1>

            <!-- Section 1: Enter how many bills -->
            <div class="section">
                <h2>1) Enter number of bills</h2>
                <div class="input-row">
                    <asp:TextBox ID="txtTotalBills" runat="server" CssClass="input-text"></asp:TextBox>
                    <asp:Button ID="btnBegin" runat="server" Text="Begin" OnClick="btnBegin_Click" CssClass="btn" />
                    <span>Remaining: <asp:Label ID="lblRemaining" runat="server" Text="0"></asp:Label></span>
                </div>

            <!-- Section 2: Add customer bill -->
                <h2>2) Add each customer's detailss </h2>
                <div class="input-row">
                    <asp:TextBox ID="txtConsumerNumber" runat="server" CssClass="input-text" Placeholder="Enter Consumer Number"></asp:TextBox>
                    <asp:TextBox ID="txtConsumerName" runat="server" CssClass="input-text" Placeholder="Enter Consumer Name"></asp:TextBox>
                    <asp:TextBox ID="txtUnits" runat="server" CssClass="input-text" Placeholder="Enter Units Consumed"></asp:TextBox>
                    <asp:Button ID="btnAddBill" runat="server" Text="Add Bill" OnClick="btnAddBill_Click" CssClass="btn" />
                </div>
                <asp:Label ID="lblMsg" runat="server" CssClass="msg-label"></asp:Label>

            <!-- Section 3: Retrieve last N bills -->
                <h2>3)Retrieve the last 'N' bills from the databasee</h2>
                <div class="input-row">
                    <asp:TextBox ID="txtLastN" runat="server" CssClass="input-text"></asp:TextBox>
                    <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" OnClick="btnRetrieve_Click" CssClass="btn" />
                    <br />
                    <br />
                </div>
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

            <!-- Logout Button -->
            <div class="section logout-section">
            </div>
        </div>
        <p style="margin-left: 560px">
                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" CssClass="btn logout-btn" />
            </p>
    </form>
</body>
</html>