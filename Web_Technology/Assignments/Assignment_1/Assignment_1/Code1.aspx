<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Code1.aspx.cs" Inherits="Assignment_1.Code1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Validation Form</title>
    <style>
        .error { color: red; }
        .label { display: inline-block; width: 100px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h3>Insert your details:</h3>

        <div>
            <span class="label">Name:</span>
            <asp:TextBox ID="txtName" runat="server" />
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                ErrorMessage="*" CssClass="error" />
        </div>

        <div>
            <span class="label">Family Name:</span>
            <asp:TextBox ID="txtFamily" runat="server" />
            <asp:RequiredFieldValidator ID="rfvFamily" runat="server" ControlToValidate="txtFamily"
                ErrorMessage="*" CssClass="error" />
            <asp:CompareValidator ID="cmpName" runat="server" ControlToCompare="txtName"
                ControlToValidate="txtFamily" Operator="NotEqual" Type="String"
                ErrorMessage="differs from name" CssClass="error" />
        </div>

        <div>
            <span class="label">Address:</span>
            <asp:TextBox ID="txtAddress" runat="server" />
            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
                ErrorMessage="*" CssClass="error" />
            <asp:RegularExpressionValidator ID="revAddress" runat="server"
                ControlToValidate="txtAddress" ValidationExpression=".{2,}"
                ErrorMessage="at least 2 chars" CssClass="error" />
        </div>

        <div>
            <span class="label">City:</span>
            <asp:TextBox ID="txtCity" runat="server" />
            <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                ErrorMessage="*" CssClass="error" />
            <asp:RegularExpressionValidator ID="revCity" runat="server"
                ControlToValidate="txtCity" ValidationExpression=".{2,}"
                ErrorMessage="at least 2 chars" CssClass="error" />
        </div>

        <div>
            <span class="label">Zip Code:</span>
            <asp:TextBox ID="txtZip" runat="server" />
            <asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtZip"
                ErrorMessage="*" CssClass="error" />
            <asp:RegularExpressionValidator ID="revZip" runat="server"
                ControlToValidate="txtZip" ValidationExpression="^\d{5}$"
                ErrorMessage="(xxxxx)" CssClass="error" />
        </div>

        <div>
            <span class="label">Phone:</span>
            <asp:TextBox ID="txtPhone" runat="server" />
            <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                ErrorMessage="*" CssClass="error" />
            <asp:RegularExpressionValidator ID="revPhone" runat="server"
                ControlToValidate="txtPhone"
                ValidationExpression="^\d{2,3}-\d{7}$"
                ErrorMessage="(xx-xxxxxxx / xxx-xxxxxxx)" CssClass="error" />
        </div>

        <div>
            <span class="label">E-Mail:</span>
            <asp:TextBox ID="txtEmail" runat="server" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="*" CssClass="error" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                ControlToValidate="txtEmail"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                ErrorMessage="Invalid email" CssClass="error" />
        </div>

        <br />
        <asp:Button ID="btnCheck" runat="server" Text="Check" />

        <br /><br />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
            HeaderText="ValidationSum" CssClass="error" ShowMessageBox="True" ShowSummary="False" />

    </form>
</body>
</html>