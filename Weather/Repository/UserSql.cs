using System.Configuration;
using System.Data.SqlClient;
using Orion.WeatherApi.Models;

namespace Orion.WeatherApi.Repository
{
    public class UserSql : IUserSql
    {
        public UserModel GetUser(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT *" +
                    "FROM dbo.[User]" +
                    "WHERE Username ='" + username + "' and Password ='" + password + "'", con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    var user = new UserModel();

                    while (reader.Read())
                    {
                        user.UserId = int.Parse(reader["UserId"].ToString());
                        user.Username = reader["Username"].ToString();
                        user.Password = reader["Password"].ToString();
                    }
                    return user;
                }
            }
        }
    }
}