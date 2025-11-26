using MiTienda.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MiTienda.Formularios
{
    public partial class FrmReporteInventario : Form
    {
        public FrmReporteInventario()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            dgvInventario.DataSource = ArticuloDAO.ObtenerInventario();

            // Personalización
            dgvInventario.Columns["CodigoArticulo"].HeaderText = "Código";
            dgvInventario.Columns["Nombre"].HeaderText = "Nombre";
            dgvInventario.Columns["PrecioCosto"].HeaderText = "Costo";
            dgvInventario.Columns["Precio"].HeaderText = "Precio Público";
            dgvInventario.Columns["Stock"].HeaderText = "Stock Actual";

            dgvInventario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventario.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInventario.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Validación de stock bajo
            int umbralStock = 1; // puedes ajustar este valor
            foreach (DataGridViewRow row in dgvInventario.Rows)
            {
                if (row.Cells["Stock"].Value != DBNull.Value)
                {
                    int stock = Convert.ToInt32(row.Cells["Stock"].Value);
                    if (stock < umbralStock)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightSalmon;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;

                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.Sheets[1];

                // Encabezados
                for (int i = 0; i < dgvInventario.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvInventario.Columns[i].HeaderText;
                }

                // Datos
                for (int i = 0; i < dgvInventario.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvInventario.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvInventario.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                worksheet.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
