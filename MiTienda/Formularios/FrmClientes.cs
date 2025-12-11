//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Data.SqlClient;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace MiTienda.Formularios
//{
//    public partial class FrmClientes : Form
//    {

//        public int ClienteID { get; set; } = 0; // 0 = nuevo cliente, >0 = edición
//        public FrmClientes()
//        {
//            InitializeComponent();
//        }

//        public FrmClientes(int clienteID = 0)
//        {
//            InitializeComponent();
//            ClienteID = clienteID;

//            if (ClienteID > 0)
//            {
//                CargarCliente();
//            }
//        }
//        private void CargarCliente()
//        {
//            using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
//            {
//                string query = "SELECT Nombre, Telefono, CorreoElectronico, Direccion FROM Clientes WHERE ClienteID = @ClienteID";
//                SqlCommand cmd = new SqlCommand(query, conn);
//                cmd.Parameters.AddWithValue("@ClienteID", ClienteID);

//                conn.Open();
//                SqlDataReader reader = cmd.ExecuteReader();
//                if (reader.Read())
//                {
//                    txtNombre.Text = reader["Nombre"].ToString();
//                    txtTelefono.Text = reader["Telefono"].ToString();
//                    txtCorreo.Text = reader["CorreoElectronico"].ToString();
//                    txtDireccion.Text = reader["Direccion"].ToString();
//                }
//                conn.Close();
//            }
//        }




//        private void btnGuardar_Click(object sender, EventArgs e)
//        {
//            if (string.IsNullOrWhiteSpace(txtNombre.Text) || (txtTelefono.Text != "1" && txtTelefono.Text.Length < 8))
//            {
//                MessageBox.Show("Nombre y teléfono son obligatorios.");
//                return;
//            }

//            using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
//            {
//                SqlCommand cmd;

//                if (ClienteID == 0)
//                {
//                    // Nuevo cliente → INSERT
//                    cmd = new SqlCommand("INSERT INTO Clientes (Nombre, Telefono, CorreoElectronico, Direccion) " +
//                                         "VALUES (@Nombre, @Telefono, @CorreoElectronico, @Direccion)", conn);
//                }
//                else
//                {
//                    // Cliente existente → UPDATE
//                    cmd = new SqlCommand("UPDATE Clientes SET Nombre=@Nombre, Telefono=@Telefono, " +
//                                         "CorreoElectronico=@CorreoElectronico, Direccion=@Direccion " +
//                                         "WHERE ClienteID=@ClienteID", conn);
//                    cmd.Parameters.AddWithValue("@ClienteID", ClienteID);
//                }

//                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
//                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
//                cmd.Parameters.AddWithValue("@CorreoElectronico", txtCorreo.Text);
//                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);

//                conn.Open();
//                cmd.ExecuteNonQuery();
//                conn.Close();

//                MessageBox.Show("Cliente guardado con éxito.");
//                this.DialogResult = DialogResult.OK;
//                this.Close();
//            }
//        }

//        private void btnCancelar_Click(object sender, EventArgs e)
//        {
//            this.DialogResult = DialogResult.Cancel;
//            this.Close();
//        }

//        private void txtTelefono_Leave(object sender, EventArgs e)
//        {
//            using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
//            {
//                string query = "SELECT ClienteID, Nombre, CorreoElectronico, Direccion FROM Clientes WHERE Telefono = @Telefono";
//                SqlCommand cmd = new SqlCommand(query, conn);
//                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);

//                conn.Open();
//                SqlDataReader reader = cmd.ExecuteReader();
//                if (reader.Read())
//                {
//                    ClienteID = Convert.ToInt32(reader["ClienteID"]);
//                    txtNombre.Text = reader["Nombre"].ToString();
//                    txtCorreo.Text = reader["CorreoElectronico"].ToString();
//                    txtDireccion.Text = reader["Direccion"].ToString();
//                }
//                conn.Close();
//            }
//        }
//    }
//}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MiTienda.Formularios
{
    public partial class FrmClientes : Form
    {
        public int ClienteID { get; set; } = 0; // 0 = nuevo cliente, >0 = edición

        public FrmClientes()
        {
            InitializeComponent();
        }

        public FrmClientes(int clienteID = 0)
        {
            InitializeComponent();
            ClienteID = clienteID;

            if (ClienteID > 0)
            {
                CargarCliente();
            }
        }

        private void CargarCliente()
        {
            using (var conn = Clases.Conexion.CrearConexion()) // conexión cerrada
            using (var cmd = new SqlCommand(
                "SELECT Nombre, Telefono, CorreoElectronico, Direccion FROM Clientes WHERE ClienteID = @ClienteID", conn))
            {
                cmd.Parameters.AddWithValue("@ClienteID", ClienteID);

                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        txtNombre.Text = reader["Nombre"].ToString();
                        txtTelefono.Text = reader["Telefono"].ToString();
                        txtCorreo.Text = reader["CorreoElectronico"].ToString();
                        txtDireccion.Text = reader["Direccion"].ToString();
                    }
                }
            } // conn y reader se cierran automáticamente aquí
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || (txtTelefono.Text != "1" && txtTelefono.Text.Length < 8))
            {
                MessageBox.Show("Nombre y teléfono son obligatorios.");
                return;
            }

            using (var conn = Clases.Conexion.CrearConexion())
            using (var cmd = new SqlCommand("", conn))
            {
                if (ClienteID == 0)
                {
                    // Nuevo cliente → INSERT
                    cmd.CommandText = "INSERT INTO Clientes (Nombre, Telefono, CorreoElectronico, Direccion) " +
                                      "VALUES (@Nombre, @Telefono, @CorreoElectronico, @Direccion)";
                }
                else
                {
                    // Cliente existente → UPDATE
                    cmd.CommandText = "UPDATE Clientes SET Nombre=@Nombre, Telefono=@Telefono, " +
                                      "CorreoElectronico=@CorreoElectronico, Direccion=@Direccion " +
                                      "WHERE ClienteID=@ClienteID";
                    cmd.Parameters.AddWithValue("@ClienteID", ClienteID);
                }

                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@CorreoElectronico", txtCorreo.Text);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            } // conn y cmd se cierran automáticamente

            MessageBox.Show("Cliente guardado con éxito.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtTelefono_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTelefono.Text)) return;

            using (var conn = Clases.Conexion.CrearConexion())
            {
                conn.Open();
                string query = @"SELECT TOP 1 ClienteID, Nombre, CorreoElectronico, Direccion 
                         FROM Clientes 
                         WHERE Telefono = @Telefono";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ClienteID = Convert.ToInt32(reader["ClienteID"]);
                            txtNombre.Text = reader["Nombre"].ToString();
                            txtCorreo.Text = reader["CorreoElectronico"].ToString();
                            txtDireccion.Text = reader["Direccion"].ToString();
                        }
                    } // ✅ reader cerrado aquí
                } // ✅ cmd cerrado aquí
            } // ✅ conn cerrado aquí
        }

    }
}
