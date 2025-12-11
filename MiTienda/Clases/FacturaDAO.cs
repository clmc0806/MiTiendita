using MiTienda.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTienda.Clases
{
    internal class FacturaDAO
    {
        public static void InsertarDetalle(int facturaId, List<DetalleFacturaDTO> detalles)
        {
            using (SqlConnection conn = Conexion.CrearConexion())
            {
                conn.Open();
                foreach (var item in detalles)
                {
                    string query = @"INSERT INTO DetalleFactura 
                             (FacturaID, CodigoArticulo, Cantidad, PrecioUnitario)
                             VALUES 
                             (@FacturaID, @CodigoArticulo, @Cantidad, @PrecioUnitario)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FacturaID", facturaId);
                        cmd.Parameters.AddWithValue("@CodigoArticulo", item.CodigoArticulo);
                        cmd.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario);
                        

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void ActualizarStock(string codigoArticulo, int cantidadVendida)
        {
            using (SqlConnection conn = Conexion.CrearConexion())
            {
                conn.Open();
                string query = @"UPDATE Articulos
                         SET Stock = Stock - @CantidadVendida
                         WHERE CodigoArticulo = @Codigo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CantidadVendida", cantidadVendida);
                    cmd.Parameters.AddWithValue("@Codigo", codigoArticulo);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static int InsertarFactura(DateTime fecha, int clienteID, decimal total, string rutaPDF)
        {
            using (SqlConnection conn = Conexion.CrearConexion())
            {
                conn.Open();

                string query = @"INSERT INTO Facturas (Fecha, ClienteID, Total, RutaPDF)
                         OUTPUT INSERTED.FacturaID
                         VALUES (@Fecha, @ClienteID, @Total, @RutaPDF)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Fecha", fecha);
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                    cmd.Parameters.AddWithValue("@Total", total);
                    cmd.Parameters.AddWithValue("@RutaPDF", rutaPDF ?? "");

                    int facturaId = (int)cmd.ExecuteScalar();
                    return facturaId;
                }
            }
        }

    }
}
