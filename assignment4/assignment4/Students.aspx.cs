using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace assignment4
{
    public partial class Students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load student data
                DataAccesLayer obj = new DataAccesLayer();
                obj.FillDG(gvStudents, "SELECT * FROM tblStudents");

                // Initialize country dropdown
                DataAccesLayer obj2 = new DataAccesLayer();
                obj2.FillDD(DropDownList2, "SELECT countryid, countryname FROM tblCountry");

                // Clear other dropdowns initially
                DropDownList3.Items.Clear();
                DropDownList4.Items.Clear();
            }
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedValue != "")
            {
                // Load states for selected country
                DataAccesLayer obj3 = new DataAccesLayer();
                obj3.FillDD(DropDownList3,
                    "SELECT stateid, statename FROM tblState " +
                    "WHERE countryid = " + Convert.ToInt32(DropDownList2.SelectedValue));

                // Clear city dropdown when country changes
                DropDownList4.Items.Clear();
            }
        }

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList3.SelectedValue != "")
            {
                // Load cities for selected state
                DataAccesLayer obj4 = new DataAccesLayer();
                obj4.FillDD(DropDownList4,
                    "SELECT cityid, cityname FROM tblCity " +
                    "WHERE stateid = " + Convert.ToInt32(DropDownList3.SelectedValue));
            }
        }

        protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Add code to filter students by selected city if needed
            if (DropDownList4.SelectedValue != "")
            {
                // Example: Filter gridview by city
                // DataAccesLayer obj = new DataAccesLayer();
                // obj.FillDG(gvStudents, "SELECT * FROM tblStudents WHERE cityid = " + Convert.ToInt32(DropDownList4.SelectedValue));
            }
        }
    }
}