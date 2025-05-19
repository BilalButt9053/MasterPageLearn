<%@ Page Title="Team Manager Dashboard" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="TeamManagerDashboard" Codebehind="TeamManagerDashboard.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Team Manager Dashboard</h2>
    
    <div class="card">
        <div class="card-title">Your Team</div>
        <div class="form-group">
            <label>Team Name:</label>
            <asp:Label ID="lblTeamName" runat="server" CssClass="form-control" style="background-color: #f9f9f9;"></asp:Label>
        </div>
        <div class="form-group">
            <label>Total Players:</label>
            <asp:Label ID="lblPlayerCount" runat="server" CssClass="form-control" style="background-color: #f9f9f9;"></asp:Label>
        </div>
        <div class="form-group">
            <asp:Button ID="btnManagePlayers" runat="server" Text="Manage Players" CssClass="btn" OnClick="btnManagePlayers_Click" />
        </div>
    </div>
    
    <div class="card" style="margin-top: 20px;">
        <div class="card-title">Upcoming Matches</div>
        <asp:GridView ID="gvUpcomingMatches" runat="server" AutoGenerateColumns="False" CssClass="grid-view" EmptyDataText="No upcoming matches found.">
            <Columns>
                <asp:BoundField DataField="TournamentName" HeaderText="Tournament" />
                <asp:BoundField DataField="OpponentTeam" HeaderText="Opponent" />
                <asp:BoundField DataField="DateTime" HeaderText="Date & Time" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="Venue" HeaderText="Venue" />
            </Columns>
        </asp:GridView>
    </div>
    
    <div class="card" style="margin-top: 20px;">
        <div class="card-title">Recent Results</div>
        <asp:GridView ID="gvRecentResults" runat="server" AutoGenerateColumns="False" CssClass="grid-view" EmptyDataText="No recent results found.">
            <Columns>
                <asp:BoundField DataField="TournamentName" HeaderText="Tournament" />
                <asp:BoundField DataField="OpponentTeam" HeaderText="Opponent" />
                <asp:BoundField DataField="Result" HeaderText="Result" />
                <asp:BoundField DataField="Score" HeaderText="Score" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>