using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeProductionApp
{
    public static class DatabaseHelper
    {
        private static string connectionString = @"Server=dbsrv\vip2025;Database=сме_2307б_инд_проект_кофе;Integrated Security=true;TrustServerCertificate=true;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}