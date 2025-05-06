using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls; 
using System.Data;
using System.Data.SqlClient;

namespace assignment4
{
    public class DataAccesLayer
    {
        string constr;
        SqlConnection mydbCon;
        SqlDataAdapter da;
        DataSet ds;

        public DataAccesLayer()
        {
            constr = System.Configuration.ConfigurationManager.ConnectionStrings["cs"].ToString();
        }

        protected SqlConnection OpenDBCon()
        {
            mydbCon = new SqlConnection(constr);
            mydbCon.Open();
            return mydbCon;
        }

        public DataSet FillDS(string query)
        {
            da = new SqlDataAdapter(query, OpenDBCon());
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public void FillDD(DropDownList dd, string query )
        {
            DataTable dt = FillDS(query).Tables[0];
            dd.DataSource = dt;
            dd.DataValueField = dt.Columns[0].ToString();
            dd.DataTextField = dt.Columns[1].ToString();
            dd.DataBind();
            dd.Items.Insert(0, new ListItem("<-----Select----->", "0"));
        }

        public void FillDG(GridView g, string query)
        {
            g.DataSource = FillDS(query).Tables[0];
            g.DataBind();
        }
    }
}
