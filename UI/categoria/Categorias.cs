using ENTITIES;
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

namespace UI.categoria
{
    public partial class Categorias : Form
    {

        private ApiService _apiService;
        private int? _idCategoriaSeleccionada = null;

        public Categorias()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void FrmCategorias_Load(object sender, EventArgs e)
        {
            await CargarCategorias();
        }

        private async Task CargarCategorias()
        {
            try
            {
                var lista = await _apiService.ObtenerCategoriasAsync();
                dgvCategorias.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es requerido.");
                return;
            }

            var categoria = new Categoria { Nombre = txtNombre.Text };
            bool exito;

            try
            {
                if (_idCategoriaSeleccionada == null)
                {
                    exito = await _apiService.CrearCategoriaAsync(categoria);
                }
                else
                {
                    categoria.Id = _idCategoriaSeleccionada.Value;
                    exito = await _apiService.ActualizarCategoriaAsync(categoria);
                }

                if (exito)
                {
                    MessageBox.Show("Operación exitosa");
                    LimpiarFormulario();
                    await CargarCategorias();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvCategorias.Rows[e.RowIndex];
                var cat = (Categoria)fila.DataBoundItem;

                _idCategoriaSeleccionada = cat.Id;
                txtNombre.Text = cat.Nombre;
                btnGuardar.Text = "Actualizar";
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idCategoriaSeleccionada == null) return;

            var confirm = MessageBox.Show("¿Eliminar categoría?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bool exito = await _apiService.EliminarCategoriaAsync(_idCategoriaSeleccionada.Value);
                if (exito)
                {
                    LimpiarFormulario();
                    await CargarCategorias();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            _idCategoriaSeleccionada = null;
            txtNombre.Clear();
            btnGuardar.Text = "Guardar";
        }
    }
}
