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
    public partial class FrmMenuPrincipal : Form
    {
        public FrmMenuPrincipal()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void AbrirFormulario(Type tipoFormulario)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.GetType() == tipoFormulario)
                {
                    frm.BringToFront();
                    return;
                }
            }

            Form nuevoFormulario = (Form)Activator.CreateInstance(tipoFormulario);
            nuevoFormulario.MdiParent = this;
            nuevoFormulario.WindowState = FormWindowState.Maximized;
            nuevoFormulario.Show();
        }


        private void btnFactura_Click(object sender, EventArgs e)
        {
            //// Verificar si ya está abierto
            //foreach (Form frm in this.MdiChildren)
            //{
            //    if (frm is FrmFactura)
            //    {
            //        frm.BringToFront();
            //        return;
            //    }
            //}

            //// Crear y mostrar el formulario como hijo MDI
            //FrmFactura factura = new FrmFactura();
            //factura.MdiParent = this;
            //factura.WindowState = FormWindowState.Maximized;
            //factura.Show();

            // Verificar si ya está abierto
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FrmFactura abierto)
                {
                    abierto.BringToFront();
                    abierto.Activate(); // asegura que sea el hijo activo

                    // Forzar foco tras la activación
                    abierto.BeginInvoke(new Action(() =>
                    {
                        abierto.SetInitialFocus(); // usa el método público del hijo
                    }));
                    return;
                }
            }

            // Crear y mostrar el formulario como hijo MDI
            FrmFactura factura = new FrmFactura
            {
                MdiParent = this,
                WindowState = FormWindowState.Maximized
            };
            factura.Show();
            factura.Activate();

            // Forzar foco tras mostrar y activar
            factura.BeginInvoke(new Action(() =>
            {
                factura.SetInitialFocus();
            }));
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmClientes));
        }

        private void btnArticulos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmCompras));
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmHistorialArticulo));
        }
        private void btnProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmProveedores));
        }
        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmReporteInventario));
        }

        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(30, 30, 35);
            panelMenu.BackColor = Color.FromArgb(45, 45, 48);

            foreach (Control ctrl in panelMenu.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = Color.FromArgb(60, 60, 65);
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.Padding = new Padding(10, 0, 0, 0);
                    btn.Height = 45;
                }
            }
        }

        private void btnReporteCompras_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmReporteCompras));
        }


        private void btnToggleMenu_Click(object sender, EventArgs e)
        {
            if (panelMenu.Width > 50)
            {
                panelMenu.Width = 50; // colapsado (solo ícono)
            }
            else
            {
                panelMenu.Width = 200; // expandido
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormulario(typeof(FrmReporteVentas));
        }
    }
}
