using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Admin_Teams : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
            }

            LoadManagers();
            LoadTeams();
        }
    }

    private void LoadManagers()
    {
        string query = "SELECT UserID, Username FROM Users WHERE Role = 'TeamManager' OR Role = 'Admin'";
        DataTable dt = DBConnection.ExecuteQuery(query);

        ddlManager.DataSource = dt;
        ddlManager.DataTextField = "Username";
        ddlManager.DataValueField = "UserID";
        ddlManager.DataBind();

        ddlManager.Items.Insert(0, new ListItem("-- Select Manager --", ""));
    }

    private void LoadTeams()
    {
        string query = @"
            SELECT t.TeamID, t.TeamName, u.Username AS ManagerName, t.ManagerID
            FROM Teams t
            LEFT JOIN Users u ON t.ManagerID = u.UserID
            ORDER BY t.TeamName";

        DataTable dt = DBConnection.ExecuteQuery(query);
        gvTeams.DataSource = dt;
        gvTeams.DataBind();
    }

    protected void btnAddTeam_Click(object sender, EventArgs e)
    {
        string teamName = txtTeamName.Text.Trim();
        string managerId = ddlManager.SelectedValue;

        if (string.IsNullOrEmpty(teamName))
        {
            lblMessage.Text = "Please enter team name.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TeamName", teamName),
            new SqlParameter("@ManagerID", string.IsNullOrEmpty(managerId) ? DBNull.Value : (object)int.Parse(managerId))
        };

        int result = DBConnection.ExecuteNonQuery("INSERT INTO Teams (TeamName, ManagerID) VALUES (@TeamName, @ManagerID)", parameters);

        if (result > 0)
        {
            lblMessage.Text = "Team added successfully.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
            txtTeamName.Text = "";
            ddlManager.SelectedIndex = 0;
            LoadTeams();
        }
        else
        {
            lblMessage.Text = "Failed to add team.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtTeamName.Text = "";
        ddlManager.SelectedIndex = 0;
        lblMessage.Text = "";
    }

    protected void gvTeams_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewPlayers")
        {
            int teamId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("Players.aspx?TeamID=" + teamId);
        }
    }

    protected void gvTeams_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvTeams.EditIndex = e.NewEditIndex;
        LoadTeams();

        // Load managers dropdown in edit mode
        DropDownList ddlEditManager = (DropDownList)gvTeams.Rows[e.NewEditIndex].FindControl("ddlEditManager");
        string query = "SELECT UserID, Username FROM Users WHERE Role = 'TeamManager' OR Role = 'Admin'";
        DataTable dt = DBConnection.ExecuteQuery(query);

        ddlEditManager.DataSource = dt;
        ddlEditManager.DataTextField = "Username";
        ddlEditManager.DataValueField = "UserID";
        ddlEditManager.DataBind();

        ddlEditManager.Items.Insert(0, new ListItem("-- Select Manager --", ""));

        // Set selected value
        DataRowView drv = (DataRowView)gvTeams.Rows[e.NewEditIndex].DataItem;
        string managerId = gvTeams.DataKeys[e.NewEditIndex]["ManagerID"].ToString();
        if (!string.IsNullOrEmpty(managerId))
        {
            ddlEditManager.SelectedValue = managerId;
        }
    }

    protected void gvTeams_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvTeams.EditIndex = -1;
        LoadTeams();
    }

    protected void gvTeams_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int teamId = Convert.ToInt32(gvTeams.DataKeys[e.RowIndex].Value);
        TextBox txtEditTeamName = (TextBox)gvTeams.Rows[e.RowIndex].FindControl("txtEditTeamName");
        DropDownList ddlEditManager = (DropDownList)gvTeams.Rows[e.RowIndex].FindControl("ddlEditManager");

        string teamName = txtEditTeamName.Text.Trim();
        string managerId = ddlEditManager.SelectedValue;

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TeamID", teamId),
            new SqlParameter("@TeamName", teamName),
            new SqlParameter("@ManagerID", string.IsNullOrEmpty(managerId) ? DBNull.Value : (object)int.Parse(managerId))
        };

        int result = DBConnection.ExecuteNonQuery("UPDATE Teams SET TeamName = @TeamName, ManagerID = @ManagerID WHERE TeamID = @TeamID", parameters);

        gvTeams.EditIndex = -1;
        LoadTeams();
    }

    protected void gvTeams_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int teamId = Convert.ToInt32(gvTeams.DataKeys[e.RowIndex].Value);

        // Check if team has players
        SqlParameter[] checkParams = new SqlParameter[]
        {
            new SqlParameter("@TeamID", teamId)
        };

        DataTable dt = DBConnection.ExecuteQuery("SELECT COUNT(*) AS PlayerCount FROM Players WHERE TeamID = @TeamID", checkParams);
        int playerCount = Convert.ToInt32(dt.Rows[0]["PlayerCount"]);

        if (playerCount > 0)
        {
            lblMessage.Text = "Cannot delete team. Team has players associated with it.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // Check if team has matches
        dt = DBConnection.ExecuteQuery("SELECT COUNT(*) AS MatchCount FROM Matches WHERE Team1ID = @TeamID OR Team2ID = @TeamID", checkParams);
        int matchCount = Convert.ToInt32(dt.Rows[0]["MatchCount"]);

        if (matchCount > 0)
        {
            lblMessage.Text = "Cannot delete team. Team has matches associated with it.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // Delete team
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TeamID", teamId)
        };

        int result = DBConnection.ExecuteNonQuery("DELETE FROM Teams WHERE TeamID = @TeamID", parameters);

        if (result > 0)
        {
            lblMessage.Text = "Team deleted successfully.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            lblMessage.Text = "Failed to delete team.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }

        LoadTeams();
    }
    protected void ddlManager_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}