<%@ Page Title="Manage Players" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="TeamManager_Players" Codebehind="Players.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Manage Players</h2>
    
    <div class="card">
        <div class="card-title">Add New Player</div>
        <div class="form-group">
            <label for="txtPlayerName">Player Name:</label>
            <asp:TextBox ID="txtPlayerName" runat="server" CssClass="form-control" ></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="ddlRole">Role:</label>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" >
                <asp:ListItem Value="" Text="-- Select Role --" />
                <asp:ListItem Value="Batsman" Text="Batsman" />
                <asp:ListItem Value="Bowler" Text="Bowler" />
                <asp:ListItem Value="All-rounder" Text="All-rounder" />
                <asp:ListItem Value="Wicket-keeper" Text="Wicket-keeper" />
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnAddPlayer" runat="server" Text="Add Player" CssClass="btn" OnClick="btnAddPlayer_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnClear_Click" style="margin-left: 10px; background-color: #999;" />
        </div>
        <div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </div>
    
    <div class="card" style="margin-top: 20px;">
        <div class="card-title">Team Players</div>
        <asp:GridView ID="gvPlayers" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
            DataKeyNames="PlayerID" OnRowEditing="gvPlayers_RowEditing" OnRowCancelingEdit="gvPlayers_RowCancelingEdit"
            OnRowUpdating="gvPlayers_RowUpdating" OnRowDeleting="gvPlayers_RowDeleting" EmptyDataText="No players found.">
            <Columns>
                <asp:BoundField DataField="PlayerID" HeaderText="ID" ReadOnly="True" />
                <asp:TemplateField HeaderText="Player Name">
                    <ItemTemplate>
                        <asp:Label ID="lblPlayerName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditPlayerName" runat="server" Text='<%# Bind("Name") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <asp:Label ID="lblRole" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEditRole" runat="server" CssClass="form-control" SelectedValue='<%# Bind("Role") %>'>
                            <asp:ListItem Value="Batsman" Text="Batsman" />
                            <asp:ListItem Value="Bowler" Text="Bowler" />
                            <asp:ListItem Value="All-rounder" Text="All-rounder" />
                            <asp:ListItem Value="Wicket-keeper" Text="Wicket-keeper" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ButtonType="Link" />
                <asp:CommandField ShowDeleteButton="True" ButtonType="Link" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>