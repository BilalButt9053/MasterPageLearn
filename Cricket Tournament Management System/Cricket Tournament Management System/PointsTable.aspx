<%@ Page Title="Points Table" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Public_PointsTable" Codebehind="PointsTable.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Points Table</h2>
    
    <div class="card">
        <div class="card-title">Select Tournament</div>
        <div class="form-group">
            <asp:DropDownList ID="ddlTournament" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTournament_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    
    <asp:Panel ID="pnlPointsTable" runat="server" Visible="false" style="margin-top: 20px;">
        <div class="card">
            <div class="card-title">
                <asp:Label ID="lblTournamentName" runat="server"></asp:Label> - Points Table
            </div>
            <asp:GridView ID="gvPointsTable" runat="server" AutoGenerateColumns="False" CssClass="grid-view" EmptyDataText="No points data found for this tournament.">
                <Columns>
                    <asp:BoundField DataField="Position" HeaderText="Position" />
                    <asp:BoundField DataField="TeamName" HeaderText="Team" />
                    <asp:BoundField DataField="MatchesPlayed" HeaderText="Played" />
                    <asp:BoundField DataField="Wins" HeaderText="Won" />
                    <asp:BoundField DataField="Losses" HeaderText="Lost" />
                    <asp:BoundField DataField="Points" HeaderText="Points" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>