using System;
using System.Data;
using System.Data.SqlClient;

public partial class Public_PointsTable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadTournaments();

            // Check if TournamentID is passed in query string
            if (!string.IsNullOrEmpty(Request.QueryString["TournamentID"]))
            {
                string tournamentId = Request.QueryString["TournamentID"];
                ddlTournament.SelectedValue = tournamentId;
                LoadPointsTable(Convert.ToInt32(tournamentId));
            }
        }
    }

    private void LoadTournaments()
    {
        string query = "SELECT TournamentID, Name + ' ' + CAST(Year AS VARCHAR) AS TournamentName FROM Tournaments ORDER BY Year DESC, Name";
        DataTable dt = DBConnection.ExecuteQuery(query);

        ddlTournament.DataSource = dt;
        ddlTournament.DataTextField = "TournamentName";
        ddlTournament.DataValueField = "TournamentID";
        ddlTournament.DataBind();

        ddlTournament.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Tournament --", ""));
    }

    protected void ddlTournament_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlTournament.SelectedValue))
        {
            int tournamentId = Convert.ToInt32(ddlTournament.SelectedValue);
            LoadPointsTable(tournamentId);
        }
        else
        {
            pnlPointsTable.Visible = false;
        }
    }

    private void LoadPointsTable(int tournamentId)
    {
        // Get tournament name
        SqlParameter[] tournamentParams = new SqlParameter[]
        {
            new SqlParameter("@TournamentID", tournamentId)
        };

        string tournamentQuery = "SELECT Name + ' ' + CAST(Year AS VARCHAR) AS TournamentName FROM Tournaments WHERE TournamentID = @TournamentID";
        DataTable dtTournament = DBConnection.ExecuteQuery(tournamentQuery, tournamentParams);

        if (dtTournament.Rows.Count > 0)
        {
            lblTournamentName.Text = dtTournament.Rows[0]["TournamentName"].ToString();
        }

        // Get points table
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TournamentID", tournamentId)
        };

        string query = @"
            SELECT ROW_NUMBER() OVER (ORDER BY pt.Points DESC, pt.Wins DESC) AS Position,
                   t.TeamName, pt.MatchesPlayed, pt.Wins, pt.Losses, pt.Points
            FROM PointsTable pt
            INNER JOIN Teams t ON pt.TeamID = t.TeamID
            WHERE pt.TournamentID = @TournamentID
            ORDER BY pt.Points DESC, pt.Wins DESC";

        DataTable dt = DBConnection.ExecuteQuery(query, parameters);
        gvPointsTable.DataSource = dt;
        gvPointsTable.DataBind();

        pnlPointsTable.Visible = true;
    }
}