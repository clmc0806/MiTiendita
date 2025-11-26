using iTextSharp.text;
using iTextSharp.text.pdf;
using MiTienda.Clases;
using MiTienda.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using WinFont = System.Drawing.Font;
using PdfFont = iTextSharp.text.Font;




namespace MiTienda.Formularios
{
    public partial class FrmFactura : Form
    {
        public FrmFactura()
        {
            InitializeComponent();
            dgvDetalle.AllowUserToAddRows = false;
            btnEliminarArticulo.Enabled = false;

        }


        //METODOS

        //Metodo para ubicar el foco siempre en txtClientetelefono
        private void FrmFactura_Shown(object sender, EventArgs e)
        {
            // Deferir el foco para después de que el formulario esté visible/activo
            BeginInvoke(new Action(() =>
            {
                ActiveControl = txtTelefonoCliente;
                txtTelefonoCliente.Focus();
                txtTelefonoCliente.Select();
            }));
        }
        // Opcional: método público para que el contenedor pueda forzar el foco
        public void SetInitialFocus()
        {
            BeginInvoke(new Action(() =>
            {
                ActiveControl = txtTelefonoCliente;
                txtTelefonoCliente.Focus();
                txtTelefonoCliente.Select();
            }));
        }

        private bool ValidarFacturaActual(out string mensajeError)
        {
            mensajeError = "";

            // Validar cliente
            if (string.IsNullOrWhiteSpace(lblNombreCliente.Text))
            {
                mensajeError = "No ha seleccionado un cliente válido.";
                return false;
            }

            // Validar detalle de factura
            if (dgvDetalle.Rows.Count == 0)
            {
                mensajeError = "No hay artículos en la factura.";
                return false;
            }

            // Validar pago efectivo si aplica
            if (string.IsNullOrWhiteSpace(txtPagoEfectivo.Text) || !decimal.TryParse(txtPagoEfectivo.Text, out decimal pago) || pago <= 0)
            {
                mensajeError = "El monto de pago en efectivo no es válido.";
                return false;
            }

            return true;
        }



        //método auxiliar para habilitar/deshabilitar
        private void BloquearBotones(bool estado)
        {
            btnFinalizarVenta.Enabled = estado;
            btnEnviarFactura.Enabled = estado;
            btnImprimirPDF.Enabled = estado;
        }

        private DataTable ConstruirTablaDetalle()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CodigoArticulo", typeof(string));
            tabla.Columns.Add("Cantidad", typeof(int));

            foreach (DataGridViewRow row in dgvDetalle.Rows)
            {
                if (!row.IsNewRow)
                {
                    string codigo = row.Cells["CodigoArticulo"].Value?.ToString();
                    int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);

                    tabla.Rows.Add(codigo, cantidad);
                }
            }

