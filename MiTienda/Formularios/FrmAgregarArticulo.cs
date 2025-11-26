using MiTienda.Clases;
using MiTienda.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiTienda.Formularios
{
    public partial class FrmAgregarArticulo : Form
    {
        public FrmAgregarArticulo()
        {
            InitializeComponent();
            txtCodigo.Leave += txtCodigo_Leave;
            btnAgregarArticulo.Click += btnAgregarArticulo_Click;

        }

        //METODOS

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        public event Action<ArticuloDTO> ArticuloAgregado;

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                return;

            DataTable dt = ArticuloDAO.BuscarPorCodigo(txtCodigo.Text.Trim());

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtNombre.Text = row["Nombre"].ToString();
                txtPrecioCosto.Text = Convert.ToDecimal(row["PrecioCosto"]).ToString("F2");
                txtPrecioPublico.Text = Convert.ToDecimal(row["Precio"]).ToString("F2");

                txtCantidad.Focus();
            }
            else
            {
                DialogResult respuesta = MessageBox.Show(
                    "Artículo no encontrado. ¿Desea registrarlo?",
                    "Nuevo artículo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta == DialogResult.Yes)
                {
                    FrmNuevoArticulo frmNuevo = new FrmNuevoArticulo();
                    frmNuevo.CodigoArticulo = txtCodigo.Text.Trim();

                    if (frmNuevo.ShowDialog() == DialogResult.OK)
                    {
                        dt = ArticuloDAO.BuscarPorCodigo(txtCodigo.Text.Trim());
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            txtNombre.Text = row["Nombre"].ToString();
                            txtPrecioCosto.Text = Convert.ToDecimal(row["PrecioCosto"]).ToString("F2");
                            txtPrecioPublico.Text = Convert.ToDecimal(row["Precio"]).ToString("F2");

                            txtCantidad.Text = frmNuevo.CantidadComprada.ToString();
                            txtCantidad.Focus();
                        }
                    }
                }
            }
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            if (!btnAgregarArticulo.Enabled)
                return; // seguridad extra

            if (!btnAgregarArticulo.Enabled)
                return; // seguridad extra

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Código y nombre son obligatorios.");
                return;
            }

            if (!decimal.TryParse(txtPrecioCosto.Text, out decimal costo) ||
                !int.TryParse(txtCantidad.Text, out int cantidad))
            {
                MessageBox.Show("Costo y cantidad deben ser válidos.");
                return;
            }

            decimal.TryParse(txtPrecioPublico.Text, out decimal precioPublico);



            var articulo = new ArticuloDTO
            {
                Codigo = txtCodigo.Text.Trim(),
                Nombre = txtNombre.Text.Trim(),
                Costo = costo,
                PrecioPublico = precioPublico,
                Cantidad = cantidad,
                Observacion = txtObservacion.Text.Trim()
            };

           
            ArticuloAgregado?.Invoke(articulo);

            // Limpiar para siguiente entrada
            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecioCosto.Clear();
            txtPrecioPublico.Clear();
            txtCantidad.Clear();
            txtObservacion.Clear();
            txtCodigo.Focus();

            btnAgregarArticulo.Enabled = true;
        }

        private void FrmAgregarArticulo_Load(object sender, EventArgs e)
        {
            // Al abrir el formulario, el botón está desactivado
            btnAgregarArticulo.Enabled = false;

            // Suscribir eventos para validar dinámicamente
            txtCodigo.TextChanged += (s, ev) => ValidarCampos();
            txtNombre.TextChanged += (s, ev) => ValidarCampos();
            txtPrecioCosto.TextChanged += (s, ev) => ValidarCampos();
            txtPrecioPublico.TextChanged += (s, ev) => ValidarCampos();
            txtCantidad.TextChanged += (s, ev) => ValidarCampos();

            //Formato de fuente
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.BackColor = Color.White;

            //Navegacion
            txtCodigo.KeyDown += Control_KeyDown;
            txtNombre.KeyDown += Control_KeyDown;
            txtPrecioCosto.KeyDown += Control_KeyDown;
            txtPrecioPublico.KeyDown += Control_KeyDown;
            txtCantidad.KeyDown += Control_KeyDown;
            txtObservacion.KeyDown += Control_KeyDown;

            this.BeginInvoke(new Action(() => txtCodigo.Focus()));
        }

        private void ValidarCampos()
        {
            // El botón se activa solo si todos los campos son válidos
            btnAgregarArticulo.Enabled =
                !string.IsNullOrWhiteSpace(txtCodigo.Text) &&
                !string.IsNullOrWhiteSpace(txtNombre.Text) &&
                decimal.TryParse(txtPrecioCosto.Text, out _) &&
                decimal.TryParse(txtPrecioPublico.Text, out _) &&
                int.TryParse(txtCantidad.Text, out _);
        }

        private void txtCodigo_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                return;

            DataTable dt = ArticuloDAO.BuscarPorCodigo(txtCodigo.Text.Trim());

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtNombre.Text = row["Nombre"].ToString();
                txtPrecioCosto.Text = Convert.ToDecimal(row["PrecioCosto"]).ToString("F2");
                txtPrecioPublico.Text = Convert.ToDecimal(row["Precio"]).ToString("F2");

                txtCantidad.Focus();
            }
            else
            {
                DialogResult respuesta = MessageBox.Show(
                    "Artículo no encontrado. ¿Desea registrarlo?",
                    "Nuevo artículo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta == DialogResult.Yes)
                {
                    FrmNuevoArticulo frmNuevo = new FrmNuevoArticulo();
                    frmNuevo.CodigoArticulo = txtCodigo.Text.Trim();

                    if (frmNuevo.ShowDialog() == DialogResult.OK)
                    {
                        dt = ArticuloDAO.BuscarPorCodigo(txtCodigo.Text.Trim());
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            txtNombre.Text = row["Nombre"].ToString();
                            txtPrecioCosto.Text = Convert.ToDecimal(row["PrecioCosto"]).ToString("F2");
                            txtPrecioPublico.Text = Convert.ToDecimal(row["Precio"]).ToString("F2");

                            txtCantidad.Text = frmNuevo.CantidadComprada.ToString();
                            txtCantidad.Focus();
                        }
                    }
                }
            }
        }

        //private void btnAgregarArticulo_Click_1(object sender, EventArgs e)
        //{
        //    if (!btnAgregarArticulo.Enabled)
        //        return; // seguridad extra

        //    //if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
        //    //{
        //    //    MessageBox.Show("Código y nombre son obligatorios.");
        //    //    return;
        //    //}

        //    //if (!decimal.TryParse(txtPrecioCosto.Text, out decimal costo) ||
        //    //    !decimal.TryParse(txtPrecioPublico.Text, out decimal precioPublico) ||
        //    //    !int.TryParse(txtCantidad.Text, out int cantidad))
        //    //{
        //    //    MessageBox.Show("Costo, precio público y cantidad deben ser válidos.");
        //    //    return;
        //    //}

        //    // Capturar valores sin mostrar mensajes
        //    decimal.TryParse(txtPrecioCosto.Text, out decimal costo);
        //    decimal.TryParse(txtPrecioPublico.Text, out decimal precioPublico);
        //    int.TryParse(txtCantidad.Text, out int cantidad);

        //    var articulo = new ArticuloDTO
        //    {
        //        Codigo = txtCodigo.Text.Trim(),
        //        Nombre = txtNombre.Text.Trim(),
        //        Costo = costo,
        //        PrecioPublico = precioPublico,
        //        Cantidad = cantidad,
        //        Observacion = txtObservacion.Text.Trim()
        //    };

        //    ArticuloAgregado?.Invoke(articulo);

        //    // Limpiar para siguiente entrada
        //    txtCodigo.Clear();
        //    txtNombre.Clear();
        //    txtPrecioCosto.Clear();
        //    txtPrecioPublico.Clear();
        //    txtCantidad.Clear();
        //    txtObservacion.Clear();
        //    txtCodigo.Focus();

        //    // Revalidar después de limpiar → botón vuelve a desactivarse
        //    ValidarCampos();
        //}
    }
}
