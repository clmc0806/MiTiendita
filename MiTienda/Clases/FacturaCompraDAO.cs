using System;
using System.Data.SqlClient;

namespace MiTienda.Clases
{
    public static class FacturaCompraDAO
    {
        // Inserta el encabezado de la factura de compra
        public static int InsertarFacturaCompra(string numeroFactura, int idProveedor, DateTime fechaCompra, decimal total)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO FacturaCompra (NumeroFactura, IdProveedor, FechaCompra, Total)
                                 OUTPUT INSERTED.IdFactura
                                 VALUES (@NumeroFactura, @IdProveedor, @FechaCompra, @Total)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura);
                    cmd.Parameters.AddWithValue("@IdProveedor", idProveedor);
                    cmd.Parameters.AddWithValue("@FechaCompra", fechaCompra);
                    cmd.Parameters.AddWithValue("@Total", total);

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        // Inserta el detalle de la factura de compra
        public static void InsertarDetalleFacturaCompra(int idFactura, string codigoArticulo, decimal costo, decimal precioPublico, int cantidad)
        {
            // Calculamos el subtotal internamente
            decimal subtotal = costo * cantidad;

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "INSERT INTO DetalleFacturaCompra (IdFactura, CodigoArticulo, PrecioCosto, Cantidad, Subtotal) " +
                               "VALUES (@IdFactura, @CodigoArticulo, @PrecioCosto, @Cantidad, @Subtotal)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                    cmd.Parameters.AddWithValue("@CodigoArticulo", codigoArticulo);
                    cmd.Parameters.AddWithValue("@PrecioCosto",costo);                    
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@Subtotal", subtotal);

                    cmd.ExecuteNonQuery();
                }
            }

            // Actualizar inventario después de registrar el detalle
            ActualizarInventario(codigoArticulo, costo, precioPublico, cantidad);
        }


        // Actualiza stock, costo y precio público en la tabla Articulos
        private static void ActualizarInventario(string codigoArticulo, decimal nuevoCosto, decimal nuevoPrecioPublico, int cantidadComprada)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Articulos
                                 SET Stock = Stock + @CantidadComprada,
                                     PrecioCosto = @NuevoCosto,
                                     Precio = @NuevoPrecioPublico
                                 WHERE CodigoArticulo = @CodigoArticulo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CantidadComprada", cantidadComprada);
                    cmd.Parameters.AddWithValue("@NuevoCosto", nuevoCosto);
                    cmd.Parameters.AddWithValue("@NuevoPrecioPublico", nuevoPrecioPublico);
                    cmd.Parameters.AddWithValue("@CodigoArticulo", codigoArticulo);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
