using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiTienda.Clases;

namespace MiTienda.Formularios
{
    public partial class FrmCompras : Form
    {
        public FrmCompras()
        {
            InitializeComponent();
            this.Activated += FrmCompras_Activated; // conectar evento
                                                    // Escalar todo el formulario según la fuente
            this.AutoScaleMode = AutoScaleMode.Font;

            // Fuente base más grande y moderna
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        }

        //METODOS



        // Carga proveedores en el combo
        private void CargarProveedores()
        {
            try
            {
                DataTable proveedores = ProveedorDAO.ObtenerProveedores(); // Debe devolver IdProveedor y Nombre

                // Insertar fila vacía al inicio
                DataRow filaVacia = proveedores.NewRow();
                filaVacia["IdProveedor"] = DBNull.Value;
                filaVacia["Nombre"] = "-- Seleccione --";
                proveedores.Rows.InsertAt(filaVacia, 0);

                cmbProveedores.DisplayMember = "Nombre";
                cmbProveedores.ValueMember = "IdProveedor";
                cmbProveedores.DataSource = proveedores;
                cmbProveedores.SelectedIndex = proveedores.Rows.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message);
                cmbProveedores.DataSource = null;
                cmbProveedores.Items.Clear();
                cmbProveedores.SelectedIndex = -1;
            }
        }

        // Enter mueve el foco al siguiente control en el encabezado
        private void ConfigurarNavegacionEncabezado()
        {
            txtNumeroFactura.KeyDown += Encabezado_KeyDownMueveSiguiente;
            cmbProveedores.KeyDown += Encabezado_KeyDownMueveSiguiente;
            dtpFechas.KeyDown += Encabezado_KeyDownMueveSiguiente;
        }

