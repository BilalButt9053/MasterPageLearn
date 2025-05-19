using System;
using System.Data;
using System.Data.SqlClient;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
        string teamName = txtTeamName.Text.Trim();

        // Check if username already exists
        SqlParameter[] checkParams = new SqlParameter[]
        {
            new SqlParameter("@Username", username)
        };

        DataTable dt = DBConnection.ExecuteQuery("SELECT UserID FROM Users WHERE Username = @Username", checkParams);

        if (dt.Rows.Count > 0)
        {
            lblMessage.Text = "Username already exists. Please choose another.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // Begin transaction to insert user and team
        using (SqlConnection con = DBConnection.GetConnection())
        {
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                // Insert user
                SqlCommand cmdUser = new SqlCommand("INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, 'TeamManager'); SELECT SCOPE_IDENTITY();", con, transaction);
                cmdUser.Parameters.AddWithValue("@Username", username);
                cmdUser.Parameters.AddWithValue("@Password", password);

                int userId = Convert.ToInt32(cmdUser.ExecuteScalar());

                // Insert team
                SqlCommand cmdTeam = new SqlCommand("INSERT INTO Teams (TeamName, ManagerID) VALUES (@TeamName, @ManagerID)", con, transaction);
                cmdTeam.Parameters.AddWithValue("@TeamName", teamName);
                cmdTeam.Parameters.AddWithValue("@ManagerID", userId);
                cmdTeam.ExecuteNonQuery();

                transaction.Commit();

                
                lblMessage.Text = "Registration successful! You can now login.";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                // Clear form
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                txtTeamName.Text = "";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                lblMessage.Text = "An error occurred: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
}