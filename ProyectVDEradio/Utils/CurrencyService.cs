using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProyectVDEradio.Utils
{
    public class CurrencyService
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // Buscamos cotizacion en las ultimas 24 horas
        public (decimal usd, decimal ars, decimal brl)? GetLatestAuditIfRecent()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"SELECT TOP 1 UYUUSD, UYUARS, UYUBRL
                           FROM CurrencyAudit
                           WHERE Timestamp >= DATEADD(DAY, -1, GETDATE())
                           ORDER BY Timestamp DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            reader.GetDecimal(0), // UYUUSD
                            reader.GetDecimal(1), // UYUARS
                            reader.GetDecimal(2)  // UYUBRL
                        );
                    }
                }
            }

            return null;
        }

        public async Task InsertCurrencyAuditAsync(decimal usd, decimal ars, decimal brl)
        {
            string query = @"INSERT INTO CurrencyAudit (Timestamp, UYUUSD, UYUARS, UYUBRL)
                         VALUES (@Timestamp, @USD, @ARS, @BRL)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                command.Parameters.AddWithValue("@USD", usd);
                command.Parameters.AddWithValue("@ARS", ars);
                command.Parameters.AddWithValue("@BRL", brl);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }


        public async Task GetCurrencyAudit()
        {
            string query = @"SELECT * FROM CurrencyAudit";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {

            }
        }

    }
}