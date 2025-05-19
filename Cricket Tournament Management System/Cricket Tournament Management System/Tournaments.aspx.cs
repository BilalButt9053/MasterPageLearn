using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Admin_Tournaments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
            }

            // Set default year to current year
            txtYear.Text = DateTime.Now.Year.ToString();

            LoadTournaments();
        }
    }

    private void LoadTournaments()
    {
        string query = "SELECT TournamentID, Name, Year FROM Tournaments ORDER BY Year DESC, Name";
        DataTable dt = DBConnection.ExecuteQuery(query);
        gvTournaments.DataSource = dt;
        gvTournaments.DataBind();
    }

    protected void btnAddTournament_Click(object sender, EventArgs e)
    {
        string tournamentName = txtTournamentName.Text.Trim();
        int year;

        if (string.IsNullOrEmpty(tournamentName))
        {
            lblMessage.Text = "Please enter tournament name.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (!int.TryParse(txtYear.Text, out year))
        {
            lblMessage.Text = "Please enter a valid year.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@Name", tournamentName),
            new SqlParameter("@Year", year)
        };

        int result = DBConnection.ExecuteNonQuery("INSERT INTO Tournaments (Name, Year) VALUES (@Name, @Year)", parameters);

        if (result > 0)
        {
            lblMessage.Text = "Tournament added successfully.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
            txtTournamentName.Text = "";
            txtYear.Text = DateTime.Now.Year.ToString();
            LoadTournaments();
        }
        else
        {
            lblMessage.Text = "Failed to add tournament.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtTournamentName.Text = "";
        txtYear.Text = DateTime.Now.Year.ToString();
        lblMessage.Text = "";
    }

    protected void gvTournaments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewMatches")
        {
            int tournamentId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("Matches.aspx?TournamentID=" + tournamentId);
        }
        else if (e.CommandName == "ViewPointsTable")
        {
            int tournamentId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("PointsTable.aspx?TournamentID=" + tournamentId);
        }
    }

    protected void gvTournaments_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvTournaments.EditIndex = e.NewEditIndex;
        LoadTournaments();
    }

    protected void gvTournaments_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvTournaments.EditIndex = -1;
        LoadTournaments();
    }

    protected void gvTournaments_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int tournamentId = Convert.ToInt32(gvTournaments.DataKeys[e.RowIndex].Value);
        TextBox txtEditTournamentName = (TextBox)gvTournaments.Rows[e.RowIndex].FindControl("txtEditTournamentName");
        TextBox txtEditYear = (TextBox)gvTournaments.Rows[e.RowIndex].FindControl("txtEditYear");

        string tournamentName = txtEditTournamentName.Text.Trim();
        int year;

        if (string.IsNullOrEmpty(tournamentName))
        {
            lblMessage.Text = "Please enter tournament name.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (!int.TryParse(txtEditYear.Text, out year))
        {
            lblMessage.Text = "Please enter a valid year.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TournamentID", tournamentId),
            new SqlParameter("@Name", tournamentName),
            new SqlParameter("@Year", year)
        };

        int result = DBConnection.ExecuteNonQuery("UPDATE Tournaments SET Name = @Name, Year = @Year WHERE TournamentID = @TournamentID", parameters);

        gvTournaments.EditIndex = -1;
        LoadTournaments();
    }

    protected void gvTournaments_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int tournamentId = Convert.ToInt32(gvTournaments.DataKeys[e.RowIndex].Value);

        // Check if tournament has matches
        SqlParameter[] checkParams = new SqlParameter[]
        {
            new SqlParameter("@TournamentID", tournamentId)
        };

        DataTable dt = DBConnection.ExecuteQuery("SELECT COUNT(*) AS MatchCount FROM Matches WHERE TournamentID = @TournamentID", checkParams);
        int matchCount = Convert.ToInt32(dt.Rows[0]["MatchCount"]);

        if (matchCount > 0)
        {
            lblMessage.Text = "Cannot delete tournament. Tournament has matches associated with it.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // Check if tournament has points table entries
        dt = DBConnection.ExecuteQuery("SELECT COUNT(*) AS PointsCount FROM PointsTable WHERE TournamentID = @TournamentID", checkParams);
        int pointsCount = Convert.ToInt32(dt.Rows[0]["PointsCount"]);

        if (pointsCount > 0)
        {
            lblMessage.Text = "Cannot delete tournament. Tournament has points table entries associated with it.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // Delete tournament
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TournamentID", tournamentId)
        };

        int result = DBConnection.ExecuteNonQuery("DELETE FROM Tournaments WHERE TournamentID = @TournamentID", parameters);

        if (result > 0)
        {
            lblMessage.Text = "Tournament deleted successfully.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            lblMessage.Text = "Failed to delete tournament.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }

        LoadTournaments();
    }
}