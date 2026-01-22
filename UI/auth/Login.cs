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
using UI.producto;

namespace UI.auth
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Ingrese email y contraseña");
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Verificando...";

            var api = new ApiService();
            string error = await api.LoginAsync(txtEmail.Text, txtPassword.Text);

            if (error == null)
            {
                // Abrimos el formulario principal (FrmInicio o el Dashboard que creamos)
                Productos principal = new Productos();
                principal.Show();

                // Ocultamos el login en lugar de cerrarlo para no matar la app
                this.Hide();

                principal.FormClosed += (s, args) => this.Close();
            }
            else
            {
                MessageBox.Show(error);
                btnLogin.Enabled = true;
                btnLogin.Text = "Ingresar";
            }
        }

        private void btnIrRegistro_Click(object sender, EventArgs e)
        {
            Registro registro = new Registro();
            registro.ShowDialog();
        }
    }
}
