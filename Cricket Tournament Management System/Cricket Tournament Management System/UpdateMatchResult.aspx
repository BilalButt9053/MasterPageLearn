<%@ Page Title="Update Match Result" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Admin_UpdateMatchResult" Codebehind="UpdateMatchResult.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Update Match Result</h2>
    
    <div class="card">
        <div class="card-title">Match Details</div>
        <div class="form-group">
            <label>Tournament:</label>
            <asp:Label ID="lblTournament" runat="server" CssClass="form-control" style="background-color: #f9f9f9;"></asp:Label>
        </div>
        <div class="form-group">
            <label>Team 1:</label>
            <asp:Label ID="lblTeam1" runat="server" CssClass="form-control" style="background-color: #f9f9f9;"></asp:Label>
        </div>
        <div class="form-group">
            <label>Team 2:</label>
            <asp:Label ID="lblTeam2" runat="server" CssClass="form-control" style="background-color: #f9f9f9;"></asp:Label>
        </div>
        <div class="form-group">
            <label>Date & Time:</label>
            <asp:Label ID="lblDateTime" runat="server" CssClass="form-control" style="background-color: #f9f9f9;"></asp:Label>
        </div>
        <div class="form-group">
            <label>Venue:</label>
            <asp:Label ID="lblVenue" runat="server" CssClass="form-control" style="background-color: #f9f9f9;"></asp:Label>
        </div>
    </div>
    
    <div class="card" style="margin-top: 20px;">
        <div class="card-title">Update Result</div>
        <div class="form-group">
            <label for="txtTeam1Score">Team 1 Score:</label>
            <asp:TextBox ID="txtTeam1Score" runat="server" CssClass="form-control" TextMode="Number" required="required"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtTeam1Wickets">Team 1 Wickets:</label>
            <asp:TextBox ID="txtTeam1Wickets" runat="server" CssClass="form-control" TextMode="Number" min="0" max="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtTeam2Score">Team 2 Score:</label>
            <asp:TextBox ID="txtTeam2Score" runat="server" CssClass="form-control" TextMode="Number" required="required"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtTeam2Wickets">Team 2 Wickets:</label>
            <asp:TextBox ID="txtTeam2Wickets" runat="server" CssClass="form-control" TextMode="Number" min="0" max="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="ddlWinner">Winner:</label>
            <asp:DropDownList ID="ddlWinner" runat="server" CssClass="form-control" required="required">
                <asp:ListItem Value="" Text="-- Select Winner --" />
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnUpdateResult" runat="server" Text="Update Result" CssClass="btn" OnClick="btnUpdateResult_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" OnClick="btnCancel_Click" style="margin-left: 10px; background-color: #999;" />
        </div>
        <div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>