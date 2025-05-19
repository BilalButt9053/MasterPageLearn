using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DBConnection
{
    private static string connectionString = ConfigurationManager.ConnectionStrings["CricketTournamentDB"].ConnectionString;

    // Get connection
    public static SqlConnection GetConnection()
    {
        SqlConnection con = new SqlConnection(connectionString);
        return con;
    }

    public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        using (var con = GetConnection())
        using (var cmd = new SqlCommand(query, con))
        {
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
   



    // Execute Query and return DataTable (SELECT)
    public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
    {
        using (var con = GetConnection())
        using (var cmd = new SqlCommand(query, con))
        {
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // Execute Scalar and return single value
    public static object ExecuteScalar(string query, params SqlParameter[] parameters)
    {
        using (var con = GetConnection())
        using (var cmd = new SqlCommand(query, con))
        {
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            con.Open();
            return cmd.ExecuteScalar();
        }
    }

}