        private void Encabezado_KeyDownMueveSiguiente(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        // Formato inicial de fecha
        private void ConfigurarFechas()
        {
            dtpFechas.Format = DateTimePickerFormat.Custom;
            dtpFechas.CustomFormat = "dd/MM/yyyy";
            dtpFechas.Value = DateTime.Today;
        }
        // Valida encabezado y habilita/deshabilita acciones

        private bool ValidarEncabezado(bool mostrarMensajes = false)
        {
            // Proveedor seleccionado: índice 0 es "-- Seleccione --"
            bool proveedorValido = cmbProveedores.SelectedIndex > 0 && cmbProveedores.SelectedValue != null && cmbProveedores.SelectedValue != DBNull.Value;
            bool numeroValido = !string.IsNullOrWhiteSpace(txtNumeroFactura.Text);
            bool fechaValida = dtpFechas.Value.Date <= DateTime.Today.AddDays(1); // ajusta tu regla

            if (mostrarMensajes)
            {
                if (!proveedorValido) MessageBox.Show("Debe seleccionar un proveedor.");
                if (!numeroValido) MessageBox.Show("Debe ingresar el número de factura.");
                if (!fechaValida) MessageBox.Show("La fecha no es válida.");
            }

            return proveedorValido && numeroValido && fechaValida;
        }
        private void ActualizarEstadoAcciones()
        {
            bool encabezadoOk = ValidarEncabezado();
            bool hayDetalle = dgvDetalleFactura.Rows.Count > 0;
            bool haySeleccion = dgvDetalleFactura.SelectedRows.Count > 0;

            // **Botones**
            btnGuardarFactura.Enabled = encabezadoOk && hayDetalle;
            btnEliminarArticulo.Enabled = hayDetalle && haySeleccion;
            btnCancelar.Enabled = true;
        }        

        private void FrmCompras_Load(object sender, EventArgs e)
        {
            // Encabezado
            ConfigurarFechas();
            CargarProveedores(); // añade "-- Seleccione --"

            // Estado inicial
            txtTotalFactura.Text = "0.00";
            btnGuardarFactura.Enabled = false;
            btnEliminarArticulo.Enabled = false;
            btnCancelar.Enabled = true;

            // Campos bloqueados
            txtNombre.ReadOnly = true;
            txtPrecioCosto.ReadOnly = true;
            txtPrecioPublico.ReadOnly = true;

            // Validaciones de encabezado → recalculan estado
            txtNumeroFactura.TextChanged += (s, ev) => ActualizarEstadoAcciones();
            cmbProveedores.SelectedIndexChanged += (s, ev) => ActualizarEstadoAcciones();
            dtpFechas.ValueChanged += (s, ev) => ActualizarEstadoAcciones();

            // Botones → asegúrate de conectar handlers
            btnGuardarFactura.Click += btnGuardarFactura_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnEliminarArticulo.Click += btnEliminarArticulo_Click;

            // Artículos
            ConfigurarEventosArticulo();

            // Mejor navegación con Enter
            this.KeyPreview = true;

            ConfigurarGrid();

        }

        private void ConfigurarGrid()
        {
            dgvDetalleFactura.AutoGenerateColumns = false; // usar solo las columnas definidas en el diseñador
            dgvDetalleFactura.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ajustar pesos de cada columna
            dgvDetalleFactura.Columns["colDescripcion"].FillWeight = 200;      // más ancho
            dgvDetalleFactura.Columns["colCodigo"].FillWeight = 100;
            dgvDetalleFactura.Columns["colCosto"].FillWeight = 50;
            dgvDetalleFactura.Columns["colCantidad"].FillWeight = 50;
            dgvDetalleFactura.Columns["colSubtotal"].FillWeight = 50;

            dgvDetalleFactura.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvDetalleFactura.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgvDetalleFactura.RowTemplate.Height = 30; // filas más altas


        }


        private void txtPrecioCosto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtPrecioPublico.Focus();
            }
        }

        private void txtPrecioPublico_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtCantidad.Focus();
            }
        }

        private void txtCantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnAgregar.PerformClick(); // Simula el clic en el botón
            }
        }


        private void ConfigurarEventosArticulo()
        {
            txtCodigos.KeyDown += TxtCodigos_KeyDown;
            btnAgregar.Click += BtnAgregar_Click;

            // Navegación con Enter después de editar
            txtPrecioCosto.KeyDown += txtPrecioCosto_KeyDown;
            txtPrecioPublico.KeyDown += txtPrecioPublico_KeyDown;
            txtCantidad.KeyDown += txtCantidad_KeyDown;
        }

        private void TxtCodigos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BuscarArticuloPorCodigo(txtCodigos.Text.Trim());
            }
        }

        private void BuscarArticuloPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return;

            DataTable resultado = ArticuloDAO.BuscarPorCodigo(codigo);
            if (resultado.Rows.Count == 0)
            {
                MessageBox.Show("Artículo no encontrado.");
                LimpiarCamposArticulo();
                return;
            }

            DataRow fila = resultado.Rows[0];
            txtNombre.Text = fila["Nombre"].ToString();
            txtPrecioCosto.Text = Convert.ToDecimal(fila["PrecioCosto"]).ToString("0.00");
            txtPrecioPublico.Text = Convert.ToDecimal(fila["Precio"]).ToString("0.00");
            txtCantidad.Text = "1";
            txtObservacion.Text = "";
            txtCantidad.Focus();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposArticulo()) return;

            string codigo = txtCodigos.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            decimal costo = Convert.ToDecimal(txtPrecioCosto.Text);
            int cantidad = Convert.ToInt32(txtCantidad.Text);
            decimal subtotal = costo * cantidad;

            dgvDetalleFactura.Rows.Add(codigo, nombre, costo.ToString("0.00"), cantidad, subtotal.ToString("0.00"));

            LimpiarCamposArticulo();
            ActualizarTotalFactura();
            ActualizarEstadoAcciones();
        }

        private bool ValidarCamposArticulo()
        {
            if (string.IsNullOrWhiteSpace(txtCodigos.Text)) return false;
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) return false;
            if (!decimal.TryParse(txtPrecioCosto.Text, out _)) return false;
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Cantidad inválida.");
                return false;
            }
            return true;
        }

        private void LimpiarCamposArticulo()
        {
            txtCodigos.Text = "";
            txtNombre.Text = "";
            txtPrecioCosto.Text = "";
            txtPrecioPublico.Text = "";
            txtCantidad.Text = "";
            txtObservacion.Text = "";
            txtCodigos.Focus();
        }

        private void ActualizarTotalFactura()
        {
            decimal total = 0;
            foreach (DataGridViewRow fila in dgvDetalleFactura.Rows)
            {
                if (fila.Cells["colSubtotal"].Value != null)
                {
                    total += Convert.ToDecimal(fila.Cells["colSubtotal"].Value);
                }
            }
            txtTotalFactura.Text = total.ToString("0.00");
        }

        

        // Eliminar artículo seleccionado
        private void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            if (dgvDetalleFactura.SelectedRows.Count > 0)
            {
                dgvDetalleFactura.Rows.RemoveAt(dgvDetalleFactura.SelectedRows[0].Index);
                ActualizarTotalFactura();
                ActualizarEstadoAcciones();
            }
            else
            {
                MessageBox.Show("Seleccione un artículo para eliminar.");
            }
        }

        // Cancelar operación
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Limpiar encabezado
            txtNumeroFactura.Text = "";
            cmbProveedores.SelectedIndex = 0; // "-- Seleccione --"
            dtpFechas.Value = DateTime.Today;

            LimpiarCamposArticulo();

            // Limpiar detalle
            dgvDetalleFactura.Rows.Clear();

            // Limpiar totales
            txtTotalFactura.Text = "0.00";

            ActualizarEstadoAcciones();
        }

        // Guardar factura completa
        private void btnGuardarFactura_Click(object sender, EventArgs e)
        {
            if (!ValidarEncabezado(true)) return;
            if (dgvDetalleFactura.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un artículo.");
                return;
            }

            try
            {
                string numeroFactura = txtNumeroFactura.Text.Trim();
                int idProveedor = Convert.ToInt32(cmbProveedores.SelectedValue);
                DateTime fecha = dtpFechas.Value;
                decimal total = Convert.ToDecimal(txtTotalFactura.Text);

                int idFacturaCompra = FacturaCompraDAO.InsertarFacturaCompra(numeroFactura, idProveedor, fecha, total);

                foreach (DataGridViewRow fila in dgvDetalleFactura.Rows)
                {
                    // Ignorar filas nuevas o vacías
                    if (fila.IsNewRow || fila.Cells["colCodigo"].Value == null)
                        continue;

                    string codigo = fila.Cells["colCodigo"].Value?.ToString();
                    decimal costo = Convert.ToDecimal(fila.Cells["colCosto"].Value);
                    int cantidad = Convert.ToInt32(fila.Cells["colCantidad"].Value);

                    // Precio público tomado del TextBox (puede estar modificado)
                    decimal precioPublico = string.IsNullOrWhiteSpace(txtPrecioPublico.Text) ? 0m : Convert.ToDecimal(txtPrecioPublico.Text);

                    
                    FacturaCompraDAO.InsertarDetalleFacturaCompra(idFacturaCompra, codigo, costo, precioPublico, cantidad);
                }

                MessageBox.Show("Factura de compra registrada correctamente.");
                btnCancelar.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar factura de compra: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            FrmNuevoArticulo frm = new FrmNuevoArticulo();
            frm.ShowDialog(); // abre el formulario y espera a que se cierre
        }

        private void FrmCompras_Activated(object sender, EventArgs e)
        {
            CargarProveedores();
        }

        private void txtPrecioCosto_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, tecla de control (ej. Backspace) y separador decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // bloquea la tecla
            }

            // Permitir solo un punto decimal
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtPrecioPublico_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo números enteros (sin punto decimal)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCodigos_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigos.Text))
            {
                LimpiarCamposArticulo();
            }
        }
    }
}
