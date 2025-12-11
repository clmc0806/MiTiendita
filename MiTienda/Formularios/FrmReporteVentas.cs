//using MiTienda.Clases;
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
//using ClosedXML.Excel;


//namespace MiTienda.Formularios
//{
//    public partial class FrmReporteVentas : Form
//    {
//        public FrmReporteVentas()
//        {
//            InitializeComponent();
//        }

//        private DataTable ObtenerVentas(DateTime fechaInicio, DateTime fechaFin, string codigoArticulo)
//        {
//            DataTable dt = new DataTable();

//            using (SqlConnection conn = Conexion.ObtenerConexion()) // tu clase Conexion
//            {
//                using (SqlCommand cmd = new SqlCommand("ReporteVentasPorFecha", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
//                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

//                    if (string.IsNullOrWhiteSpace(codigoArticulo))
//                        cmd.Parameters.AddWithValue("@CodigoArticulo", DBNull.Value);
//                    else
//                        cmd.Parameters.AddWithValue("@CodigoArticulo", codigoArticulo);

//                    SqlDataAdapter da = new SqlDataAdapter(cmd);
//                    da.Fill(dt);
//                }
//            }

//            return dt;
//        }


//        private void btnGenerar_Click(object sender, EventArgs e)
//        {
//            using (SqlConnection conn = Conexion.ObtenerConexion())
//            {
//                using (SqlCommand cmd = new SqlCommand("ReporteVentasPorFecha", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@FechaInicio", dtpInicio.Value.Date);
//                    cmd.Parameters.AddWithValue("@FechaFin", dtpFin.Value.Date);

//                    if (string.IsNullOrWhiteSpace(txtCodigoArticulo.Text))
//                        cmd.Parameters.AddWithValue("@CodigoArticulo", DBNull.Value);
//                    else
//                        cmd.Parameters.AddWithValue("@CodigoArticulo", txtCodigoArticulo.Text);

//                    SqlDataAdapter da = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    da.Fill(dt);

//                    dgvReporte.DataSource = dt;
//                    FormatearGrid();
//                }
//            }
//        }
//        private void FormatearGrid()
//        {
//            if (dgvReporte.Columns.Count == 0) return;

//            dgvReporte.Columns["CodigoArticulo"].HeaderText = "Código";
//            dgvReporte.Columns["NombreArticulo"].HeaderText = "Producto";
//            dgvReporte.Columns["TotalVendida"].HeaderText = "Cantidad";
//            dgvReporte.Columns["TotalVentas"].HeaderText = "Ventas ($)";
//            dgvReporte.Columns["TotalCosto"].HeaderText = "Costo ($)";
//            dgvReporte.Columns["Utilidad"].HeaderText = "Utilidad ($)";
//            dgvReporte.Columns["MargenPorcentaje"].HeaderText = "Margen (%)";

//            dgvReporte.Columns["TotalVentas"].DefaultCellStyle.Format = "C2";
//            dgvReporte.Columns["TotalCosto"].DefaultCellStyle.Format = "C2";
//            dgvReporte.Columns["Utilidad"].DefaultCellStyle.Format = "C2";
//            dgvReporte.Columns["MargenPorcentaje"].DefaultCellStyle.Format = "N2";

//            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            dgvReporte.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvReporte.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//        }

//        private void btnGenerarProyecciones_Click(object sender, EventArgs e)
//        {
//            // Reutilizamos el SP para obtener ventas históricas
//            DataTable dtVentas = ObtenerVentas(dtpInicio.Value.Date, dtpFin.Value.Date, txtCodigoArticulo.Text);

//            int diasProyeccion = (int)nudDiasProyeccion.Value;
//            DataTable dtProyecciones = CalcularProyecciones(dtVentas, diasProyeccion);

//            dgvProyecciones.DataSource = dtProyecciones;
//            FormatearGridProyecciones();
//        }

//        private DataTable CalcularProyecciones(DataTable ventas, int diasProyeccion)
//        {
//            DataTable dtProyecciones = ventas.Copy();
//            dtProyecciones.Columns.Add("PromedioDiario", typeof(decimal));
//            dtProyecciones.Columns.Add("ProyeccionXDias", typeof(decimal));
//            dtProyecciones.Columns.Add("UtilidadProyectada", typeof(decimal));

//            foreach (DataRow row in dtProyecciones.Rows)
//            {
//                int totalVendida = Convert.ToInt32(row["TotalVendida"]);
//                decimal utilidad = Convert.ToDecimal(row["Utilidad"]);
//                DateTime inicio = dtpInicio.Value.Date;
//                DateTime fin = dtpFin.Value.Date;
//                int diasPeriodo = (fin - inicio).Days;
//                if (diasPeriodo <= 0) diasPeriodo = 1;

