<%@ Page Title="Cricket Tournament" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Public_Default" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .banner {
            background-color: #0066cc;
            color: white;
            padding: 40px 20px;
            text-align: center;
            margin-bottom: 20px;
        }
        
        .banner h1 {
            font-size: 36px;
            margin-bottom: 10px;
        }
        
        .banner p {
            font-size: 18px;
            margin-bottom: 20px;
        }
        
        .stats-container {
           display: -webkit-flex;
        display: -ms-flexbox;
            -webkit-justify-content: space-between;
            -ms-justify-content: space-between;
            flex-wrap: wrap;
            margin-bottom: 20px;
        }
        
        .stat-box {
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            padding: 20px;
            margin: 10px;
            text-align: center;
            width: 200px;
        }
        
        .stat-number {
            font-size: 36px;
            font-weight: bold;
            color: #0066cc;
            margin-bottom: 10px;
        }
        
        .stat-label {
            font-size: 16px;
            color: #666;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="banner">
        <h1>Cricket Tournament Management System</h1>
        <p>Welcome to the official website for cricket tournaments</p>
    </div>
    
    <div class="stats-container">
        <div class="stat-box">
            <div class="stat-number"><asp:Label ID="lblTournamentCount" runat="server" Text="0"></asp:Label></div>
            <div class="stat-label">Tournaments</div>
        </div>
        <div class="stat-box">
            <div class="stat-number"><asp:Label ID="lblTeamCount" runat="server" Text="0"></asp:Label></div>
            <div class="stat-label">Teams</div>
        </div>
        <div class="stat-box">
            <div class="stat-number"><asp:Label ID="lblMatchCount" runat="server" Text="0"></asp:Label></div>
            <div class="stat-label">Matches</div>
        </div>
        <div class="stat-box">
            <div class="stat-number"><asp:Label ID="lblPlayerCount" runat="server" Text="0"></asp:Label></div>
            <div class="stat-label">Players</div>
        </div>
    </div>
    
    <div class="card">
        <div class="card-title">Upcoming Matches</div>
        <asp:GridView ID="gvUpcomingMatches" runat="server" AutoGenerateColumns="False" CssClass="grid-view" EmptyDataText="No upcoming matches found.">
            <Columns>
                <asp:BoundField DataField="TournamentName" HeaderText="Tournament" />
                <asp:BoundField DataField="Team1" HeaderText="Team 1" />
                <asp:BoundField DataField="Team2" HeaderText="Team 2" />
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
                <asp:BoundField DataField="Team1" HeaderText="Team 1" />
                <asp:BoundField DataField="Team2" HeaderText="Team 2" />
                <asp:BoundField DataField="Result" HeaderText="Result" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>