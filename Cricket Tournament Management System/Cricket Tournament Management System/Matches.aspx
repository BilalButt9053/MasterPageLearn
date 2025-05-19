<%@ Page Title="Manage Matches" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="Admin_Matches" Codebehind="Matches.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Manage Matches</h2>
    
    <div class="card">
        <div class="form-group">
    <label for="ddlTournament">Tournament:</label>
  <asp:DropDownList ID="ddlTournament" runat="server" CssClass="form-control">
    <asp:ListItem Text="--Select Tournament--" Value=""></asp:ListItem>
</asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvTournament" runat="server" ControlToValidate="ddlTournament"
        InitialValue="" ErrorMessage="Please select a tournament." Display="Dynamic" ForeColor="Red" />
</div>

<div class="form-group">
    <label for="ddlTeam1">Team 1:</label>
    <asp:DropDownList ID="ddlTeam1" runat="server" CssClass="form-control">
        <asp:ListItem Text="--Select Team 1--" Value=""></asp:ListItem>
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvTeam1" runat="server" ControlToValidate="ddlTeam1"
        InitialValue="" ErrorMessage="Please select Team 1." Display="Dynamic" ForeColor="Red" />
</div>

<div class="form-group">
    <label for="ddlTeam2">Team 2:</label>
    <asp:DropDownList ID="ddlTeam2" runat="server" CssClass="form-control">
        <asp:ListItem Text="--Select Team 2--" Value=""></asp:ListItem>
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvTeam2" runat="server" ControlToValidate="ddlTeam2"
        InitialValue="" ErrorMessage="Please select Team 2." Display="Dynamic" ForeColor="Red" />
</div>

<div class="form-group">
    <label for="txtDateTime">Date & Time:</label>
    <asp:TextBox ID="txtDateTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvDateTime" runat="server" ControlToValidate="txtDateTime"
        ErrorMessage="Please enter date & time." Display="Dynamic" ForeColor="Red" />
</div>

<div class="form-group">
    <label for="txtVenue">Venue:</label>
    <asp:TextBox ID="txtVenue" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvVenue" runat="server" ControlToValidate="txtVenue"
        ErrorMessage="Please enter venue." Display="Dynamic" ForeColor="Red" />
</div>

        <asp:Label ID="lblMessage" runat="server"></asp:Label>

    </div>
    <div class="form-group">
    <asp:Button ID="btnAddMatch" runat="server" Text="Add Match" CssClass="btn btn-primary" OnClick="btnAddMatch_Click" />
    <asp:Button ID="btnClear" runat="server" Text="Clear Form" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
</div>
    
    <div class="card" style="margin-top: 20px;">
        <div class="card-title">All Matches</div>
        <asp:GridView ID="gvMatches" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
            DataKeyNames="MatchID" OnRowCommand="gvMatches_RowCommand" EmptyDataText="No matches found.">
            <Columns>
                <asp:BoundField DataField="MatchID" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="TournamentName" HeaderText="Tournament" />
                <asp:BoundField DataField="Team1" HeaderText="Team 1" />
                <asp:BoundField DataField="Team2" HeaderText="Team 2" />
                <asp:BoundField DataField="DateTime" HeaderText="Date & Time" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="Venue" HeaderText="Venue" />
                <asp:TemplateField HeaderText="Result">
    <ItemTemplate>
        <asp:Label ID="lblResult" runat="server" 
            Text='<%# ((Admin_Matches)Page).GetResultText(Eval("Team1Score"), Eval("Team1Wickets"), Eval("Team2Score"), Eval("Team2Wickets"), Eval("WinnerTeamID"), Eval("Team1ID"), Eval("Team2ID"), Eval("Team1"), Eval("Team2")) %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkUpdateResult" runat="server" CommandName="UpdateResult" CommandArgument='<%# Eval("MatchID") %>'>Update Result</asp:LinkButton>
                        |
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteMatch" CommandArgument='<%# Eval("MatchID") %>' OnClientClick="return confirm('Are you sure you want to delete this match?');">Delete</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>