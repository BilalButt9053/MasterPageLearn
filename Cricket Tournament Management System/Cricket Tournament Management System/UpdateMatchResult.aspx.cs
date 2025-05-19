using System;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_UpdateMatchResult : System.Web.UI.Page
{
    private int matchId;
    private int tournamentId;
    private int team1Id;
    private int team2Id;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
        {
            Response.Redirect("~/Login.aspx");
        }

        if (string.IsNullOrEmpty(Request.QueryString["MatchID"]))
        {
            Response.Redirect("Matches.aspx");
        }

        matchId = Convert.ToInt32(Request.QueryString["MatchID"]);

        if (!IsPostBack)
        {
            LoadMatchDetails();
        }
    }

    private void LoadMatchDetails()
    {
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@MatchID", matchId)
        };

        string query = @"
            SELECT m.*, t.Name + ' ' + CAST(t.Year AS VARCHAR) AS TournamentName, 
                   t1.TeamName AS Team1Name, t2.TeamName AS Team2Name
            FROM Matches m
            INNER JOIN Tournaments t ON m.TournamentID = t.TournamentID
            INNER JOIN Teams t1 ON m.Team1ID = t1.TeamID
            INNER JOIN Teams t2 ON m.Team2ID = t2.TeamID
            WHERE m.MatchID = @MatchID";

        DataTable dt = DBConnection.ExecuteQuery(query, parameters);

        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];

            tournamentId = Convert.ToInt32(row["TournamentID"]);
            team1Id = Convert.ToInt32(row["Team1ID"]);
            team2Id = Convert.ToInt32(row["Team2ID"]);

            lblTournament.Text = row["TournamentName"].ToString();
            lblTeam1.Text = row["Team1Name"].ToString();
            lblTeam2.Text = row["Team2Name"].ToString();
            lblDateTime.Text = Convert.ToDateTime(row["DateTime"]).ToString("dd/MM/yyyy HH:mm");
            lblVenue.Text = row["Venue"].ToString();

            // Set existing values if available
            if (row["Team1Score"] != DBNull.Value)
            {
                txtTeam1Score.Text = row["Team1Score"].ToString();
            }

            if (row["Team1Wickets"] != DBNull.Value)
            {
                txtTeam1Wickets.Text = row["Team1Wickets"].ToString();
            }

            if (row["Team2Score"] != DBNull.Value)
            {
                txtTeam2Score.Text = row["Team2Score"].ToString();
            }

            if (row["Team2Wickets"] != DBNull.Value)
            {
                txtTeam2Wickets.Text = row["Team2Wickets"].ToString();
            }

            // Add winner options
            ddlWinner.Items.Add(new System.Web.UI.WebControls.ListItem(row["Team1Name"].ToString(), team1Id.ToString()));
            ddlWinner.Items.Add(new System.Web.UI.WebControls.ListItem(row["Team2Name"].ToString(), team2Id.ToString()));
            ddlWinner.Items.Add(new System.Web.UI.WebControls.ListItem("Draw", "0"));

            // Set selected winner if available
            if (row["WinnerTeamID"] != DBNull.Value)
            {
                ddlWinner.SelectedValue = row["WinnerTeamID"].ToString();
            }
        }
        else
        {
            Response.Redirect("Matches.aspx");
        }
    }

    protected void btnUpdateResult_Click(object sender, EventArgs e)
    {
        int team1Score, team2Score;
        int? team1Wickets = null, team2Wickets = null;
        int winnerTeamId;

        if (!int.TryParse(txtTeam1Score.Text, out team1Score) || team1Score < 0)
        {
            lblMessage.Text = "Please enter a valid score for Team 1.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (!int.TryParse(txtTeam2Score.Text, out team2Score) || team2Score < 0)
        {
            lblMessage.Text = "Please enter a valid score for Team 2.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (!string.IsNullOrEmpty(txtTeam1Wickets.Text))
        {
            int wickets;
            if (int.TryParse(txtTeam1Wickets.Text, out wickets) && wickets >= 0 && wickets <= 10)
            {
                team1Wickets = wickets;
            }
            else
            {
                lblMessage.Text = "Please enter a valid number of wickets for Team 1 (0-10).";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        if (!string.IsNullOrEmpty(txtTeam2Wickets.Text))
        {
            int wickets;
            if (int.TryParse(txtTeam2Wickets.Text, out wickets) && wickets >= 0 && wickets <= 10)
            {
                team2Wickets = wickets;
            }
            else
            {
                lblMessage.Text = "Please enter a valid number of wickets for Team 2 (0-10).";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        if (string.IsNullOrEmpty(ddlWinner.SelectedValue))
        {
            lblMessage.Text = "Please select a winner.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        winnerTeamId = int.Parse(ddlWinner.SelectedValue);

        // Begin transaction to update match result and points table
        using (SqlConnection con = DBConnection.GetConnection())
        {
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                // Update match result
                SqlCommand cmdUpdateMatch = new SqlCommand(@"
                    UPDATE Matches 
                    SET Team1Score = @Team1Score, Team1Wickets = @Team1Wickets,
                        Team2Score = @Team2Score, Team2Wickets = @Team2Wickets,
                        WinnerTeamID = @WinnerTeamID
                    WHERE MatchID = @MatchID", con, transaction);

                cmdUpdateMatch.Parameters.AddWithValue("@MatchID", matchId);
                cmdUpdateMatch.Parameters.AddWithValue("@Team1Score", team1Score);
                cmdUpdateMatch.Parameters.AddWithValue("@Team2Score", team2Score);
                cmdUpdateMatch.Parameters.AddWithValue("@WinnerTeamID", winnerTeamId == 0 ? DBNull.Value : (object)winnerTeamId);

                if (team1Wickets.HasValue)
                    cmdUpdateMatch.Parameters.AddWithValue("@Team1Wickets", team1Wickets.Value);
                else
                    cmdUpdateMatch.Parameters.AddWithValue("@Team1Wickets", DBNull.Value);

                if (team2Wickets.HasValue)
                    cmdUpdateMatch.Parameters.AddWithValue("@Team2Wickets", team2Wickets.Value);
                else
                    cmdUpdateMatch.Parameters.AddWithValue("@Team2Wickets", DBNull.Value);

                cmdUpdateMatch.ExecuteNonQuery();

                // Update points table for Team 1
                UpdatePointsTable(con, transaction, tournamentId, team1Id, winnerTeamId == team1Id);

                // Update points table for Team 2
                UpdatePointsTable(con, transaction, tournamentId, team2Id, winnerTeamId == team2Id);

                transaction.Commit();

                lblMessage.Text = "Match result updated successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                lblMessage.Text = "An error occurred: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    private void UpdatePointsTable(SqlConnection con, SqlTransaction transaction, int tournamentId, int teamId, bool isWinner)
    {
        // Check if team exists in points table for this tournament
        SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM PointsTable WHERE TournamentID = @TournamentID AND TeamID = @TeamID", con, transaction);
        cmdCheck.Parameters.AddWithValue("@TournamentID", tournamentId);
        cmdCheck.Parameters.AddWithValue("@TeamID", teamId);

        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

        if (count == 0)
        {
            // Insert new record
            SqlCommand cmdInsert = new SqlCommand(@"
                INSERT INTO PointsTable (TeamID, TournamentID, MatchesPlayed, Wins, Losses, Points)
                VALUES (@TeamID, @TournamentID, 1, @Wins, @Losses, @Points)", con, transaction);

            cmdInsert.Parameters.AddWithValue("@TeamID", teamId);
            cmdInsert.Parameters.AddWithValue("@TournamentID", tournamentId);
            cmdInsert.Parameters.AddWithValue("@Wins", isWinner ? 1 : 0);
            cmdInsert.Parameters.AddWithValue("@Losses", isWinner ? 0 : 1);
            cmdInsert.Parameters.AddWithValue("@Points", isWinner ? 2 : 0);

            cmdInsert.ExecuteNonQuery();
        }
        else
        {
            // Update existing record
            SqlCommand cmdUpdate = new SqlCommand(@"
                UPDATE PointsTable
                SET MatchesPlayed = MatchesPlayed + 1,
                    Wins = Wins + @WinIncrement,
                    Losses = Losses + @LossIncrement,
                    Points = Points + @PointsIncrement
                WHERE TournamentID = @TournamentID AND TeamID = @TeamID", con, transaction);

            cmdUpdate.Parameters.AddWithValue("@TeamID", teamId);
            cmdUpdate.Parameters.AddWithValue("@TournamentID", tournamentId);
            cmdUpdate.Parameters.AddWithValue("@WinIncrement", isWinner ? 1 : 0);
            cmdUpdate.Parameters.AddWithValue("@LossIncrement", isWinner ? 0 : 1);
            cmdUpdate.Parameters.AddWithValue("@PointsIncrement", isWinner ? 2 : 0);

            cmdUpdate.ExecuteNonQuery();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Matches.aspx");
    }
}