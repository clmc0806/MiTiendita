//using MiTienda.Clases;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace MiTienda.Formularios
//{
//    public partial class FrmNuevoArticulo : Form
//    {
//        public FrmNuevoArticulo()
//        {
//            InitializeComponent();
//        }

//        public string CodigoArticulo
//        {
//            get => txtCodigo.Text;
//            set => txtCodigo.Text = value;
//        }

//        //Esto te permitirá acceder a la cantidad desde FrmArticulos
//        public int CantidadComprada
//        {
//            get => int.TryParse(txtCantidad.Text, out int val) ? val : 0;
//        }        

//        private void btnGuardar_Click_1(object sender, EventArgs e)
//        {
//            if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
//            {
//                MessageBox.Show("Código y nombre son obligatorios.");
//                return;
//            }

//            int cantidad = int.TryParse(txtCantidad.Text, out int val) ? val : 0;

//            try
//            {
//                bool exito = ArticuloDAO.RegistrarNuevoArticulo(
//                    txtCodigo.Text.Trim(),
//                    txtNombre.Text.Trim(),
//                    decimal.Parse(txtPrecioPublico.Text),
//                    cantidad,
//                    decimal.Parse(txtPrecioCosto.Text),
//                    txtObservaciones.Text.Trim()
//                );

//                if (exito)
//                {
//                    MessageBox.Show("Artículo registrado correctamente.");
//                    this.DialogResult = DialogResult.OK;
//                    this.Close();
//                }
//                else
//                {
//                    // 👉 Aquí ya no es un error genérico, sino que el artículo existe
//                    DialogResult respuesta = MessageBox.Show(
//                        "El artículo ya existe. ¿Desea actualizarlo?",
//                        "Artículo existente",
//                        MessageBoxButtons.YesNo,
//                        MessageBoxIcon.Question
//                    );

//                    if (respuesta == DialogResult.Yes)
//                    {
//                        bool actualizado = ArticuloDAO.ActualizarArticulo(
//                            txtCodigo.Text.Trim(),
//                            txtNombre.Text.Trim(),
//                            decimal.Parse(txtPrecioPublico.Text),
//                            cantidad,
//                            decimal.Parse(txtPrecioCosto.Text),
//                            txtObservaciones.Text.Trim()
//                        );

//                        if (actualizado)
//                        {
//                            MessageBox.Show("Artículo actualizado correctamente.");
//                            this.DialogResult = DialogResult.OK;
//                            this.Close();
//                        }
//                        else
//                        {
//                            MessageBox.Show("No se pudo actualizar el artículo.");
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Error inesperado: " + ex.Message);
//            }
//        }

//        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Enter)
//            {
//                string codigo = txtCodigo.Text.Trim();
//                if (!string.IsNullOrEmpty(codigo))
//                {
//                    DataTable dt = ArticuloDAO.BuscarPorCodigo(codigo);
//                    if (dt.Rows.Count > 0)
//                    {
//                        // Si encontró el artículo, rellenar los campos
//                        DataRow row = dt.Rows[0];
//                        txtNombre.Text = row["Nombre"].ToString();
//                        txtPrecioCosto.Text = row["PrecioCosto"].ToString();
//                        txtCantidad.Text = row["Stock"].ToString();
//                        txtPrecioPublico.Text = row["Precio"].ToString();
//                        txtObservaciones.Text = row["Observaciones"].ToString();

//                        MessageBox.Show("Artículo encontrado y listo para actualizar.");
//                    }
//                    else
//                    {
//                        // Si no existe, limpiar campos para nuevo registro
//                        txtNombre.Clear();
//                        txtPrecioCosto.Clear();
//                        txtCantidad.Clear();
//                        txtPrecioPublico.Clear();
//                        txtObservaciones.Clear();

//                        MessageBox.Show("Artículo no encontrado. Puede registrarlo como nuevo.");
//                    }
//                }

//                // Evita que el Enter haga un 'ding' en el TextBox
//                e.SuppressKeyPress = true;
//            }
//        }

//        private void btnActualizar_Click(object sender, EventArgs e)
//        {
//            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
//            {
//                MessageBox.Show("Debe ingresar el código del artículo a actualizar.");
//                return;
//            }

//            // Parseo seguro de cantidad (stock)
//            int stock = int.TryParse(txtCantidad.Text, out var val) ? val : 0;

//            bool exito = ArticuloDAO.ActualizarArticulo(
//                txtCodigo.Text.Trim(),
//                txtNombre.Text.Trim(),
//                decimal.Parse(txtPrecioPublico.Text),
//                stock, // 👉 parámetro faltante: cantidad/stock
//                decimal.Parse(txtPrecioCosto.Text),
//                txtObservaciones.Text.Trim()
//            );

