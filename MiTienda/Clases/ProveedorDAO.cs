using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTienda.Clases
{
    public static class ProveedorDAO
    {
        public static DataTable ObtenerProveedores()
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                string query = @"SELECT IdProveedor, Nombre FROM Proveedores ORDER BY Nombre";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
