//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Data.SqlClient;
//using MiTienda.Clases;

//namespace MiTienda.Formularios
//{
//    public partial class FrmProveedores : Form
//    {
//        public FrmProveedores()
//        {
//            InitializeComponent();
//        }

//        private void btnGuardar_Click(object sender, EventArgs e)
//        {
//            if (string.IsNullOrWhiteSpace(txtNombre.Text))
//            {
//                MessageBox.Show("El nombre del proveedor es obligatorio.");
//                return;
//            }

//            using (SqlConnection conn = Conexion.ObtenerConexion())
//            {
//                SqlCommand cmd = new SqlCommand("INSERT INTO Proveedores (Nombre, Telefono, Direccion) VALUES (@Nombre, @Telefono, @Direccion)", conn);
//                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
//                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
//                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());

//                try
//                {
//                    conn.Open();
//                    cmd.ExecuteNonQuery();
//                    conn.Close();

//                    MessageBox.Show("Proveedor registrado con éxito.");
//                    this.DialogResult = DialogResult.OK;
//                    this.Close();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error al guardar el proveedor: " + ex.Message);
//                }
//            }
//        }

//        private void btnCancelar_Click(object sender, EventArgs e)
//        {
//            this.DialogResult = DialogResult.Cancel;
//            this.Close();
//        }
//    }
//}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MiTienda.Clases;

namespace MiTienda.Formularios
{
    public partial class FrmProveedores : Form
    {
        public FrmProveedores()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del proveedor es obligatorio.");
                return;
            }

            try
            {
                using (var conn = Conexion.CrearConexion())
                using (var cmd = new SqlCommand("INSERT INTO Proveedores (Nombre, Telefono, Direccion) VALUES (@Nombre, @Telefono, @Direccion)", conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Proveedor registrado con éxito.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el proveedor: " + ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