            return tabla;
        }


        private decimal CalcularTotalDesdeGrid(DataGridView dgv)
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow && row.Cells[3].Value != null)
                {
                    if (decimal.TryParse(row.Cells[3].Value.ToString(), out decimal valor))
                    {
                        total += valor;
                    }
                }
            }

            return total;
        }


        private void GuardarYGenerarFactura()
        {
            if (clienteID <= 0)
            {
                MessageBox.Show("No se ha seleccionado un cliente válido.");
                return;
            }

            if (dgvDetalle.Rows.Count == 0)
            {
                MessageBox.Show("No hay artículos en la factura.");
                return;
            }

            DataTable tablaDetalle = ConstruirTablaDetalle();

            using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("CrearFactura", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClienteID", clienteID);

                SqlParameter detalleParam = cmd.Parameters.AddWithValue("@Detalle", tablaDetalle);
                detalleParam.SqlDbType = SqlDbType.Structured;
                detalleParam.TypeName = "dbo.DetalleVentaTipo";

                SqlParameter outputId = new SqlParameter("@FacturaID", SqlDbType.Int);
                outputId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                facturaIDActual = Convert.ToInt32(outputId.Value);
            }

            // Generar PDF
            string rutaPDF = "";
            try
            {
                rutaPDF = GenerarFacturaPDF(facturaIDActual, lblNombreCliente.Text, dgvDetalle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message);
                return;
            }

            // Registrar ruta del PDF
            using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("RegistrarRutaPDF", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FacturaID", facturaIDActual);
                cmd.Parameters.AddWithValue("@RutaPDF", rutaPDF);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            MessageBox.Show($"Factura registrada correctamente. N° {facturaIDActual}");
            LimpiarFormulario();

        }


        private int facturaIDActual = 0; // aquí guardaremos el ID de la última factura

        private string GenerarFacturaPDF(int facturaId, string clienteNombre, DataGridView dgvDetalle)
        {
            try
            {
                string carpetaFacturas = Path.Combine(Application.StartupPath, "Facturas");
                Directory.CreateDirectory(carpetaFacturas);
                string rutaArchivo = Path.Combine(carpetaFacturas, $"Factura_{facturaId}.pdf");

                Document doc = new Document(PageSize.LETTER, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));
                doc.Open();

                // Fuentes
                var fuenteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var fuenteNormal = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                var fuenteNegrita = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                // =========================
                // Encabezado con dos columnas
                // =========================
                PdfPTable tablaEncabezado = new PdfPTable(2);
                tablaEncabezado.WidthPercentage = 100;
                tablaEncabezado.SetWidths(new float[] { 70f, 30f });

                // Celda izquierda: datos empresa (alineado a la izquierda)
                PdfPCell celdaEmpresa = new PdfPCell(new Phrase(
                    "Variedades y Papelería La Ceiba\n" +
                    "NIT: 0614-XXXXXX-XXX\n" +
                    "Dirección: Canton San Isidro\n" +
                    "Teléfono: (503) 72439671\n" +
                    "Correo: contacto@laceiba.com",
                    fuenteNormal
                ));
                celdaEmpresa.Border = iTextSharp.text.Rectangle.NO_BORDER;
                celdaEmpresa.HorizontalAlignment = Element.ALIGN_LEFT;
                tablaEncabezado.AddCell(celdaEmpresa);

                // Celda derecha: número de factura (centrado)
                PdfPCell celdaFactura = new PdfPCell(new Phrase($"Factura Nº {facturaId}", fuenteTitulo));
                celdaFactura.Border = iTextSharp.text.Rectangle.NO_BORDER;
                celdaFactura.HorizontalAlignment = Element.ALIGN_CENTER;
                celdaFactura.VerticalAlignment = Element.ALIGN_MIDDLE;
                tablaEncabezado.AddCell(celdaFactura);

                doc.Add(tablaEncabezado);
                doc.Add(new Paragraph(" ")); // Espacio

                // =========================
                // Cliente y fecha
                // =========================
                doc.Add(new Paragraph($"Cliente: {clienteNombre}", fuenteNormal));
                doc.Add(new Paragraph($"Fecha: {DateTime.Now}", fuenteNormal));
                doc.Add(new Paragraph(" "));

                // =========================
                // Tabla de detalle
                // =========================
                PdfPTable tabla = new PdfPTable(4);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 3f, 1f, 1f, 1f });

                string[] encabezados = { "Producto", "Cantidad", "Precio", "SubTotal" };
                foreach (string textoEncabezado in encabezados)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(textoEncabezado, fuenteNegrita));
                    celda.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    celda.BorderWidthBottom = 1f;
                    celda.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(celda);
                }

                decimal totalGeneral = 0;

                foreach (DataGridViewRow row in dgvDetalle.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string nombre = row.Cells["NombreArticulo"].Value?.ToString() ?? "";
                        string cantidad = row.Cells["Cantidad"].Value?.ToString() ?? "";
                        string precio = row.Cells["PrecioUnitario"].Value?.ToString() ?? "";
                        string subtotal = row.Cells["SubTotal"].Value?.ToString() ?? "";

                        PdfPCell celdaNombre = new PdfPCell(new Phrase(nombre, fuenteNormal));
                        PdfPCell celdaCantidad = new PdfPCell(new Phrase(cantidad, fuenteNormal));
                        PdfPCell celdaPrecio = new PdfPCell(new Phrase(precio, fuenteNormal));
                        PdfPCell celdaSubtotal = new PdfPCell(new Phrase(subtotal, fuenteNormal));

                        celdaNombre.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        celdaCantidad.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        celdaPrecio.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        celdaSubtotal.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        celdaNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                        celdaCantidad.HorizontalAlignment = Element.ALIGN_CENTER;
                        celdaPrecio.HorizontalAlignment = Element.ALIGN_CENTER;
                        celdaSubtotal.HorizontalAlignment = Element.ALIGN_CENTER;

                        tabla.AddCell(celdaNombre);
                        tabla.AddCell(celdaCantidad);
                        tabla.AddCell(celdaPrecio);
                        tabla.AddCell(celdaSubtotal);

                        if (decimal.TryParse(subtotal, out decimal valor))
                        {
                            totalGeneral += valor;
                        }
                    }
                }

                doc.Add(tabla);
                doc.Add(new Paragraph(" "));

                // =========================
                // Total y despedida
                // =========================
                Paragraph totalFinal = new Paragraph($"Total: ${totalGeneral:0.00}", fuenteNegrita);
                totalFinal.Alignment = Element.ALIGN_RIGHT;
                doc.Add(totalFinal);

                doc.Add(new Paragraph(" "));
                Paragraph despedida = new Paragraph("Gracias por preferirnos", fuenteNormal);
                despedida.Alignment = Element.ALIGN_CENTER;
                doc.Add(despedida);

                doc.Close();
                return rutaArchivo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message);
                return "";
            }
        }


        private void EnviarFacturaPorCorreo(string rutaPDF, string correoDestino)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("clmc0806@gmail.com");
            mail.To.Add(correoDestino);
            mail.Subject = "Factura electrónica";
            mail.Body = "Adjunto encontrará su factura en formato PDF. ¡Gracias por su preferencia!";
            mail.Attachments.Add(new Attachment(rutaPDF));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("clmc0806@gmail.com", "jieqwoxzlkfvzhhs");
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }

        private void LimpiarFormulario()
        {
            // Cliente
            txtTelefonoCliente.Clear();
            lblNombreCliente.Text = "";
            clienteID = 0;

            // Artículo
            txtCodigoArticulo.Clear();
            lblNombreArticulo.Text = "";
            lblPrecio.Text = "";
            lblStock.Text = "";
            lblEstado.Text = "";
            txtCantidad.Clear();
            txtPagoEfectivo.Text = "";


            // Detalle
            dgvDetalle.Rows.Clear();

            // Total
            lblTotalFactura.Text = "Total: $0.00";

            // Enfocar en el teléfono para iniciar nueva venta
            txtTelefonoCliente.Focus();
        }


        private void BuscarArticuloPorCodigo()
        {
            if (string.IsNullOrWhiteSpace(txtCodigoArticulo.Text))
            {
                // ✅ Limpiar etiquetas si el campo está vacío
                lblNombreArticulo.Text = "";
                lblPrecio.Text = "";
                lblStock.Text = "";
                txtCantidad.Clear();
                return;
            }


            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                string query = "SELECT Nombre, Precio, Stock FROM Articulos WHERE CodigoArticulo = @Codigo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Codigo", txtCodigoArticulo.Text);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblNombreArticulo.Text = reader["Nombre"].ToString();
                    lblPrecio.Text = Convert.ToDecimal(reader["Precio"]).ToString("0.00");
                    lblStock.Text = reader["Stock"].ToString();
                    txtCantidad.Focus();
                }
                else
                {
                    MessageBox.Show("Artículo no encontrado.");
                    lblNombreArticulo.Text = "";
                    lblPrecio.Text = "";
                    lblStock.Text = "";
                }
                conn.Close();
            }
        }

        private decimal ObtenerTotalDesdeLabel()
        {
            string texto = lblTotalFactura.Text.Replace("Total:", "").Replace("$", "").Trim();

            if (decimal.TryParse(texto, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal total))
            {
                return total;
            }

            return 0;
        }


        private void CalcularTotalFactura()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvDetalle.Rows)
            {
                if (row.Cells["SubTotal"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value);
                }
            }

            lblTotalFactura.Text = $"Total:{total:C2}"; // C2 = formato moneda
        }

        private void txtTelefonoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Permitir Enter
                return;
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTelefonoCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BuscarClientePorTelefono();
            }
        }
        int clienteID = 0; // Declara esto a nivel de clase, fuera de cualquier método

        private void BuscarClientePorTelefono()
        {
            if (txtTelefonoCliente.Text != "1" && txtTelefonoCliente.Text.Length < 8)
            {
                MessageBox.Show("El número debe tener al menos 8 dígitos o ser '1' para cliente general.");
                txtTelefonoCliente.Focus();
                return;
            }

            using (SqlConnection conn = Clases.Conexion.ObtenerConexion()) 
            {
                string query = "EXEC BuscarClientePorTelefono @Telefono";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefonoCliente.Text);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblNombreCliente.Text = reader["Nombre"].ToString(); // Muestra el nombre en el label
                    clienteID = Convert.ToInt32(reader["ClienteID"]);
                    // ✅ Mover el foco al campo de artículo
                    txtCodigoArticulo.Focus();
                    txtCodigoArticulo.SelectAll();
                }
                else
                {
                    MessageBox.Show("Cliente no encontrado.");

                    var nuevoCliente = new FrmClientes();
                    if (nuevoCliente.ShowDialog() == DialogResult.OK)
                    {
                        // Reintentar búsqueda después de registrar
                        BuscarClientePorTelefono();
                    }

                    lblNombreCliente.Text = "";
                    clienteID = 0;
                }
                conn.Close();
            }
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(txtCodigoArticulo.Text) ||
                string.IsNullOrWhiteSpace(lblNombreArticulo.Text) ||
                string.IsNullOrWhiteSpace(lblPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                MessageBox.Show("Complete todos los campos antes de agregar.");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Cantidad inválida.");
                return;
            }

            if (!decimal.TryParse(lblPrecio.Text, out decimal precioUnitario))
            {
                MessageBox.Show("Precio inválido.");
                return;
            }
            //Verifica si el stock es suficiente
            if (!int.TryParse(lblStock.Text, out int stockDisponible))
            {
                MessageBox.Show("Stock no válido o no disponible.");
                return;
            }

            if (cantidad > stockDisponible)
            {
                MessageBox.Show("Stock insuficiente. No se puede agregar esta cantidad.");
                return;
            }


            // ✅ Verificar si el artículo ya fue agregado
            foreach (DataGridViewRow row in dgvDetalle.Rows)
            {
                if (row.Cells["CodigoArticulo"].Value?.ToString() == txtCodigoArticulo.Text)
                {
                    MessageBox.Show("Este artículo ya fue agregado.");
                    return;
                }
            }

            // Calcular subtotal y agregar al grid
            decimal subtotal = cantidad * precioUnitario;

            dgvDetalle.Rows.Add(
                txtCodigoArticulo.Text,
                lblNombreArticulo.Text,
                cantidad,
                precioUnitario,
                subtotal
            );

            CalcularTotalFactura();

            // Limpiar campos
            txtCodigoArticulo.Clear();
            lblNombreArticulo.Text = "";
            lblPrecio.Text = "";
            lblStock.Text = "";
            txtCantidad.Clear();
            txtCodigoArticulo.Focus();
        }

        private void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            if (dgvDetalle.CurrentRow != null)
            {
                dgvDetalle.Rows.Remove(dgvDetalle.CurrentRow);
                CalcularTotalFactura();
            }
            else
            {
                MessageBox.Show("Seleccione un artículo para eliminar.");
            }
        }

        private void dgvDetalle_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalle.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                var row = dgvDetalle.Rows[e.RowIndex];

                if (int.TryParse(row.Cells["Cantidad"].Value?.ToString(), out int cantidad) &&
                    decimal.TryParse(row.Cells["PrecioUnitario"].Value?.ToString(), out decimal precio))
                {
                    row.Cells["SubTotal"].Value = cantidad * precio;
                    CalcularTotalFactura();
                }
                else
                {
                    MessageBox.Show("Cantidad inválida.");
                    row.Cells["Cantidad"].Value = 1;
                }
            }
        }

        private void btnFinalizarVenta_Click(object sender, EventArgs e)
        {

            BloquearBotones(false);

            try
            {
                if (!ValidarFacturaActual(out string mensaje))
                {
                    MessageBox.Show(mensaje);
                    BloquearBotones(true);
                    return;
                }

                GuardarYGenerarFactura();

                MessageBox.Show("Venta finalizada.");
                LimpiarFormulario();
                // No reactivamos botones porque la venta ya terminó
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al finalizar la venta: " + ex.Message);
                BloquearBotones(true);
            }

        }

        private void txtCodigoArticulo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true; // evita que suene el beep
                BuscarArticuloPorCodigo();
            }
        }

        private void txtCantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // evita el beep
                btnAgregarArticulo.PerformClick(); // simula clic en el botón
            }
        }

        private void btnCancelarVenta_Click(object sender, EventArgs e)
        {
            // Limpiar cliente
            txtTelefonoCliente.Clear();
            lblNombreCliente.Text = "";
            clienteID = 0;

            // Limpiar artículo
            txtCodigoArticulo.Clear();
            txtCantidad.Clear();

            // Limpiar detalle
            dgvDetalle.Rows.Clear();

            // Limpiar total
            lblTotalFactura.Text = "Total: $0.00";

            // Enfocar en el teléfono para iniciar nueva venta
            txtTelefonoCliente.Focus();
        }

        private void txtCodigoArticulo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoArticulo.Text))
            {
                lblNombreArticulo.Text = "";
                lblPrecio.Text = "";
                lblStock.Text = "";
                txtCantidad.Clear();
            }
        }

        private void dgvDetalle_SelectionChanged(object sender, EventArgs e)
        {
            // Verifica si hay una fila seleccionada y que no sea la fila nueva
            if (dgvDetalle.CurrentRow != null && !dgvDetalle.CurrentRow.IsNewRow)
            {
                btnEliminarArticulo.Enabled = true;
            }
            else
            {
                btnEliminarArticulo.Enabled = false;
            }
        }

        private async void btnEnviarFactura_Click(object sender, EventArgs e)
        {
            // 🔍 Validaciones previas
            if (string.IsNullOrWhiteSpace(lblNombreCliente.Text) || clienteID <= 0)
            {
                MessageBox.Show("Debe seleccionar un cliente antes de enviar la factura.");
                return;
            }

            if (dgvDetalle.Rows.Count == 0)
            {
                MessageBox.Show("No hay artículos en la factura.");
                return;
            }

            // 🔒 Bloquear botones solo si todo es válido
            BloquearBotones(false);

            FrmProgresoEnvio progreso = null;

            try
            {
                // Paso 1: Generar y registrar factura
                DataTable tablaDetalle = ConstruirTablaDetalle();

                using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("CrearFactura", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);

                    SqlParameter detalleParam = cmd.Parameters.AddWithValue("@Detalle", tablaDetalle);
                    detalleParam.SqlDbType = SqlDbType.Structured;
                    detalleParam.TypeName = "dbo.DetalleVentaTipo";

                    SqlParameter outputId = new SqlParameter("@FacturaID", SqlDbType.Int);
                    outputId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    facturaIDActual = Convert.ToInt32(outputId.Value);
                }

                if (facturaIDActual <= 0)
                {
                    MessageBox.Show("No se pudo generar la factura.");
                    return;
                }

                // Paso 2: Generar PDF
                string rutaPDF = "";
                try
                {
                    rutaPDF = GenerarFacturaPDF(facturaIDActual, lblNombreCliente.Text, dgvDetalle);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el PDF: " + ex.Message);
                    return;
                }

                // Paso 3: Registrar ruta del PDF
                using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("RegistrarRutaPDF", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FacturaID", facturaIDActual);
                    cmd.Parameters.AddWithValue("@RutaPDF", rutaPDF);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                // Paso 4: Obtener correo del cliente
                string correoCliente = "";
                using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
                {
                    string query = "SELECT CorreoElectronico FROM Clientes WHERE ClienteID = @ClienteID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        correoCliente = result.ToString();
                    }
                    conn.Close();
                }

                if (string.IsNullOrWhiteSpace(correoCliente))
                {
                    MessageBox.Show("El cliente no tiene correo electrónico registrado.");
                    return;
                }

                // Paso 5: Enviar por correo
                progreso = new FrmProgresoEnvio();
                progreso.Show();

                lblEstado.Text = "Enviando factura...";
                lblEstado.ForeColor = Color.Black;

                await Task.Run(() => EnviarFacturaPorCorreo(rutaPDF, correoCliente));

                lblEstado.Text = "Factura enviada correctamente.";
                lblEstado.ForeColor = Color.Green;
                MessageBox.Show("Factura enviada con éxito.");
                LimpiarFormulario();
            }
            catch (SmtpException smtpEx)
            {
                lblEstado.Text = "Error al enviar.";
                lblEstado.ForeColor = Color.Red;
                MessageBox.Show("Error SMTP: " + smtpEx.Message);
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error inesperado.";
                lblEstado.ForeColor = Color.Red;
                MessageBox.Show("Error general: " + ex.Message);
            }
            finally
            {
                progreso?.Close();
                BloquearBotones(true); // 🔓 Siempre reactivar
            }
        }

        private void txtPagoEfectivo_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPagoEfectivo.Text, out decimal pago))
            {
                decimal totalFactura = ObtenerTotalDesdeLabel();
                decimal vuelto = pago - totalFactura;

                if (vuelto >= 0)
                {
                    lblVuelto.Text = $"Vuelto: ${vuelto:0.00}"; ;
                    lblVuelto.ForeColor = Color.Black;
                }
                else
                {
                    lblVuelto.Text = "Pago insuficiente";
                    lblVuelto.ForeColor = Color.Red;
                }
            }
            else
            {
                lblVuelto.Text = "";
            }
        }

        private void btnImprimirPDF_Click(object sender, EventArgs e)
        {
            BloquearBotones(false); // deshabilita botones críticos

            
            try
            {
                // Validaciones previas
                if (clienteID <= 0)
                {
                    MessageBox.Show("Debe seleccionar un cliente antes de imprimir la factura.");
                    return;
                }

                if (dgvDetalle.Rows.Count == 0)
                {
                    MessageBox.Show("No hay artículos en la factura.");
                    return;
                }              

                // Paso 1: Generar y registrar factura en BD
                DataTable tablaDetalle = ConstruirTablaDetalle();

                using (SqlConnection conn = Clases.Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("CrearFactura", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);

                    SqlParameter detalleParam = cmd.Parameters.AddWithValue("@Detalle", tablaDetalle);
                    detalleParam.SqlDbType = SqlDbType.Structured;
                    detalleParam.TypeName = "dbo.DetalleVentaTipo";

                    SqlParameter outputId = new SqlParameter("@FacturaID", SqlDbType.Int);
                    outputId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    facturaIDActual = Convert.ToInt32(outputId.Value);
                }

                if (facturaIDActual <= 0)
                {
                    MessageBox.Show("No se pudo generar la factura.");
                    return;
                }

                // Paso 2: Generar PDF
                string rutaPDF = "";
                try
                {
                    rutaPDF = GenerarFacturaPDF(facturaIDActual, lblNombreCliente.Text, dgvDetalle);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el PDF: " + ex.Message);
                    return;
                }

                // Paso 3: Abrir PDF en visor predeterminado
                
                System.Diagnostics.Process.Start(rutaPDF);

               
                MessageBox.Show("Factura abierta en el visor predeterminado.");

                // Paso 4: Limpiar formulario
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Error general: " + ex.Message);
            }
            finally
            {                

                BloquearBotones(true); // habilita botones de nuevo
            }
        }

        private void FrmFactura_Load(object sender, EventArgs e)
        {
            
            AplicarEstiloGlobal();
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ajustar proporciones manualmente
            dgvDetalle.Columns["CodigoArticulo"].FillWeight = 15;
            dgvDetalle.Columns["NombreArticulo"].FillWeight = 45;     // más ancho
            dgvDetalle.Columns["Cantidad"].FillWeight = 15;
            dgvDetalle.Columns["PrecioUnitario"].FillWeight = 15;
            dgvDetalle.Columns["SubTotal"].FillWeight = 10;

        }
        private void AplicarEstiloGlobal()
        {
            WinFont fuenteGeneral = new WinFont("Segoe UI", 12F, FontStyle.Regular);
            PdfFont fuentePDF = FontFactory.GetFont(FontFactory.HELVETICA, 13);


            foreach (Control ctrl in this.Controls)
            {
                ctrl.Font = fuenteGeneral;
            }
        }
    }
}