//            if (exito)
//            {
//                MessageBox.Show("Artículo actualizado correctamente.");
//                this.DialogResult = DialogResult.OK;
//                this.Close();
//            }
//            else
//            {
//                MessageBox.Show("No se encontró el artículo o no se pudo actualizar.");
//            }
//        }

//        private void FrmNuevoArticulo_Load(object sender, EventArgs e)
//        {
//            // 1. Fuente más grande
//            this.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
//        }
//    }
//}

using MiTienda.Clases;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MiTienda.Formularios
{
    public partial class FrmNuevoArticulo : Form
    {
        public FrmNuevoArticulo()
        {
            InitializeComponent();
        }

        public string CodigoArticulo
        {
            get => txtCodigo.Text;
            set => txtCodigo.Text = value;
        }

        // Permite acceder a la cantidad desde FrmArticulos
        public int CantidadComprada
        {
            get => int.TryParse(txtCantidad.Text, out int val) ? val : 0;
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Código y nombre son obligatorios.");
                return;
            }

            int cantidad = int.TryParse(txtCantidad.Text, out int val) ? val : 0;

            try
            {
                // Parseo seguro de precios
                if (!decimal.TryParse(txtPrecioPublico.Text, out decimal precioPublico) ||
                    !decimal.TryParse(txtPrecioCosto.Text, out decimal precioCosto))
                {
                    MessageBox.Show("Precio público y costo deben ser valores numéricos válidos.");
                    return;
                }

                bool exito = ArticuloDAO.RegistrarNuevoArticulo(
                    txtCodigo.Text.Trim(),
                    txtNombre.Text.Trim(),
                    precioPublico,
                    cantidad,
                    precioCosto,
                    txtObservaciones.Text.Trim()
                );

                if (exito)
                {
                    MessageBox.Show("Artículo registrado correctamente.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // 👉 Si el artículo ya existe, preguntar si se desea actualizar
                    DialogResult respuesta = MessageBox.Show(
                        "El artículo ya existe. ¿Desea actualizarlo?",
                        "Artículo existente",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (respuesta == DialogResult.Yes)
                    {
                        bool actualizado = ArticuloDAO.ActualizarArticulo(
                            txtCodigo.Text.Trim(),
                            txtNombre.Text.Trim(),
                            precioPublico,
                            cantidad,
                            precioCosto,
                            txtObservaciones.Text.Trim()
                        );

                        if (actualizado)
                        {
                            MessageBox.Show("Artículo actualizado correctamente.");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo actualizar el artículo.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message);
            }
        }

        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string codigo = txtCodigo.Text.Trim();
                if (!string.IsNullOrEmpty(codigo))
                {
                    DataTable dt = ArticuloDAO.BuscarPorCodigo(codigo);
                    if (dt.Rows.Count > 0)
                    {
                        // Si encontró el artículo, rellenar los campos
                        DataRow row = dt.Rows[0];
                        txtNombre.Text = row["Nombre"].ToString();
                        txtPrecioCosto.Text = row["PrecioCosto"].ToString();
                        txtCantidad.Text = row["Stock"].ToString();
                        txtPrecioPublico.Text = row["Precio"].ToString();
                        txtObservaciones.Text = row["Observaciones"].ToString();

                        MessageBox.Show("Artículo encontrado y listo para actualizar.");
                    }
                    else
                    {
                        // Si no existe, limpiar campos para nuevo registro
                        txtNombre.Clear();
                        txtPrecioCosto.Clear();
                        txtCantidad.Clear();
                        txtPrecioPublico.Clear();
                        txtObservaciones.Clear();

                        MessageBox.Show("Artículo no encontrado. Puede registrarlo como nuevo.");
                    }
                }

                // Evita que el Enter haga un 'ding' en el TextBox
                e.SuppressKeyPress = true;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Debe ingresar el código del artículo a actualizar.");
                return;
            }

            // Parseo seguro de cantidad y precios
            int stock = int.TryParse(txtCantidad.Text, out var val) ? val : 0;

            if (!decimal.TryParse(txtPrecioPublico.Text, out decimal precioPublico) ||
                !decimal.TryParse(txtPrecioCosto.Text, out decimal precioCosto))
            {
                MessageBox.Show("Precio público y costo deben ser valores numéricos válidos.");
                return;
            }

            bool exito = ArticuloDAO.ActualizarArticulo(
                txtCodigo.Text.Trim(),
                txtNombre.Text.Trim(),
                precioPublico,
                stock,
                precioCosto,
                txtObservaciones.Text.Trim()
            );

            if (exito)
            {
                MessageBox.Show("Artículo actualizado correctamente.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("No se encontró el artículo o no se pudo actualizar.");
            }
        }

        private void FrmNuevoArticulo_Load(object sender, EventArgs e)
        {
            // Fuente más grande para todo el formulario
            this.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
        }
    }
}
