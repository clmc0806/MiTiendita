using System;
using System.Windows.Forms;
using System.Drawing;


namespace MiTienda.Formularios
{
    public partial class FrmProgresoEnvio : Form
    {
        public FrmProgresoEnvio()
        {
            InitializeComponent();
        }

        private void FrmProgresoEnvio_Load(object sender, EventArgs e)
        {
            timerAutoCerrar.Enabled = true;
        }

        // Mostrar estado genérico
        public void MostrarEstado(string texto, Color color)
        {
            lblEstado.Text = texto;
            lblEstado.ForeColor = color;
            picAdvertencia.Visible = false;
        }
        // Mostrar advertencia con ícono
        public void MostrarAdvertencia(string texto)
        {
            lblEstado.Text = texto;
            lblEstado.ForeColor = Color.DarkRed;
            picAdvertencia.Image = SystemIcons.Warning.ToBitmap();
            picAdvertencia.Visible = true;
        }
        // Mostrar error con ícono de error
        public void MostrarError(string texto)
        {
            lblEstado.Text = texto;
            lblEstado.ForeColor = Color.Red;
            picAdvertencia.Image = SystemIcons.Error.ToBitmap();
            picAdvertencia.Visible = true;
        }

        private void timerAutoCerrar_Tick(object sender, EventArgs e)
        {
            timerAutoCerrar.Enabled = false;
            lblEstado.Text = "Tiempo de espera agotado";
            lblEstado.ForeColor = Color.DarkRed;
            picAdvertencia.Image = SystemIcons.Warning.ToBitmap();
            picAdvertencia.Visible = true;

            Application.DoEvents(); // fuerza actualización visual
            System.Threading.Thread.Sleep(3000); // espera 2 segundos para que el usuario lo vea
            this.Close(); ;
        }
    }
}
