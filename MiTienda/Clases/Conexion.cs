using System.Data.SqlClient;

namespace MiTienda.Clases
{
    public class Conexion
    {
        public static SqlConnection ObtenerConexion()
        {
            //string cadena = "Server=DESKTOP-G0FUU8N;Database=SistemaMT;Trusted_Connection=True;";
            //return new SqlConnection(cadena);

            string connString = @"Server=(LocalDB)\MSSQLLocalDB;
                      Integrated Security=true;
                      AttachDbFilename=|DataDirectory|\Datos\SistemaMT.mdf;
                      ";

            SqlConnection conexion = new SqlConnection(connString);
            conexion.Open(); // Puedes omitir esto si prefieres abrirla más adelante
            return conexion;

        }
        //Database=SistemaMT;
    }
}
