using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class PublicTeams : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadTeams();
        }
    }

    private void LoadTeams()
    {
        string query = @"
            SELECT t.TeamID, t.TeamName, 
                   ISNULL(u.Username, 'Not Assigned') AS ManagerName,
                   (SELECT COUNT(*) FROM Players p WHERE p.TeamID = t.TeamID) AS PlayerCount
            FROM Teams t
            LEFT JOIN Users u ON t.ManagerID = u.UserID
            ORDER BY t.TeamName";

        DataTable dt = DBConnection.ExecuteQuery(query);
        rptTeams.DataSource = dt;
        rptTeams.DataBind();
    }

    protected void rptTeams_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "ViewPlayers")
        {
            int teamId = Convert.ToInt32(e.CommandArgument);
            LoadTeamPlayers(teamId);
        }
    }

    private void LoadTeamPlayers(int teamId)
    {
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TeamID", teamId)
        };

        // Get team name
        string teamQuery = "SELECT TeamName FROM Teams WHERE TeamID = @TeamID";
        DataTable dtTeam = DBConnection.ExecuteQuery(teamQuery, parameters);

        if (dtTeam.Rows.Count > 0)
        {
            lblTeamName.Text = dtTeam.Rows[0]["TeamName"].ToString();
        }

        // Get players
        string playersQuery = "SELECT Name, Role FROM Players WHERE TeamID = @TeamID ORDER BY Name";
        DataTable dtPlayers = DBConnection.ExecuteQuery(playersQuery, parameters);

        gvPlayers.DataSource = dtPlayers;
        gvPlayers.DataBind();

        pnlPlayers.Visible = true;
    }
}