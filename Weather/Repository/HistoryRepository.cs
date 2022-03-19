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
                    "[Username], [SearchRequest], [Data], [Type], [Response])" +
                    "VALUES" +
                    "(@Username, @SearchRequest, @Data, @Type, @Response)";

            cmd.Parameters.AddWithValue("@username", history.Username);
            cmd.Parameters.AddWithValue("@SearchRequest", history.SearchRequest);
            cmd.Parameters.AddWithValue("@Data", history.Data);
            cmd.Parameters.AddWithValue("@Type", history.Type);
            cmd.Parameters.AddWithValue("@Response", history.Response);
            cmd.ExecuteNonQuery();
                      
        }
    }
}