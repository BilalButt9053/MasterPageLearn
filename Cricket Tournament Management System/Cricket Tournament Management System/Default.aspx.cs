using System;
using System.Data;

public partial class Public_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadStats();
            LoadUpcomingMatches();
            LoadRecentResults();
        }
    }

    private void LoadStats()
    {
        // Get tournament count
        object tournamentCount = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Tournaments");
        lblTournamentCount.Text = tournamentCount.ToString();

        // Get team count
        object teamCount = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Teams");
        lblTeamCount.Text = teamCount.ToString();

        // Get match count
        object matchCount = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Matches");
        lblMatchCount.Text = matchCount.ToString();

        // Get player count
        object playerCount = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Players");
        lblPlayerCount.Text = playerCount.ToString();
    }

    private void LoadUpcomingMatches()
    {
        string query = @"
            SELECT TOP 5 t.Name + ' ' + CAST(t.Year AS VARCHAR) AS TournamentName,
                   t1.TeamName AS Team1, t2.TeamName AS Team2,
                   m.DateTime, m.Venue
            FROM Matches m
            INNER JOIN Tournaments t ON m.TournamentID = t.TournamentID
            INNER JOIN Teams t1 ON m.Team1ID = t1.TeamID
            INNER JOIN Teams t2 ON m.Team2ID = t2.TeamID
            WHERE m.DateTime > GETDATE()
            ORDER BY m.DateTime";

        DataTable dt = DBConnection.ExecuteQuery(query);
        gvUpcomingMatches.DataSource = dt;
        gvUpcomingMatches.DataBind();
    }

    private void LoadRecentResults()
    {
        string query = @"
            SELECT TOP 5 t.Name + ' ' + CAST(t.Year AS VARCHAR) AS TournamentName,
                   t1.TeamName AS Team1, t2.TeamName AS Team2,
                   CASE 
                       WHEN m.WinnerTeamID = m.Team1ID THEN t1.TeamName + ' won'
                       WHEN m.WinnerTeamID = m.Team2ID THEN t2.TeamName + ' won'
                       WHEN m.WinnerTeamID = 0 THEN 'Draw'
                       ELSE 'Not played yet'
                   END AS Result
            FROM Matches m
            INNER JOIN Tournaments t ON m.TournamentID = t.TournamentID
            INNER JOIN Teams t1 ON m.Team1ID = t1.TeamID
            INNER JOIN Teams t2 ON m.Team2ID = t2.TeamID
            WHERE m.WinnerTeamID IS NOT NULL
            ORDER BY m.DateTime DESC";

        DataTable dt = DBConnection.ExecuteQuery(query);
        gvRecentResults.DataSource = dt;
        gvRecentResults.DataBind();
    }
}