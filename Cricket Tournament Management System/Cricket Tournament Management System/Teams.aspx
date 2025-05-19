<%@ Page Title="Manage Teams" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Admin_Teams" Codebehind="Teams.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Manage Teams</h2>
    
    <div class="card">
        <div class="card-title">Add New Team</div>
        <div class="form-group">
            <label for="txtTeamName">Team Name:</label>
            <asp:TextBox ID="txtTeamName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="ddlManager" runat="server">Manager:</label>
            <asp:DropDownList ID="ddlManager" runat="server">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnAddTeam" runat="server" Text="Add Team" CssClass="btn" OnClick="btnAddTeam_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnClear_Click" style="margin-left: 10px; background-color: #999;" />
        </div>
        <div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </div>
    
    <div class="card" style="margin-top: 20px;">
        <div class="card-title">All Teams</div>
        <asp:GridView ID="gvTeams" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
            DataKeyNames="TeamID" OnRowCommand="gvTeams_RowCommand" OnRowEditing="gvTeams_RowEditing"
            OnRowCancelingEdit="gvTeams_RowCancelingEdit" OnRowUpdating="gvTeams_RowUpdating"
            OnRowDeleting="gvTeams_RowDeleting" EmptyDataText="No teams found.">
            <Columns>
                <asp:BoundField DataField="TeamID" HeaderText="ID" ReadOnly="True" />
                <asp:TemplateField HeaderText="Team Name">
                    <ItemTemplate>
                        <asp:Label ID="lblTeamName" runat="server" Text='<%# Eval("TeamName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditTeamName" runat="server" Text='<%# Bind("TeamName") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Manager">
                    <ItemTemplate>
                        <asp:Label ID="lblManager" runat="server" Text='<%# Eval("ManagerName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEditManager" runat="server" CssClass="form-control"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Players">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkViewPlayers" runat="server" CommandName="ViewPlayers" CommandArgument='<%# Eval("TeamID") %>'>View Players</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ButtonType="Link" />
                <asp:CommandField ShowDeleteButton="True" ButtonType="Link" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>