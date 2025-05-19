using System;
using System.Data;

public partial class Admin_Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
            }

            LoadDashboardData();
        }
    }

    private void LoadDashboardData()
    {
        // Get team count
        object teamCount = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Teams");
        lblTeamCount.Text = teamCount.ToString();

        // Get tournament count
        object tournamentCount = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Tournaments");
        lblTournamentCount.Text = tournamentCount.ToString();

        // Get match count
        object matchCount = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Matches");
        lblMatchCount.Text = matchCount.ToString();

        // Get recent matches
        string query = @"
            SELECT TOP 5 m.MatchID, t.Name AS TournamentName, 
                   t1.TeamName AS Team1, t2.TeamName AS Team2, 
                   m.DateTime, m.Venue,
                   CASE 
                       WHEN m.WinnerTeamID IS NULL THEN 'Not played yet'
                       WHEN m.WinnerTeamID = m.Team1ID THEN t1.TeamName + ' won'
                       WHEN m.WinnerTeamID = m.Team2ID THEN t2.TeamName + ' won'
                       ELSE 'Draw'
                   END AS Result
            FROM Matches m
            INNER JOIN Tournaments t ON m.TournamentID = t.TournamentID
            INNER JOIN Teams t1 ON m.Team1ID = t1.TeamID
            INNER JOIN Teams t2 ON m.Team2ID = t2.TeamID
            ORDER BY m.DateTime DESC";

        DataTable dtMatches = DBConnection.ExecuteQuery(query);
        gvRecentMatches.DataSource = dtMatches;
        gvRecentMatches.DataBind();
    }

    protected void btnManageTeams_Click(object sender, EventArgs e)
    {
        Response.Redirect("Teams.aspx");
    }

    protected void btnManageTournaments_Click(object sender, EventArgs e)
    {
        Response.Redirect("Tournaments.aspx");
    }

    protected void btnManageMatches_Click(object sender, EventArgs e)
    {
        Response.Redirect("Matches.aspx");
    }
}