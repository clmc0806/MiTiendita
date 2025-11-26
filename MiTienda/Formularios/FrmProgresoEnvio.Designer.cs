namespace MiTienda.Formularios
{
    partial class FrmProgresoEnvio
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
            this.components = new System.ComponentModel.Container();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timerAutoCerrar = new System.Windows.Forms.Timer(this.components);
            this.lblEstado = new System.Windows.Forms.Label();
            this.picAdvertencia = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAdvertencia)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 70);
            this.progressBar1.MarqueeAnimationSpeed = 30;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(387, 44);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            // 
            // timerAutoCerrar
            // 
            this.timerAutoCerrar.Interval = 30000;
            this.timerAutoCerrar.Tick += new System.EventHandler(this.timerAutoCerrar_Tick);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(0, 0);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(264, 20);
            this.lblEstado.TabIndex = 2;
            this.lblEstado.Text = "Enviando factura, por favor espere...";
            // 
            // picAdvertencia
            // 
            this.picAdvertencia.Location = new System.Drawing.Point(175, 32);
            this.picAdvertencia.Name = "picAdvertencia";
            this.picAdvertencia.Size = new System.Drawing.Size(32, 32);
            this.picAdvertencia.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAdvertencia.TabIndex = 3;
            this.picAdvertencia.TabStop = false;
            this.picAdvertencia.Visible = false;
            // 
            // FrmProgresoEnvio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 114);
            this.ControlBox = false;
            this.Controls.Add(this.picAdvertencia);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmProgresoEnvio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Progreso";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmProgresoEnvio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picAdvertencia)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timerAutoCerrar;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.PictureBox picAdvertencia;
    }
}