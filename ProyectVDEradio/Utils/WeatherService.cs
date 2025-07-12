using ProyectVDEradio.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProyectVDEradio.Utils
{
    public class WeatherService
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<WeatherAuditHistory> GetWeatherHistory(int days = 7)
        {
            var historial = new List<WeatherAuditHistory>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT WeatherAuditId, TimeStamp, Temp, Icon, Description, 
                              Feels_like, Temp_min, Temp_max, Sunrise, Sunset
                       FROM WeatherAudit 
                       WHERE TimeStamp >= DATEADD(DAY, -@days, GETDATE()) 
                       ORDER BY TimeStamp DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@days", days);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            historial.Add(new WeatherAuditHistory
                            {
                                WeatherAuditId = Convert.ToInt32(reader["WeatherAuditId"]),
                                TimeStamp = Convert.ToDateTime(reader["TimeStamp"]),
                                Temp = Convert.ToDecimal(reader["Temp"]),
                                Icon = reader["Icon"].ToString(),
                                Description = reader["Description"].ToString(),
                                Feels_like = Convert.ToInt32(reader["Feels_like"]),
                                Temp_min = Convert.ToInt32(reader["Temp_min"]),
                                Temp_max = Convert.ToInt32(reader["Temp_max"]),
                                Sunrise = Convert.ToDateTime(reader["Sunrise"]),
                                Sunset = Convert.ToDateTime(reader["Sunset"])
                            });
                        }
                    }
                }
            }
            historial.Reverse();
            return historial;
        }

        // Método para insertar datos del clima (usar en IndexClima)
        public async Task InsertWeatherAuditAsync(WeatherData weather)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO WeatherAudit (TimeStamp, Temp, Icon, Description, Feels_like, Temp_min, Temp_max, Sunrise, Sunset)
                       VALUES (@timestamp, @temp, @icon, @description, @feels_like, @temp_min, @temp_max, @sunrise, @sunset)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                    cmd.Parameters.AddWithValue("@temp", Convert.ToDecimal(weather.Temp));
                    cmd.Parameters.AddWithValue("@icon", weather.Icono ?? "01d"); // Valor por defecto si es null
                    cmd.Parameters.AddWithValue("@description", weather.Estado ?? "Desconocido");
                    cmd.Parameters.AddWithValue("@feels_like", Convert.ToInt32(weather.Sensacion));
                    cmd.Parameters.AddWithValue("@temp_min", Convert.ToInt32(weather.TempMin));
                    cmd.Parameters.AddWithValue("@temp_max", Convert.ToInt32(weather.TempMax));
                    cmd.Parameters.AddWithValue("@sunrise", ConvertUnixToDateTime(weather.Amanecer));
                    cmd.Parameters.AddWithValue("@sunset", ConvertUnixToDateTime(weather.Atardecer));

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método auxiliar mejorado para convertir Unix timestamp
        private DateTime ConvertUnixToDateTime(long unixTimeStamp)
        {
            try
            {
                if (unixTimeStamp == 0)
                    return DateTime.Now; // Valor por defecto si no hay timestamp

                return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime;
            }
            catch
            {
                return DateTime.Now; // Valor por defecto en caso de error
            }
        }

        // Método auxiliar para extraer el ID del icono
        private int ExtractIconId(string iconCode)
        {
            // Extrae el número del código del icono (ej: "01d" -> 1)
            if (int.TryParse(iconCode.Substring(0, 2), out int iconId))
                return iconId;
            return 1; // default
        }
        
    }
}