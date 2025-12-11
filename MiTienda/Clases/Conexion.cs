using System.Data.SqlClient;

namespace MiTienda.Clases
{
    public static class Conexion
    {
        // Ajusta tu cadena de conexión según tu servidor y base de datos
        private static readonly string cadenaConexion =
            "Server=DESKTOP-G0FUU8N;Database=SistemaMT;Integrated Security=True;";

        // Devuelve una conexión cerrada lista para usar con 'using'
        public static SqlConnection CrearConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}
//"Server=DESKTOP-G0FUU8N;Database=SistemaMT;Integrated Security=True;";


//@"Server=(localdb)\MSSQLLocalDB;Database=SistemaMT;Integrated Security=True;";

