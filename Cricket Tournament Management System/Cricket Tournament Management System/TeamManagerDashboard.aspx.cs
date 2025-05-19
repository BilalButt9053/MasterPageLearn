using System;
using System.Data;
using System.Data.SqlClient;

public partial class TeamManagerDashboard : System.Web.UI.Page
{
    private int userId;
    private int teamId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "TeamManager")
            {
                Response.Redirect("~/Login.aspx");
            }

            userId = Convert.ToInt32(Session["UserID"]);
            LoadTeamInfo();
            LoadUpcomingMatches();
            LoadRecentResults();
        }
    }

    private void LoadTeamInfo()
    {
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@ManagerID", userId)
        };

        string query = "SELECT TeamID, TeamName FROM Teams WHERE ManagerID = @ManagerID";
        DataTable dt = DBConnection.ExecuteQuery(query, parameters);

        if (dt.Rows.Count > 0)
        {
            teamId = Convert.ToInt32(dt.Rows[0]["TeamID"]);
            lblTeamName.Text = dt.Rows[0]["TeamName"].ToString();

            // Get player count
            SqlParameter[] playerParams = new SqlParameter[]
            {
                new SqlParameter("@TeamID", teamId)
            };

            object playerCount = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Players WHERE TeamID = @TeamID", playerParams);
            lblPlayerCount.Text = playerCount.ToString();
        }
        else
        {
            Response.Redirect("~/Login.aspx");
        }
    }

    private void LoadUpcomingMatches()
    {
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TeamID", teamId)
        };

        string query = @"
            SELECT TOP 5 t.Name + ' ' + CAST(t.Year AS VARCHAR) AS TournamentName,
                   CASE 
                       WHEN m.Team1ID = @TeamID THEN t2.TeamName
                       ELSE t1.TeamName
                   END AS OpponentTeam,
                   m.DateTime, m.Venue
            FROM Matches m
            INNER JOIN Tournaments t ON m.TournamentID = t.TournamentID
            INNER JOIN Teams t1 ON m.Team1ID = t1.TeamID
            INNER JOIN Teams t2 ON m.Team2ID = t2.TeamID
            WHERE (m.Team1ID = @TeamID OR m.Team2ID = @TeamID)
              AND m.DateTime > GETDATE()
            ORDER BY m.DateTime";

        DataTable dt = DBConnection.ExecuteQuery(query, parameters);
        gvUpcomingMatches.DataSource = dt;
        gvUpcomingMatches.DataBind();
    }

    private void LoadRecentResults()
    {
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TeamID", teamId)
        };

        string query = @"
            SELECT TOP 5 t.Name + ' ' + CAST(t.Year AS VARCHAR) AS TournamentName,
                   CASE 
                       WHEN m.Team1ID = @TeamID THEN t2.TeamName
                       ELSE t1.TeamName
                   END AS OpponentTeam,
                   CASE
                       WHEN m.WinnerTeamID = @TeamID THEN 'Won'
                       WHEN m.WinnerTeamID IS NULL THEN 'Not played'
                       WHEN m.WinnerTeamID = 0 THEN 'Draw'
                       ELSE 'Lost'
                   END AS Result,
                   CASE
                       WHEN m.Team1ID = @TeamID THEN 
                           CAST(m.Team1Score AS VARCHAR) + 
                           CASE WHEN m.Team1Wickets IS NOT NULL THEN '/' + CAST(m.Team1Wickets AS VARCHAR) ELSE '' END +
                           ' vs ' +
                           CAST(m.Team2Score AS VARCHAR) + 
                           CASE WHEN m.Team2Wickets IS NOT NULL THEN '/' + CAST(m.Team2Wickets AS VARCHAR) ELSE '' END
                       ELSE 
                           CAST(m.Team2Score AS VARCHAR) + 
                           CASE WHEN m.Team2Wickets IS NOT NULL THEN '/' + CAST(m.Team2Wickets AS VARCHAR) ELSE '' END +
                           ' vs ' +
                           CAST(m.Team1Score AS VARCHAR) + 
                           CASE WHEN m.Team1Wickets IS NOT NULL THEN '/' + CAST(m.Team1Wickets AS VARCHAR) ELSE '' END
                   END AS Score
            FROM Matches m
            INNER JOIN Tournaments t ON m.TournamentID = t.TournamentID
            INNER JOIN Teams t1 ON m.Team1ID = t1.TeamID
            INNER JOIN Teams t2 ON m.Team2ID = t2.TeamID
            WHERE (m.Team1ID = @TeamID OR m.Team2ID = @TeamID)
              AND m.WinnerTeamID IS NOT NULL
            ORDER BY m.DateTime DESC";

        DataTable dt = DBConnection.ExecuteQuery(query, parameters);
        gvRecentResults.DataSource = dt;
        gvRecentResults.DataBind();
    }

    protected void btnManagePlayers_Click(object sender, EventArgs e)
    {
        Response.Redirect("Players.aspx");
    }
}