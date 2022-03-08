using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Orion.WeatherApi.Models;

namespace Orion.WeatherApi.Repository
{
    public class HistoryRepository : IHistoryRepository
    {
        public void AddHistory(HystoryModel history)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO [dbo].[History](" +
                    "[Username]" +
                    ",[SearchRequest]" +
                    ",[Data]" +
                    ",[Type]" +
                    ",[Response])" +
                    "VALUES" +
                    "('" + history.Username + "','" + 
                    history.SearchRequest + "', '" + 
                    history.Data + "" +
                    "', '" + history.Type + "', '" + 
                    history.Response + "')", con))
                {
                    cmd.ExecuteNonQuery();

                }
                con.Close();
            }            
        }
    }
}