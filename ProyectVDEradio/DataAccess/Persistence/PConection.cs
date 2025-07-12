using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ProyectVDEradio.DataAccess.Persistence
{
    public class PConection
    {

        private string conectionString = "source=DESKTOP-BTQ6HRC\\SQLEXPRESS;initial catalog=VozDelEsteDB;integrated security=True;encrypt=False;MultipleActiveResultSets=True;";
        
        protected SqlConnection GetConection()
        {
            SqlConnection context = new SqlConnection(conectionString);
            return context;
        }
            

    }
}