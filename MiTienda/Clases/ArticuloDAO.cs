using System.Data;
using System.Data.SqlClient;

namespace MiTienda.Clases
{
    public class ArticuloDAO
    {
        public static DataTable BuscarPorCodigo(string codigo)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                string query = "EXEC BuscarArticuloPorCodigo @CodigoArticulo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CodigoArticulo", codigo);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static bool RegistrarNuevoArticulo(string codigo, string nombre, decimal precio, int stock, decimal precioCosto, string observaciones)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                // 1. Verificar si ya existe
                string checkQuery = "SELECT COUNT(*) FROM Articulos WHERE CodigoArticulo = @Codigo";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Codigo", codigo);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // 👉 Ya existe → no insertar, devolver false
                        return false;
                    }
                }

                // 2. Insertar si no existe
                string query = @"INSERT INTO Articulos (CodigoArticulo, Nombre, PrecioCosto, Precio, Stock, Observaciones)
                         VALUES (@Codigo, @Nombre, @PrecioCosto, @Precio, @Stock, @Observaciones)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Codigo", codigo);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Precio", precio);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@PrecioCosto", precioCosto);
                    cmd.Parameters.AddWithValue("@Observaciones", observaciones);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public static DataTable ObtenerInventario()
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"SELECT CodigoArticulo, Nombre, PrecioCosto, Precio, Stock, Observaciones FROM Articulos ORDER BY Nombre";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static void ActualizarStock(string codigoArticulo, int cantidadVendida)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Articulos
                                 SET Stock = Stock - @CantidadVendida
                                 WHERE CodigoArticulo = @CodigoArticulo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CantidadVendida", cantidadVendida);
                    cmd.Parameters.AddWithValue("@CodigoArticulo", codigoArticulo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool ActualizarArticulo(string codigo, string nombre, decimal precio, int stock, decimal precioCosto, string observaciones)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Articulos 
                         SET Nombre=@Nombre, Precio=@Precio, Stock=@Stock, 
                             PrecioCosto=@PrecioCosto, Observaciones=@Observaciones
                         WHERE CodigoArticulo=@Codigo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Codigo", codigo);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Precio", precio);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@PrecioCosto", precioCosto);
                    cmd.Parameters.AddWithValue("@Observaciones", observaciones);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public static bool GuardarArticulo(string codigo, string nombre, decimal precio, int stock, decimal precioCosto, string observaciones)
        {
            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();

                // Verificar existencia
                string checkQuery = "SELECT COUNT(*) FROM Articulos WHERE CodigoArticulo = @Codigo";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Codigo", codigo);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Ya existe → UPDATE
                        string updateQuery = @"UPDATE Articulos 
                                       SET Nombre=@Nombre, Precio=@Precio, Stock=@Stock, 
                                           PrecioCosto=@PrecioCosto, Observaciones=@Observaciones
                                       WHERE CodigoArticulo=@Codigo";
                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@Codigo", codigo);
                            updateCmd.Parameters.AddWithValue("@Nombre", nombre);
                            updateCmd.Parameters.AddWithValue("@Precio", precio);
                            updateCmd.Parameters.AddWithValue("@Stock", stock);
                            updateCmd.Parameters.AddWithValue("@PrecioCosto", precioCosto);
                            updateCmd.Parameters.AddWithValue("@Observaciones", observaciones);

                            return updateCmd.ExecuteNonQuery() > 0;
                        }
                    }
                    else
                    {
                        // No existe → INSERT
                        string insertQuery = @"INSERT INTO Articulos (CodigoArticulo, Nombre, PrecioCosto, Precio, Stock, Observaciones)
                                       VALUES (@Codigo, @Nombre, @PrecioCosto, @Precio, @Stock, @Observaciones)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@Codigo", codigo);
                            insertCmd.Parameters.AddWithValue("@Nombre", nombre);
                            insertCmd.Parameters.AddWithValue("@Precio", precio);
                            insertCmd.Parameters.AddWithValue("@Stock", stock);
                            insertCmd.Parameters.AddWithValue("@PrecioCosto", precioCosto);
                            insertCmd.Parameters.AddWithValue("@Observaciones", observaciones);

                            return insertCmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
            }
        }


    }
}
