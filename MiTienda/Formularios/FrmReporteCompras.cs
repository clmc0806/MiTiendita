using MiTienda.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MiTienda.Formularios
{
    public partial class FrmReporteCompras : Form
    {
        public FrmReporteCompras()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable compras = CompraDAO.ObtenerComprasPorFecha(dtpDesde.Value, dtpHasta.Value);
                MessageBox.Show("Filas encontradas: " + compras.Rows.Count); // Confirmación visual
                dgvCompras.DataSource = compras;

                if (dgvCompras.Columns.Count > 0)
                {
                    dgvCompras.Columns["IdFactura"].HeaderText = "ID";
                    dgvCompras.Columns["NumeroFactura"].HeaderText = "Factura";
                    dgvCompras.Columns["FechaCompra"].HeaderText = "Fecha";
                    dgvCompras.Columns["Proveedor"].HeaderText = "Proveedor";
                    dgvCompras.Columns["Total"].HeaderText = "Total";

                    dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar compras: " + ex.Message);
            }
        }
               

        private void dgvCompras_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCompras.CurrentRow != null)
            {
                int idFactura = Convert.ToInt32(dgvCompras.CurrentRow.Cells["IdFactura"].Value);
                dgvDetall.DataSource = CompraDAO.ObtenerDetalleFactura(idFactura);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnExportarCompras_Click(object sender, EventArgs e)
        {
            if (dgvCompras.Rows.Count == 0)
            {
                MessageBox.Show("No hay compras para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;

                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.Sheets[1];
                worksheet.Name = "Compras";

                worksheet.Cells[1, 1] = "Reporte de Compras";
                worksheet.Range["A1:E1"].Merge();
                worksheet.Range["A1"].Font.Bold = true;
                worksheet.Range["A1"].Font.Size = 14;

                for (int i = 0; i < dgvCompras.Columns.Count; i++)
                {
                    worksheet.Cells[2, i + 1] = dgvCompras.Columns[i].HeaderText;
                }

                for (int i = 0; i < dgvCompras.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvCompras.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 3, j + 1] = dgvCompras.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                worksheet.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar compras: " + ex.Message);
            }
        }

        private void btnExportarDetalle_Click(object sender, EventArgs e)
        {
            if (dgvCompras.CurrentRow == null || dgvDetall.Rows.Count == 0)
            {
                MessageBox.Show("Seleccione una factura con detalle para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;

                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.Sheets[1];
                worksheet.Name = "Detalle";

                // Encabezado contextual
                string numeroFactura = dgvCompras.CurrentRow.Cells["NumeroFactura"].Value?.ToString();
                string proveedor = dgvCompras.CurrentRow.Cells["Proveedor"].Value?.ToString();
                string fecha = Convert.ToDateTime(dgvCompras.CurrentRow.Cells["FechaCompra"].Value).ToShortDateString();
                string total = dgvCompras.CurrentRow.Cells["Total"].Value?.ToString();

                worksheet.Cells[1, 1] = $"Detalle de la factura {numeroFactura}";
                worksheet.Range["A1:D1"].Merge();
                worksheet.Range["A1"].Font.Bold = true;
                worksheet.Range["A1"].Font.Size = 14;

                worksheet.Cells[2, 1] = $"Proveedor: {proveedor}";
                worksheet.Cells[2, 2] = $"Fecha: {fecha}";
                worksheet.Cells[2, 3] = $"Total: {total}";

                // Encabezados del detalle
                for (int i = 0; i < dgvDetall.Columns.Count; i++)
                {
                    worksheet.Cells[4, i + 1] = dgvDetall.Columns[i].HeaderText;
                }

                // Datos del detalle
                for (int i = 0; i < dgvDetall.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvDetall.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 5, j + 1] = dgvDetall.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                worksheet.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar detalle: " + ex.Message);
            }
        }
    }
}