//                decimal promedioDiario = (decimal)totalVendida / diasPeriodo;
//                decimal proyeccion = promedioDiario * diasProyeccion;
//                decimal utilidadProyectada = (totalVendida > 0) ? (utilidad / totalVendida) * proyeccion : 0;

//                row["PromedioDiario"] = promedioDiario;
//                row["ProyeccionXDias"] = proyeccion;
//                row["UtilidadProyectada"] = utilidadProyectada;
//            }

//            return dtProyecciones;
//        }

//        private void FormatearGridProyecciones()
//        {
//            dgvProyecciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            dgvProyecciones.Columns["PromedioDiario"].HeaderText = "Promedio Diario";
//            dgvProyecciones.Columns["ProyeccionXDias"].HeaderText = "Proyección";
//            dgvProyecciones.Columns["UtilidadProyectada"].HeaderText = "Utilidad Proyectada ($)";
//            dgvProyecciones.Columns["UtilidadProyectada"].DefaultCellStyle.Format = "C2";
//        }

//        private void btnExportarProyecciones_Click(object sender, EventArgs e)
//        {
//            if (dgvProyecciones.Rows.Count == 0)
//            {
//                MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ProyeccionesVentas.xlsx");

//            using (var wb = new ClosedXML.Excel.XLWorkbook())
//            {
//                DataTable dt = (DataTable)dgvProyecciones.DataSource;
//                wb.Worksheets.Add(dt, "Proyecciones");
//                wb.SaveAs(tempPath);
//            }

//            // Abrir en Excel
//            System.Diagnostics.Process.Start(tempPath);
//        }

//        private void btnLimpiar_Click(object sender, EventArgs e)
//        {
//            // Limpia el DataGridView
//            dgvReporte.DataSource = null;
//            dgvReporte.Rows.Clear();

//            // Limpia los filtros
//            txtCodigoArticulo.Clear();
//            dtpInicio.Value = DateTime.Today;
//            dtpFin.Value = DateTime.Today;

//            // Opcional: mensaje al usuario
//            MessageBox.Show("Los datos del reporte de ventas han sido limpiados.",
//                            "Limpiar", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private void btnExportar_Click(object sender, EventArgs e)
//        {
//            if (dgvReporte.Rows.Count == 0)
//            {
//                MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ReporteVentas.xlsx");

//            using (var wb = new ClosedXML.Excel.XLWorkbook())
//            {
//                DataTable dt = (DataTable)dgvReporte.DataSource;
//                wb.Worksheets.Add(dt, "Ventas");
//                wb.SaveAs(tempPath);
//            }

//            System.Diagnostics.Process.Start(tempPath);
//        }

//        private void btnLimpia_Click(object sender, EventArgs e)
//        {
//            dgvReporte.DataSource = null;
//            dgvProyecciones.DataSource = null;
//            txtCodigoArticulo.Clear();
//            nudDiasProyeccion.Value = 30; // valor por defecto
//            dtpInicio.Value = DateTime.Today;
//            dtpFin.Value = DateTime.Today;
//        }
//    }
//}

