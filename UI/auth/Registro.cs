using SERVICES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.auth
{
    public partial class Registro : Form
    {
        public Registro()
        {
            InitializeComponent();
        }

        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Complete todos los campos");
                return;
            }

            var api = new ApiService();

            string error = await api.RegistrarAsync(txtUsuario.Text, txtEmail.Text, txtPassword.Text);

            if (error == null)
            {
                MessageBox.Show("¡Usuario registrado con éxito! Ahora puede iniciar sesión.");
                this.Close();
            }
            else
            {
                MessageBox.Show(error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
