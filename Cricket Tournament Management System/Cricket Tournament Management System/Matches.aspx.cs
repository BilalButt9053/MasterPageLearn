using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Admin_Matches : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
            }

            LoadTournaments();
            LoadTeams();

            // Set default date time to tomorrow
            txtDateTime.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm");

            // Check if TournamentID is passed in query string
            if (!string.IsNullOrEmpty(Request.QueryString["TournamentID"]))
            {
                string tournamentId = Request.QueryString["TournamentID"];
                ddlTournament.SelectedValue = tournamentId;
            }

            LoadMatches();
        }
    }

    private void LoadTournaments()
    {
        try
        {
            string query = "SELECT TournamentID, Name + ' ' + CAST(Year AS VARCHAR) AS TournamentName FROM Tournaments ORDER BY Year DESC, Name";
            DataTable dt = DBConnection.ExecuteQuery(query);

            ddlTournament.DataSource = dt;
            ddlTournament.DataTextField = "TournamentName";
            ddlTournament.DataValueField = "TournamentID";
            ddlTournament.DataBind();

            ddlTournament.Items.Insert(0, new ListItem("-- Select Tournament --", ""));
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading tournaments: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void LoadTeams()
    {
        try
        {
            string query = "SELECT TeamID, TeamName FROM Teams ORDER BY TeamName";
            DataTable dt = DBConnection.ExecuteQuery(query);

            // Create a copy for the second dropdown
            DataTable dtCopy = dt.Copy();

            // Team 1 dropdown
            ddlTeam1.DataSource = dt;
            ddlTeam1.DataTextField = "TeamName";
            ddlTeam1.DataValueField = "TeamID";
            ddlTeam1.DataBind();
            ddlTeam1.Items.Insert(0, new ListItem("-- Select Team 1 --", ""));

            // Team 2 dropdown
            ddlTeam2.DataSource = dtCopy;
            ddlTeam2.DataTextField = "TeamName";
            ddlTeam2.DataValueField = "TeamID";
            ddlTeam2.DataBind();
            ddlTeam2.Items.Insert(0, new ListItem("-- Select Team 2 --", ""));
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading teams: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void LoadMatches()
    {
        try
        {
            string query = @"
                SELECT m.MatchID, t.Name + ' ' + CAST(t.Year AS VARCHAR) AS TournamentName, 
                       t1.TeamName AS Team1, t2.TeamName AS Team2, 
                       m.DateTime, m.Venue, m.Team1Score, m.Team1Wickets, 
                       m.Team2Score, m.Team2Wickets, m.WinnerTeamID, m.Team1ID, m.Team2ID,
                       t.TournamentID
                FROM Matches m
                INNER JOIN Tournaments t ON m.TournamentID = t.TournamentID
                INNER JOIN Teams t1 ON m.Team1ID = t1.TeamID
                INNER JOIN Teams t2 ON m.Team2ID = t2.TeamID";

            SqlParameter[] parameters = null;

            if (!string.IsNullOrEmpty(Request.QueryString["TournamentID"]))
            {
                query += " WHERE m.TournamentID = @TournamentID";
                parameters = new SqlParameter[] {
                    new SqlParameter("@TournamentID", Request.QueryString["TournamentID"])
                };
            }

            query += " ORDER BY m.DateTime DESC";

            DataTable dt = DBConnection.ExecuteQuery(query, parameters);
            gvMatches.DataSource = dt;
            gvMatches.DataBind();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading matches: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    public string GetResultText(object team1Score, object team1Wickets, object team2Score, object team2Wickets,
                              object winnerTeamId, object team1Id, object team2Id, object team1Name, object team2Name)
    {
        if (winnerTeamId == DBNull.Value)
        {
            return "Not played yet";
        }

        string result = "";

        // Add scores
        if (team1Score != DBNull.Value && team2Score != DBNull.Value)
        {
            result += team1Name + ": " + team1Score;
            if (team1Wickets != DBNull.Value)
            {
                result += "/" + team1Wickets;
            }

            result += ", " + team2Name + ": " + team2Score;
            if (team2Wickets != DBNull.Value)
            {
                result += "/" + team2Wickets;
            }

            result += " | ";
        }

        // Add winner
        if (winnerTeamId != DBNull.Value)
        {
            int winner = Convert.ToInt32(winnerTeamId);
            int team1 = Convert.ToInt32(team1Id);
            int team2 = Convert.ToInt32(team2Id);

            if (winner == team1)
            {
                result += team1Name + " won";
            }
            else if (winner == team2)
            {
                result += team2Name + " won";
            }
            else
            {
                result += "Draw";
            }
        }

        return result;
    }

    protected void btnAddMatch_Click(object sender, EventArgs e)
    {
        try
        {
            string tournamentId = ddlTournament.SelectedValue;
            string team1Id = ddlTeam1.SelectedValue;
            string team2Id = ddlTeam2.SelectedValue;
            string venue = txtVenue.Text.Trim();
            DateTime dateTime;

            // Validation
            if (string.IsNullOrEmpty(tournamentId))
            {
                lblMessage.Text = "Please select a tournament.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (string.IsNullOrEmpty(team1Id))
            {
                lblMessage.Text = "Please select Team 1.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (string.IsNullOrEmpty(team2Id))
            {
                lblMessage.Text = "Please select Team 2.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (team1Id == team2Id)
            {
                lblMessage.Text = "Team 1 and Team 2 cannot be the same.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (!DateTime.TryParse(txtDateTime.Text, out dateTime) || dateTime < DateTime.Now)
            {
                lblMessage.Text = "Please enter a valid future date and time.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (string.IsNullOrEmpty(venue))
            {
                lblMessage.Text = "Please enter a venue.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Check for duplicate matches
            string checkDuplicateQuery = @"
                SELECT COUNT(*) FROM Matches 
                WHERE TournamentID = @TournamentID 
                AND ((Team1ID = @Team1ID AND Team2ID = @Team2ID) 
                OR (Team1ID = @Team2ID AND Team2ID = @Team1ID))";

            SqlParameter[] duplicateParams = new SqlParameter[]
            {
                new SqlParameter("@TournamentID", int.Parse(tournamentId)),
                new SqlParameter("@Team1ID", int.Parse(team1Id)),
                new SqlParameter("@Team2ID", int.Parse(team2Id))
            };

            int existingMatches = (int)DBConnection.ExecuteScalar(checkDuplicateQuery, duplicateParams);

            if (existingMatches > 0)
            {
                lblMessage.Text = "A match between these teams already exists in this tournament.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Insert the match
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TournamentID", int.Parse(tournamentId)),
                new SqlParameter("@Team1ID", int.Parse(team1Id)),
                new SqlParameter("@Team2ID", int.Parse(team2Id)),
                new SqlParameter("@DateTime", dateTime),
                new SqlParameter("@Venue", venue)
            };

            int result = DBConnection.ExecuteNonQuery(
                "INSERT INTO Matches (TournamentID, Team1ID, Team2ID, DateTime, Venue) " +
                "VALUES (@TournamentID, @Team1ID, @Team2ID, @DateTime, @Venue)",
                parameters);

            if (result > 0)
            {
                lblMessage.Text = "Match added successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                // Clear form
                ddlTournament.SelectedIndex = 0;
                ddlTeam1.SelectedIndex = 0;
                ddlTeam2.SelectedIndex = 0;
                txtDateTime.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm");
                txtVenue.Text = "";

                LoadMatches();
            }
            else
            {
                lblMessage.Text = "Failed to add match.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (SqlException ex)
        {
            lblMessage.Text = "Database error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlTournament.SelectedIndex = 0;
        ddlTeam1.SelectedIndex = 0;
        ddlTeam2.SelectedIndex = 0;
        txtDateTime.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm");
        txtVenue.Text = "";
        lblMessage.Text = "";
    }

    protected void gvMatches_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "UpdateResult")
            {
                int matchId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("UpdateMatchResult.aspx?MatchID=" + matchId);
            }
            else if (e.CommandName == "DeleteMatch")
            {
                int matchId = Convert.ToInt32(e.CommandArgument);

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MatchID", matchId)
                };

                int result = DBConnection.ExecuteNonQuery("DELETE FROM Matches WHERE MatchID = @MatchID", parameters);

                if (result > 0)
                {
                    lblMessage.Text = "Match deleted successfully.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    LoadMatches();
                }
                else
                {
                    lblMessage.Text = "Failed to delete match.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
}