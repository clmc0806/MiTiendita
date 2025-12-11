namespace MiTienda.Formularios
{
    partial class FrmHistorialArticulo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBuscarArticulo = new System.Windows.Forms.TextBox();
            this.dgvCoincidenciass = new System.Windows.Forms.DataGridView();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.lblInfoArticulo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLimpiarBusqueda = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFiltrarFechas = new System.Windows.Forms.Button();
            this.chkFiltrarPorFechas = new System.Windows.Forms.CheckBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnExportarHistorial = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCoincidenciass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBuscarArticulo
            // 
            this.txtBuscarArticulo.Location = new System.Drawing.Point(161, 40);
            this.txtBuscarArticulo.Name = "txtBuscarArticulo";
            this.txtBuscarArticulo.Size = new System.Drawing.Size(184, 20);
            this.txtBuscarArticulo.TabIndex = 0;
            this.txtBuscarArticulo.TextChanged += new System.EventHandler(this.txtBuscarArticulo_TextChanged);
            // 
            // dgvCoincidenciass
            // 
            this.dgvCoincidenciass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCoincidenciass.Location = new System.Drawing.Point(161, 107);
            this.dgvCoincidenciass.Name = "dgvCoincidenciass";
            this.dgvCoincidenciass.Size = new System.Drawing.Size(871, 150);
            this.dgvCoincidenciass.TabIndex = 1;
           
            // 
            // dgvHistorial
            // 
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Location = new System.Drawing.Point(161, 311);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.Size = new System.Drawing.Size(871, 150);
            this.dgvHistorial.TabIndex = 2;
            // 
            // lblInfoArticulo
            // 
            this.lblInfoArticulo.AutoSize = true;
            this.lblInfoArticulo.Location = new System.Drawing.Point(158, 79);
            this.lblInfoArticulo.Name = "lblInfoArticulo";
            this.lblInfoArticulo.Size = new System.Drawing.Size(45, 13);
            this.lblInfoArticulo.TabIndex = 3;
            this.lblInfoArticulo.Text = "Detalles";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(158, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Buscar articulo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Historial";
            // 
            // btnLimpiarBusqueda
            // 
            this.btnLimpiarBusqueda.Location = new System.Drawing.Point(366, 37);
            this.btnLimpiarBusqueda.Name = "btnLimpiarBusqueda";
            this.btnLimpiarBusqueda.Size = new System.Drawing.Size(97, 23);
            this.btnLimpiarBusqueda.TabIndex = 6;
            this.btnLimpiarBusqueda.Text = "LIMPIAR";
            this.btnLimpiarBusqueda.UseVisualStyleBackColor = true;
            
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.Location = new System.Drawing.Point(827, 40);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(189, 23);
            this.btnExportarExcel.TabIndex = 7;
            this.btnExportarExcel.Text = "EXPORTAR ARTICULO";
            this.btnExportarExcel.UseVisualStyleBackColor = true;
           
            // 
            // dtpDesde
            // 
            this.dtpDesde.Location = new System.Drawing.Point(9, 178);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(104, 20);
            this.dtpDesde.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Desde";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(9, 253);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(104, 20);
            this.dtpHasta.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Hasta";
            // 
            // btnFiltrarFechas
            // 
            this.btnFiltrarFechas.Location = new System.Drawing.Point(9, 305);
            this.btnFiltrarFechas.Name = "btnFiltrarFechas";
            this.btnFiltrarFechas.Size = new System.Drawing.Size(104, 23);
            this.btnFiltrarFechas.TabIndex = 12;
            this.btnFiltrarFechas.Text = "Ejecutar";
            this.btnFiltrarFechas.UseVisualStyleBackColor = true;
            this.btnFiltrarFechas.Click += new System.EventHandler(this.btnFiltrarFechas_Click);
            // 
            // chkFiltrarPorFechas
            // 
            this.chkFiltrarPorFechas.AutoSize = true;
            this.chkFiltrarPorFechas.Location = new System.Drawing.Point(12, 107);
            this.chkFiltrarPorFechas.Name = "chkFiltrarPorFechas";
            this.chkFiltrarPorFechas.Size = new System.Drawing.Size(112, 17);
            this.chkFiltrarPorFechas.TabIndex = 13;
            this.chkFiltrarPorFechas.Text = "Reporte por fecha";
            this.chkFiltrarPorFechas.UseVisualStyleBackColor = true;
            
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(9, 438);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(104, 23);
            this.btnCerrar.TabIndex = 14;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            
            // 
            // btnExportarHistorial
            // 
            this.btnExportarHistorial.Location = new System.Drawing.Point(827, 280);
            this.btnExportarHistorial.Name = "btnExportarHistorial";
            this.btnExportarHistorial.Size = new System.Drawing.Size(189, 23);
            this.btnExportarHistorial.TabIndex = 15;
            this.btnExportarHistorial.Text = "EXPORTAR HISTORIAL";
            this.btnExportarHistorial.UseVisualStyleBackColor = true;
            
            // 
            // FrmHistorialArticulo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 488);
            this.Controls.Add(this.btnExportarHistorial);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.chkFiltrarPorFechas);
            this.Controls.Add(this.btnFiltrarFechas);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.btnExportarExcel);
            this.Controls.Add(this.btnLimpiarBusqueda);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblInfoArticulo);
            this.Controls.Add(this.dgvHistorial);
            this.Controls.Add(this.dgvCoincidenciass);
            this.Controls.Add(this.txtBuscarArticulo);
            this.Name = "FrmHistorialArticulo";
            this.Text = "FrmHistorialArticulo";
           
            ((System.ComponentModel.ISupportInitialize)(this.dgvCoincidenciass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBuscarArticulo;
        private System.Windows.Forms.DataGridView dgvCoincidenciass;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.Label lblInfoArticulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLimpiarBusqueda;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnFiltrarFechas;
        private System.Windows.Forms.CheckBox chkFiltrarPorFechas;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnExportarHistorial;
    }
}