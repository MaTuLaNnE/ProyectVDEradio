using ProyectVDEradio.Controllers;
using ProyectVDEradio.DataAccess.Persistence;
using ProyectVDEradio.Models;
using ProyectVDEradio.Utils;
using ProyectVDEradio.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace ProyectVDEradio.DataAccess.APIsRepository
{
    public class APIs : PConection
    {
        public void AddCurrencyAudit(Currency newCurrencyAudit) // ACTUALIZAR ESTO PARA Q RECIBA TODOS LOS PARAMETROS NECESARIOS
        {
            SqlConnection context = null;
            SqlCommand cmd = null;

            using (context = this.GetConection())
            {
                try
                {
                    context.Open();
                    string sqlCurrencyAudits = "insert into Familia_Producto (TimeStamp, UYUUSD, UYUARS, UYUBRL) " +
                                                "values (GATDATE(), @UYUUSD, @UYUARS, @UYUBRL)";
                    cmd = new SqlCommand(sqlCurrencyAudits, context);

                    cmd.Parameters.AddWithValue("TimeStamp", DateTime.Now);
                    cmd.Parameters.AddWithValue("UYUUSD", newCurrencyAudit.UYUUSD);
                    cmd.Parameters.AddWithValue("UYUUSD", newCurrencyAudit.UYUARS);
                    cmd.Parameters.AddWithValue("UYUUSD", newCurrencyAudit.UYUBRL);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    context.Close();
                }
            }
        }
    }
}