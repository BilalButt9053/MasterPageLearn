<%@ Page Title="Manage Tournaments" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Admin_Tournaments" Codebehind="Tournaments.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Manage Tournaments</h2>
    
    <div class="card">
        <div class="card-title">Add New Tournament</div>
        <div class="form-group">
            <label for="txtTournamentName">Tournament Name:</label>
            <asp:TextBox ID="txtTournamentName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtYear">Year:</label>
            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" TextMode="Number" required="required"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnAddTournament" runat="server" Text="Add Tournament" CssClass="btn" OnClick="btnAddTournament_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnClear_Click" style="margin-left: 10px; background-color: #999;" />
        </div>
        <div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </div>
    
    <div class="card" style="margin-top: 20px;">
        <div class="card-title">All Tournaments</div>
        <asp:GridView ID="gvTournaments" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
            DataKeyNames="TournamentID" OnRowCommand="gvTournaments_RowCommand" OnRowEditing="gvTournaments_RowEditing"
            OnRowCancelingEdit="gvTournaments_RowCancelingEdit" OnRowUpdating="gvTournaments_RowUpdating"
            OnRowDeleting="gvTournaments_RowDeleting" EmptyDataText="No tournaments found.">
            <Columns>
                <asp:BoundField DataField="TournamentID" HeaderText="ID" ReadOnly="True" />
                <asp:TemplateField HeaderText="Tournament Name">
                    <ItemTemplate>
                        <asp:Label ID="lblTournamentName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditTournamentName" runat="server" Text='<%# Bind("Name") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Year">
                    <ItemTemplate>
                        <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditYear" runat="server" Text='<%# Bind("Year") %>' CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Matches">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkViewMatches" runat="server" CommandName="ViewMatches" CommandArgument='<%# Eval("TournamentID") %>'>View Matches</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Points Table">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkViewPointsTable" runat="server" CommandName="ViewPointsTable" CommandArgument='<%# Eval("TournamentID") %>'>View Points Table</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ButtonType="Link" />
                <asp:CommandField ShowDeleteButton="True" ButtonType="Link" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>