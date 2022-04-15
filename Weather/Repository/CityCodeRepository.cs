using Orion.WeatherApi.DTO;
using Orion.WeatherApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Orion.WeatherApi.Repository
{
    public class CityCodeRepository : ICityCodeRepository
    {
        public List<CityCode> GetAllCityCode() 
        {
            List<CityCode> cityCodes = new List<CityCode>();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT [CityId],[Name] FROM[dbo].[CityCode]";

            SqlDataReader reader = cmd.ExecuteReader();

            

            while (reader.Read())
            {
                CityCode cityCode = new CityCode();

                cityCode.CityId = int.Parse(reader["CityId"].ToString());
                cityCode.Name = reader["Name"].ToString();

                cityCodes.Add(cityCode);
            }


            return cityCodes;
        }

        public int GetWeatherCityId (int cityId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT [WeatherCityId] FROM [dbo].[CityCode] WHERE CityId = @cityId";
            cmd.Parameters.AddWithValue("@CityId", cityId);

            SqlDataReader reader = cmd.ExecuteReader();

            int id = 0;

            while (reader.Read())
            {
                id = int.Parse(reader["WeatherCityId"].ToString());
            }

            return id;
        }

        public bool ValidateCity(string city)
        {
            string name = null;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT [Name] FROM [dbo].[CityCode] WHERE Name = @City";
            cmd.Parameters.AddWithValue("@City", city);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
               name = reader["Name"].ToString();
            }
            if (name == null )
            {
                return false;
            }
            return true;
        }

    }
}