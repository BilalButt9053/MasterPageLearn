<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Admin_Dashboard" Codebehind="Dashboard.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Admin Dashboard</h2>
    
    <div class="row">
        <div class="card" style="width: 30%; float: left; margin-right: 2%;">
            <div class="card-title">Teams</div>
            <div style="font-size: 24px; text-align: center; margin: 20px 0;">
                <asp:Label ID="lblTeamCount" runat="server" Text="0"></asp:Label>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="btnManageTeams" runat="server" Text="Manage Teams" CssClass="btn" OnClick="btnManageTeams_Click" />
            </div>
        </div>
        
        <div class="card" style="width: 30%; float: left; margin-right: 2%;">
            <div class="card-title">Tournaments</div>
            <div style="font-size: 24px; text-align: center; margin: 20px 0;">
                <asp:Label ID="lblTournamentCount" runat="server" Text="0"></asp:Label>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="btnManageTournaments" runat="server" Text="Manage Tournaments" CssClass="btn" OnClick="btnManageTournaments_Click" />
            </div>
        </div>
        
        <div class="card" style="width: 30%; float: left;">
            <div class="card-title">Matches</div>
            <div style="font-size: 24px; text-align: center; margin: 20px 0;">
                <asp:Label ID="lblMatchCount" runat="server" Text="0"></asp:Label>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="btnManageMatches" runat="server" Text="Manage Matches" CssClass="btn" OnClick="btnManageMatches_Click" />
            </div>
        </div>
    </div>
    
    <div style="clear: both; margin-top: 20px;">
        <div class="card">
            <div class="card-title">Recent Matches</div>
            <asp:GridView ID="gvRecentMatches" runat="server" AutoGenerateColumns="False" CssClass="grid-view" EmptyDataText="No recent matches found.">
                <Columns>
                    <asp:BoundField DataField="MatchID" HeaderText="ID" />
                    <asp:BoundField DataField="TournamentName" HeaderText="Tournament" />
                    <asp:BoundField DataField="Team1" HeaderText="Team 1" />
                    <asp:BoundField DataField="Team2" HeaderText="Team 2" />
                    <asp:BoundField DataField="DateTime" HeaderText="Date & Time" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="Venue" HeaderText="Venue" />
                    <asp:BoundField DataField="Result" HeaderText="Result" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>