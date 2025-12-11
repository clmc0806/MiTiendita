//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MiTienda.Clases
//{
//    public static class ProveedorDAO
//    {
//        public static DataTable ObtenerProveedores()
//        {
//            using (SqlConnection conn = Conexion.ObtenerConexion())
//            {
//                string query = @"SELECT IdProveedor, Nombre FROM Proveedores ORDER BY Nombre";
//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
//                {
//                    DataTable dt = new DataTable();
//                    da.Fill(dt);
//                    return dt;
//                }
//            }
//        }
//    }
//}
using System.Data;
using System.Data.SqlClient;

namespace MiTienda.Clases
{
    public static class ProveedorDAO
    {
        public static DataTable ObtenerProveedores()
        {
            string query = @"SELECT IdProveedor, Nombre FROM Proveedores ORDER BY Nombre";

            using (var conn = Conexion.CrearConexion()) // conexión cerrada
            using (var cmd = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                conn.Open();   // abrir aquí, dentro del using
                da.Fill(dt);
                return dt;
            } // conn, cmd y da se cierran automáticamente
        }
    }
}

