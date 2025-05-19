<%@ Page Language="C#" AutoEventWireup="true" Inherits="Register" Codebehind="Register.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Register - Cricket Tournament Management</title>
    <link href="styles.css" rel="stylesheet" type="text/css" />
    <style>
        .register-container {
            max-width: 500px;
            margin: 50px auto;
            padding: 20px;
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }
        .register-title {
            text-align: center;
            margin-bottom: 20px;
            color: #0066cc;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-container">
            <h2 class="register-title">Register as Team Manager</h2>
            <div class="form-group">
                <label for="txtUsername">Username:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtConfirmPassword">Confirm Password:</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" ></asp:TextBox>
                <asp:CompareValidator ID="cvPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword" 
                    ControlToCompare="txtPassword" 
                    ErrorMessage="Passwords do not match" 
                    ForeColor="Red">
                </asp:CompareValidator>
            </div>
            <div class="form-group">
                <label for="txtTeamName">Team Name:</label>
                <asp:TextBox ID="txtTeamName" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn" OnClick="btnRegister_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" style="margin-left: 10px; background-color: #999;" />
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </div>
            <div style="margin-top: 15px; text-align: center;">
                <a href="Login.aspx">Already have an account? Login</a>
            </div>
        </div>
    </form>
</body>
</html>