using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class TeamManager_Players : System.Web.UI.Page
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
                return;
            }

            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            userId = Convert.ToInt32(Session["UserID"]);
            if (!LoadTeamInfo())
            {
                // If no team is found, show a message
                lblMessage.Text = "No team found for this manager. Please contact an administrator.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                LoadPlayers();
            }
        }
    }

    private bool LoadTeamInfo()
    {
        try
        {
            // First, verify we have the correct UserID
            if (Session["UserID"] == null)
            {
                lblMessage.Text = "Session expired. Please log in again.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return false;
            }

            userId = Convert.ToInt32(Session["UserID"]);

            // For debugging - show the UserID we're using
            lblMessage.Text = "Checking teams for UserID: " + userId;

            SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@ManagerID", userId)
        };

            string query = "SELECT TeamID, TeamName FROM Teams WHERE ManagerID = @ManagerID";
            DataTable dt = DBConnection.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                teamId = Convert.ToInt32(dt.Rows[0]["TeamID"]);
                string teamName = dt.Rows[0]["TeamName"].ToString();

                // Store team info in session for use across pages
                Session["TeamID"] = teamId;
                Session["TeamName"] = teamName;

                // Clear any error messages
                lblMessage.Text = "Managing team: " + teamName;
                lblMessage.ForeColor = System.Drawing.Color.Green;

                return true;
            }
            else
            {
                lblMessage.Text = "No team found for this manager (UserID: " + userId + "). Please contact an administrator.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return false;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading team information: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
    }

    private void LoadPlayers()
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TeamID", teamId)
            };

            string query = "SELECT PlayerID, Name, Role FROM Players WHERE TeamID = @TeamID ORDER BY Name";
            DataTable dt = DBConnection.ExecuteQuery(query, parameters);
            gvPlayers.DataSource = dt;
            gvPlayers.DataBind();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading players: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnAddPlayer_Click(object sender, EventArgs e)
    {
        try
        {
            string playerName = txtPlayerName.Text.Trim();
            string role = ddlRole.SelectedValue;

            if (string.IsNullOrEmpty(playerName))
            {
                lblMessage.Text = "Please enter player name.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (string.IsNullOrEmpty(role))
            {
                lblMessage.Text = "Please select a role.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Verify the team exists before adding a player
            SqlParameter[] checkTeamParams = new SqlParameter[]
            {
                new SqlParameter("@TeamID", teamId)
            };

            DataTable teamCheck = DBConnection.ExecuteQuery("SELECT TeamID FROM Teams WHERE TeamID = @TeamID", checkTeamParams);

            if (teamCheck.Rows.Count == 0)
            {
                lblMessage.Text = "Error: Team not found. Please contact an administrator.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", playerName),
                new SqlParameter("@TeamID", teamId),
                new SqlParameter("@Role", role)
            };

            // Fixed the query - removed single quotes around parameter names
            int result = DBConnection.ExecuteNonQuery("INSERT INTO Players (Name, TeamID, Role) VALUES (@Name, @TeamID, @Role)", parameters);

            if (result > 0)
            {
                lblMessage.Text = "Player added successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                txtPlayerName.Text = "";
                ddlRole.SelectedIndex = 0;
                LoadPlayers();
            }
            else
            {
                lblMessage.Text = "Failed to add player.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error adding player: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtPlayerName.Text = "";
        ddlRole.SelectedIndex = 0;
        lblMessage.Text = "";
    }

    protected void gvPlayers_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPlayers.EditIndex = e.NewEditIndex;
        LoadPlayers();
    }

    protected void gvPlayers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPlayers.EditIndex = -1;
        LoadPlayers();
    }

    protected void gvPlayers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int playerId = Convert.ToInt32(gvPlayers.DataKeys[e.RowIndex].Value);
            TextBox txtEditPlayerName = (TextBox)gvPlayers.Rows[e.RowIndex].FindControl("txtEditPlayerName");
            DropDownList ddlEditRole = (DropDownList)gvPlayers.Rows[e.RowIndex].FindControl("ddlEditRole");

            string playerName = txtEditPlayerName.Text.Trim();
            string role = ddlEditRole.SelectedValue;

            if (string.IsNullOrEmpty(playerName))
            {
                lblMessage.Text = "Please enter player name.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PlayerID", playerId),
                new SqlParameter("@Name", playerName),
                new SqlParameter("@Role", role)
            };

            int result = DBConnection.ExecuteNonQuery("UPDATE Players SET Name = @Name, Role = @Role WHERE PlayerID = @PlayerID", parameters);

            gvPlayers.EditIndex = -1;
            LoadPlayers();

            lblMessage.Text = "Player updated successfully.";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error updating player: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void gvPlayers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int playerId = Convert.ToInt32(gvPlayers.DataKeys[e.RowIndex].Value);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PlayerID", playerId)
            };

            int result = DBConnection.ExecuteNonQuery("DELETE FROM Players WHERE PlayerID = @PlayerID", parameters);

            if (result > 0)
            {
                lblMessage.Text = "Player deleted successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMessage.Text = "Failed to delete player.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }

            LoadPlayers();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting player: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
}