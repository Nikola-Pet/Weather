using System.Configuration;
using System.Data.SqlClient;
using Orion.WeatherApi.Models;

namespace Orion.WeatherApi.Repository
{
    public class HistoryRepository : IHistoryRepository
    {
        public void AddHistory(HystoryModel history)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO [dbo].[History](" +
                    "[Username], [IPAddress], [DateTime], [Request], [Data], [TypeId], [Response])" +
                    "VALUES" +
                    "(@Username, @IPAddress, @DateTime, @Request, @Data, @Type, @Response)";

            if (history.Username == null)
            {
                history.Username = " ";
            }
            cmd.Parameters.AddWithValue("@username", history.Username);
            cmd.Parameters.AddWithValue("@Request", history.Request);
            cmd.Parameters.AddWithValue("@IPAddress", history.IPAddress);
            cmd.Parameters.AddWithValue("@DateTime", history.DateTime);
            cmd.Parameters.AddWithValue("@Data", history.Data);
            cmd.Parameters.AddWithValue("@Type", GetTipeIdByName(history.TypeId.ToString()));
            if (history.Response == null)
            {
                history.Response = " ";
            }
            cmd.Parameters.AddWithValue("@Response", history.Response);

            cmd.ExecuteNonQuery();
        }
        public int GetTipeIdByName(string name)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT [TypeId] FROM[dbo].[TypeResponse] WHERE Name = @name";
            
            cmd.Parameters.AddWithValue("@Name", name);

            SqlDataReader reader = cmd.ExecuteReader();

            int id = 0;

            while (reader.Read())
            {
                 id = int.Parse(reader["TypeId"].ToString());
            }

            return id;


        }

    }

}