using System;
using System.Web.Security;

public partial class Site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            lnkLogout.Visible = true;
            lnkLogin.Visible = false;

            // Check user role and show appropriate menu
            if (Context.User.IsInRole("Admin"))
            {
                pnlAdminMenu.Visible = true;
            }
            else if (Context.User.IsInRole("TeamManager"))
            {
                pnlManagerMenu.Visible = true;
            }
        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Response.Redirect("~/Login.aspx");
    }
}