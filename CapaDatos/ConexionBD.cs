using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class ConexionBD
    {
        #region "PATRON SINGLETON"
        private static ConexionBD conexion = null;

        private ConexionBD() { }

        public static ConexionBD GetInstance()
        {
            if (conexion == null)
            {
                conexion = new ConexionBD();
            }
            return conexion;
        }
        #endregion

        public SqlConnection ConexionDB()
        {
            SqlConnection conexion = new SqlConnection
            {
                //ConnectionString = @"Data Source=SQL5111.site4now.net;Initial Catalog=db_ac6a1f_tropivocadb;User Id=db_ac6a1f_tropivocadb_admin;Password=Zerodev2050@;Encrypt=True;TrustServerCertificate=True;"
                ConnectionString = "Data Source=.;Initial Catalog=TropVocacionalBD;Integrated Security=True"
            };

            return conexion;
        }
    }
}
