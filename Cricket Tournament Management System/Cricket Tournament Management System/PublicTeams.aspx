<%@ Page Title="Teams" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="PublicTeams" Codebehind="PublicTeams.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .team-container {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            margin-top: 20px;
        }
        
        .team-card {
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            padding: 20px;
            width: calc(33.33% - 20px);
            min-width: 250px;
        }
        
        .team-name {
            font-size: 18px;
            font-weight: bold;
            color: #0066cc;
            margin-bottom: 10px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eee;
        }
        
        .team-manager {
            font-size: 14px;
            color: #666;
            margin-bottom: 15px;
        }
        
        .view-players {
            display: inline-block;
            padding: 5px 10px;
            background-color: #0066cc;
            color: white;
            text-decoration: none;
            border-radius: 3px;
            font-size: 14px;
        }
        
        .view-players:hover {
            background-color: #0052a3;
        }
        
        @media (max-width: 768px) {
            .team-card {
                width: 100%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Teams</h2>
    
    <div class="team-container">
        <asp:Repeater ID="rptTeams" runat="server" OnItemCommand="rptTeams_ItemCommand">
            <ItemTemplate>
                <div class="team-card">
                    <div class="team-name"><%# Eval("TeamName") %></div>
                    <div class="team-manager">Manager: <%# Eval("ManagerName") %></div>
                    <div class="team-manager">Players: <%# Eval("PlayerCount") %></div>
                    <asp:LinkButton ID="lnkViewPlayers" runat="server" CssClass="view-players" 
                        CommandName="ViewPlayers" CommandArgument='<%# Eval("TeamID") %>'>
                        View Players
                    </asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
    <asp:Panel ID="pnlPlayers" runat="server" Visible="false" style="margin-top: 30px;">
        <div class="card">
            <div class="card-title">
                <asp:Label ID="lblTeamName" runat="server"></asp:Label> - Players
            </div>
            <asp:GridView ID="gvPlayers" runat="server" AutoGenerateColumns="False" CssClass="grid-view" EmptyDataText="No players found.">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Player Name" />
                    <asp:BoundField DataField="Role" HeaderText="Role" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>