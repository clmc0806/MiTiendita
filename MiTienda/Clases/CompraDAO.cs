using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTienda.Clases
{
    public class CompraDAO
    {
        public static DataTable ObtenerComprasPorFecha(DateTime desde, DateTime hasta)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"SELECT FC.IdFactura, FC.NumeroFactura, FC.FechaCompra, P.Nombre AS Proveedor, FC.Total
                             FROM FacturaCompra FC
                             INNER JOIN Proveedores P ON P.IdProveedor = FC.IdProveedor
                             WHERE FC.FechaCompra BETWEEN @Desde AND @Hasta
                             ORDER BY FC.FechaCompra DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Desde", desde.Date);
                    cmd.Parameters.AddWithValue("@Hasta", hasta.Date.AddDays(1));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static DataTable ObtenerDetalleFactura(int idFactura)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"SELECT A.Nombre AS Producto, DFC.Cantidad, DFC.PrecioCosto, DFC.SubTotal
                             FROM DetalleFacturaCompra DFC
                             INNER JOIN Articulos A ON A.CodigoArticulo = DFC.CodigoArticulo
                             WHERE DFC.IdFactura = @IdFactura";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdFactura", idFactura);

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
}
