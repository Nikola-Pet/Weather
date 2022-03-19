using System.Configuration;
using System.Data.SqlClient;
using Orion.WeatherApi.Models;

namespace Orion.WeatherApi.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserModel GetUser(string username, string password)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM dbo.[User] WHERE Username = @username and Password = @password";

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

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