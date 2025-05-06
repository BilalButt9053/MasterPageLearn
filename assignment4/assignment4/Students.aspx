
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="assignment4.Students" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Information System</title>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        table {
            border-collapse: collapse;
            margin-bottom: 20px;
        }
        th, td {
            padding: 8px;
            text-align: left;
            border-bottom: 1px solid #ddd;
        }
        th {
            background-color: #f2f2f2;
        }
        .dropdown {
            width: 200px;
            padding: 5px;
            margin: 5px 0;
        }
        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        .gridview th {
            background-color: #4CAF50;
            color: white;
        }
        .gridview tr:nth-child(even) {
            background-color: #f2f2f2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Cascading Dropdown List Demo</h2>
            
            <table>
                <tr>
                    <th>Select Country</th>
                    <td>
                        <asp:DropDownList ID="DropDownList2" runat="server" 
                            AutoPostBack="True"
                            OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged"
                            CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Select State</th>
                    <td>
                        <asp:DropDownList ID="DropDownList3" runat="server" 
                            AutoPostBack="True"
                            OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged"
                            CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Select City</th>
                    <td>
                        <asp:DropDownList ID="DropDownList4" runat="server" 
                            AutoPostBack="True"
                            OnSelectedIndexChanged="DropDownList4_SelectedIndexChanged"
                            CssClass="dropdown">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <h3>Student Records</h3>
    
        <asp:GridView ID="gvStudents" runat="server" style="margin-left: 0px">
        </asp:GridView>
    
        

    </div>
    </form>
</body>
</html>
