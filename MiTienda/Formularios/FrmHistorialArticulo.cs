using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace MiTienda.Formularios
{
    public partial class FrmHistorialArticulo : Form
    {
        public FrmHistorialArticulo()
        {
            InitializeComponent();
        }

        //METODOS PARA DAR FORMATO AL GRID

        private void ConfigurarCoincidenciasGrid()
        {
            dgvCoincidenciass.AutoGenerateColumns = false;
            dgvCoincidenciass.Columns.Clear();
            dgvCoincidenciass.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // ✅ mantiene ajuste automático

            dgvCoincidenciass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CodigoArticulo",          // ✅ nombre interno
                DataPropertyName = "CodigoArticulo",
                HeaderText = "Código",
                FillWeight = 100
            });

            dgvCoincidenciass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nombre",                  // ✅ nombre interno
                DataPropertyName = "Nombre",
                HeaderText = "Nombre del artículo",
                FillWeight = 260
            });

            dgvCoincidenciass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Precio",                  // ✅ nombre interno
                DataPropertyName = "Precio",
                HeaderText = "Precio venta",
                DefaultCellStyle = { Format = "C2" },
                FillWeight = 75
            });

            dgvCoincidenciass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Stock",                   // ✅ nombre interno
                DataPropertyName = "Stock",
                HeaderText = "Stock",
                FillWeight = 70
            });

            dgvCoincidenciass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UltimoCosto",             // ✅ nombre interno
                DataPropertyName = "UltimoCosto",
                HeaderText = "Último costo",
                DefaultCellStyle = { Format = "C2" },
                FillWeight = 90
            });

            dgvCoincidenciass.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UltimaCompra",            // ✅ nombre interno
                DataPropertyName = "UltimaCompra",
                HeaderText = "Última compra",
                DefaultCellStyle = { Format = "dd/MM/yyyy" },
                FillWeight = 110
            });

            dgvCoincidenciass.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CentrarDatosGrid(DataGridView dgv)
        {
            // Centrar encabezados
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Centrar contenido de las celdas
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Opcional: centrar filas alternadas también
            dgv.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void AgrandarFuenteYElementos(DataGridView dgv)
        {
            // Fuente más grande para celdas
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Regular);

            // Fuente más grande y negrita para encabezados
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // Ajustar alto de filas y encabezados
            dgv.RowTemplate.Height = 30;           // altura de cada fila
            dgv.ColumnHeadersHeight = 35;          // altura de los encabezados

            // Opcional: aumentar padding interno para más espacio visual
            dgv.DefaultCellStyle.Padding = new Padding(5, 3, 5, 3);
        }


        private void ConfigurarHistorialGrid()
        {
            dgvHistorial.AutoGenerateColumns = false;
            dgvHistorial.Columns.Clear();

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NumeroFactura",
                HeaderText = "N° Factura",
                Width = 120
            });

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaCompra",
                HeaderText = "Fecha",
                DefaultCellStyle = { Format = "dd/MM/yyyy" },
                Width = 100
            });

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreProveedor",
                HeaderText = "Proveedor",
                Width = 200
            });

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CodigoArticulo",
                HeaderText = "Código",
                Width = 100
            });

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "Cantidad",
                Width = 80
            });

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioCosto",
                HeaderText = "Costo unitario",
                DefaultCellStyle = { Format = "C2" },
                Width = 100
            });

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Subtotal",
                HeaderText = "Subtotal",
                DefaultCellStyle = { Format = "C2" },
                Width = 100
            });

            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void AplicarEstilosDataGrid(DataGridView dgv)
        {
            // Alternar colores de filas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

            // Encabezados en negrita y centrados
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            dgv.EnableHeadersVisualStyles = false;

            // Celdas con fuente clara
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Bordes suaves
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Ajuste automático de filas
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }


        private void CargarHistorial(string codigoArticulo)
        {
            using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("ObtenerHistorialEntradasSinFechas", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoArticulo", codigoArticulo);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvHistorial.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Este artículo no tiene historial de entradas.", "Sin historial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportarGrid(DataGridView dgv)
        {
            if (dgv.DataSource == null)
            {
                MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dt = dgv.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.ActiveSheet;

            // Encabezados
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
            }

            // Datos
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dt.Rows[i][j]?.ToString();
                }
            }

            worksheet.Columns.AutoFit();
            excelApp.Visible = true;
        }



        private void txtBuscarArticulo_TextChanged(object sender, EventArgs e)
        {
            string texto = txtBuscarArticulo.Text.Trim();

            if (string.IsNullOrWhiteSpace(texto))
            {
                dgvCoincidenciass.Visible = false;
                lblInfoArticulo.Text = "Escribe un nombre o código para buscar.";
                return;
            }

            using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("BuscarArticulosPorNombreOCodigo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Texto", texto);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    dgvCoincidenciass.Visible = false;
                    lblInfoArticulo.Text = "No se encontraron artículos con ese texto.";
                }
                else
                {
                    dgvCoincidenciass.DataSource = dt;
                    dgvCoincidenciass.Visible = true;
                    lblInfoArticulo.Text = $"Coincidencias encontradas: {dt.Rows.Count}";
                }
            }
        }

        private void dgvCoincidenciass_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (chkFiltrarPorFechas.Checked)
                {
                    MessageBox.Show("El filtro por fechas está activado. Usa el botón 'Filtrar por fechas' para ver el historial.", "Filtro activo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var fila = dgvCoincidenciass.Rows[e.RowIndex];

                if (fila.Cells["CodigoArticulo"].Value == null ||
                    fila.Cells["Nombre"].Value == null ||
                    fila.Cells["Precio"].Value == null ||
                    string.IsNullOrWhiteSpace(fila.Cells["CodigoArticulo"].Value.ToString()))
                {
                    MessageBox.Show("Este artículo no existe o la fila está vacía.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string codigo = fila.Cells["CodigoArticulo"].Value.ToString();
                string nombre = fila.Cells["Nombre"].Value.ToString();
                string precioTexto = fila.Cells["Precio"].Value.ToString();

                if (!decimal.TryParse(precioTexto, out decimal precio))
                {
                    MessageBox.Show("El precio del artículo no es válido.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblInfoArticulo.Text = $"Artículo: {nombre} | Precio actual: {precio:C}";
                CargarHistorial(codigo);
            }
        }

        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscarArticulo.Text = "";
            dgvCoincidenciass.Visible = false;
            dgvHistorial.DataSource = null;
            lblInfoArticulo.Text = "Búsqueda reiniciada.";
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportarGrid(dgvCoincidenciass);
        }

        private void dgvCoincidenciass_SelectionChanged(object sender, EventArgs e)
        {            
            btnFiltrarFechas.Enabled = dgvCoincidenciass.CurrentRow != null && chkFiltrarPorFechas.Checked;
        }


        private void btnFiltrarFechas_Click(object sender, EventArgs e)
        {
            if (!chkFiltrarPorFechas.Checked)
            {
                MessageBox.Show("Activa el filtro por fechas para usar este botón.", "Filtro desactivado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvCoincidenciass.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un artículo en la lista de coincidencias.", "Filtro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigo = dgvCoincidenciass.CurrentRow.Cells["CodigoArticulo"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("No se pudo obtener el código del artículo seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("ObtenerHistorialEntradas", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoArticulo", codigo);
                cmd.Parameters.AddWithValue("@Desde", dtpDesde.Value.Date);
                cmd.Parameters.AddWithValue("@Hasta", dtpHasta.Value.Date);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvHistorial.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No hay entradas en ese rango de fechas.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void FrmHistorialArticulo_Load(object sender, EventArgs e)
        {
            btnFiltrarFechas.Enabled = false;
            ConfigurarCoincidenciasGrid();
            ConfigurarHistorialGrid();

            // Aplicar estilos visuales
            AplicarEstilosDataGrid(dgvCoincidenciass);
            AplicarEstilosDataGrid(dgvHistorial);

            // Centrar datos en ambos grids
            CentrarDatosGrid(dgvCoincidenciass);
            CentrarDatosGrid(dgvHistorial);

            // ✅ Agrandar fuente y elementos
            AgrandarFuenteYElementos(dgvCoincidenciass);
            AgrandarFuenteYElementos(dgvHistorial);
        }

        private void chkFiltrarPorFechas_CheckedChanged(object sender, EventArgs e)
        {
            btnFiltrarFechas.Enabled = dgvCoincidenciass.CurrentRow != null && chkFiltrarPorFechas.Checked;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnExportarHistorial_Click(object sender, EventArgs e)
        {
            ExportarGrid(dgvHistorial);
        }
    }
}
