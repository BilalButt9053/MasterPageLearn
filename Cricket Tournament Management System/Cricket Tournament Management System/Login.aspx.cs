using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            RedirectBasedOnRole();
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            lblMessage.Text = "Please enter both username and password.";
            return;
        }

        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };

            DataTable dt = DBConnection.ExecuteQuery("SELECT UserID, Username, Role FROM Users WHERE Username = @Username AND Password = @Password", parameters);

            if (dt.Rows.Count > 0)
            {
                string role = dt.Rows[0]["Role"].ToString();
                int userId = Convert.ToInt32(dt.Rows[0]["UserID"]); // Convert to int immediately

                // Create authentication ticket
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                              // version
                    username,                       // user name
                    DateTime.Now,                   // issue time
                    DateTime.Now.AddMinutes(30),    // expiration
                    false,                          // persistent
                    role,                           // user data (role)
                    FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie
                Response.Cookies.Add(new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                // Store user ID in session as integer
                Session["UserID"] = userId;
                Session["Username"] = username;
                Session["Role"] = role;

                // For TeamManager role, verify team association
                if (role == "TeamManager")
                {
                    // Check if this manager has an associated team
                    bool hasTeam = VerifyTeamAssociation(userId);
                    Session["HasTeam"] = hasTeam;

                    if (!hasTeam)
                    {
                        // Still redirect, but we'll show a message on the dashboard
                        Response.Redirect("~/TeamManagerDashboard.aspx?noteam=true");
                        return;
                    }
                }

                RedirectBasedOnRole();
            }
            else
            {
                lblMessage.Text = "Invalid username or password.";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Login error: " + ex.Message;
        }
    }

    private bool VerifyTeamAssociation(int managerId)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ManagerID", managerId)
            };

            DataTable dt = DBConnection.ExecuteQuery("SELECT TeamID FROM Teams WHERE ManagerID = @ManagerID", parameters);
            return dt.Rows.Count > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void RedirectBasedOnRole()
    {
        string role = Session["Role"] as string;

        if (role == "Admin")
        {
            Response.Redirect("~/Dashboard.aspx");
        }
        else if (role == "TeamManager")
        {
            Response.Redirect("~/TeamManagerDashboard.aspx");
        }
        else
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}