using MiTienda.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace MiTienda.Formularios
{
    public partial class FrmReporteVentas : Form
    {
        public FrmReporteVentas()
        {
            InitializeComponent();
        }

        private DataTable ObtenerVentas(DateTime fechaInicio, DateTime fechaFin, string codigoArticulo)
        {
            DataTable dt = new DataTable();

            using (var conn = Conexion.CrearConexion()) // ✅ corregido
            using (var cmd = new SqlCommand("ReporteVentasPorFecha", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                if (string.IsNullOrWhiteSpace(codigoArticulo))
                    cmd.Parameters.AddWithValue("@CodigoArticulo", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@CodigoArticulo", codigoArticulo);

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            using (var conn = Conexion.CrearConexion()) // ✅ corregido
            using (var cmd = new SqlCommand("ReporteVentasPorFecha", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", dtpInicio.Value.Date);
                cmd.Parameters.AddWithValue("@FechaFin", dtpFin.Value.Date);

                if (string.IsNullOrWhiteSpace(txtCodigoArticulo.Text))
                    cmd.Parameters.AddWithValue("@CodigoArticulo", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@CodigoArticulo", txtCodigoArticulo.Text);

                using (var da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReporte.DataSource = dt;
                    FormatearGrid();
                }
            }
        }

        private void FormatearGrid()
        {
            if (dgvReporte.Columns.Count == 0) return;

            dgvReporte.Columns["CodigoArticulo"].HeaderText = "Código";
            dgvReporte.Columns["NombreArticulo"].HeaderText = "Producto";
            dgvReporte.Columns["TotalVendida"].HeaderText = "Cantidad";
            dgvReporte.Columns["TotalVentas"].HeaderText = "Ventas ($)";
            dgvReporte.Columns["TotalCosto"].HeaderText = "Costo ($)";
            dgvReporte.Columns["Utilidad"].HeaderText = "Utilidad ($)";
            dgvReporte.Columns["MargenPorcentaje"].HeaderText = "Margen (%)";

            dgvReporte.Columns["TotalVentas"].DefaultCellStyle.Format = "C2";
            dgvReporte.Columns["TotalCosto"].DefaultCellStyle.Format = "C2";
            dgvReporte.Columns["Utilidad"].DefaultCellStyle.Format = "C2";
            dgvReporte.Columns["MargenPorcentaje"].DefaultCellStyle.Format = "N2";

            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReporte.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReporte.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnGenerarProyecciones_Click(object sender, EventArgs e)
        {
            DataTable dtVentas = ObtenerVentas(dtpInicio.Value.Date, dtpFin.Value.Date, txtCodigoArticulo.Text);

            int diasProyeccion = (int)nudDiasProyeccion.Value;
            DataTable dtProyecciones = CalcularProyecciones(dtVentas, diasProyeccion);

            dgvProyecciones.DataSource = dtProyecciones;
            FormatearGridProyecciones();
        }

        private DataTable CalcularProyecciones(DataTable ventas, int diasProyeccion)
        {
            DataTable dtProyecciones = ventas.Copy();
            dtProyecciones.Columns.Add("PromedioDiario", typeof(decimal));
            dtProyecciones.Columns.Add("ProyeccionXDias", typeof(decimal));
            dtProyecciones.Columns.Add("UtilidadProyectada", typeof(decimal));

            foreach (DataRow row in dtProyecciones.Rows)
            {
                int totalVendida = Convert.ToInt32(row["TotalVendida"]);
                decimal utilidad = Convert.ToDecimal(row["Utilidad"]);
                DateTime inicio = dtpInicio.Value.Date;
                DateTime fin = dtpFin.Value.Date;
                int diasPeriodo = (fin - inicio).Days;
                if (diasPeriodo <= 0) diasPeriodo = 1;

                decimal promedioDiario = (decimal)totalVendida / diasPeriodo;
                decimal proyeccion = promedioDiario * diasProyeccion;
                decimal utilidadProyectada = (totalVendida > 0) ? (utilidad / totalVendida) * proyeccion : 0;

                row["PromedioDiario"] = promedioDiario;
                row["ProyeccionXDias"] = proyeccion;
                row["UtilidadProyectada"] = utilidadProyectada;
            }

            return dtProyecciones;
        }

        private void FormatearGridProyecciones()
        {
            dgvProyecciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProyecciones.Columns["PromedioDiario"].HeaderText = "Promedio Diario";
            dgvProyecciones.Columns["ProyeccionXDias"].HeaderText = "Proyección";
            dgvProyecciones.Columns["UtilidadProyectada"].HeaderText = "Utilidad Proyectada ($)";
            dgvProyecciones.Columns["UtilidadProyectada"].DefaultCellStyle.Format = "C2";
        }

        private void btnExportarProyecciones_Click(object sender, EventArgs e)
        {
            if (dgvProyecciones.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ProyeccionesVentas.xlsx");

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                DataTable dt = (DataTable)dgvProyecciones.DataSource;
                wb.Worksheets.Add(dt, "Proyecciones");
                wb.SaveAs(tempPath);
            }

            System.Diagnostics.Process.Start(tempPath);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dgvReporte.DataSource = null;
            dgvReporte.Rows.Clear();

            txtCodigoArticulo.Clear();
            dtpInicio.Value = DateTime.Today;
            dtpFin.Value = DateTime.Today;

            MessageBox.Show("Los datos del reporte de ventas han sido limpiados.",
                            "Limpiar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvReporte.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ReporteVentas.xlsx");

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                DataTable dt = (DataTable)dgvReporte.DataSource;
                wb.Worksheets.Add(dt, "Ventas");
                wb.SaveAs(tempPath);
            }

            System.Diagnostics.Process.Start(tempPath);
        }

        private void btnLimpia_Click(object sender, EventArgs e)
        {
            dgvReporte.DataSource = null;
            dgvProyecciones.DataSource = null;
            txtCodigoArticulo.Clear();
            nudDiasProyeccion.Value = 30;
            dtpInicio.Value = DateTime.Today;
            dtpFin.Value = DateTime.Today;
        }
    }
}
