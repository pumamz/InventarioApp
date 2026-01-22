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
using UI.categoria;

namespace UI.producto
{

    public partial class Productos : Form
    {

        private ApiService _apiService;

        private int? _idProductoSeleccionado = null;

        public Productos()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void FrmProductos_Load(object sender, EventArgs e)
        {
            ConfigurarGrilla();
            await CargarCategorias();
            await CargarProductos();
        }

        private void ConfigurarGrilla()
        {
            dgvProductos.AutoGenerateColumns = false;

            if (dgvProductos.Columns.Count == 0)
            {
                dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 50 });
                dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Nombre", Width = 150 });
                dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Precio", HeaderText = "Precio", Width = 80 });
                dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Stock", HeaderText = "Stock", Width = 80 });
                dgvProductos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CategoriaId", HeaderText = "Cat. ID", Width = 60 });
            }
        }

        private async Task CargarCategorias()
        {
            try
            {
                var categorias = await _apiService.ObtenerCategoriasAsync();

                cmbCategorias.DataSource = categorias;
                cmbCategorias.DisplayMember = "Nombre";
                cmbCategorias.ValueMember = "Id";

                cmbCategorias.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
        }

        private async Task CargarProductos()
        {
            try
            {
                var productos = await _apiService.ObtenerProductosAsync();
                dgvProductos.DataSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text) ||
                cmbCategorias.SelectedValue == null)
            {
                MessageBox.Show("Por favor complete todos los campos.");
                return;
            }

            var producto = new Producto
            {
                Nombre = txtNombre.Text,
                Precio = double.Parse(txtPrecio.Text),
                Stock = int.Parse(txtStock.Text),
                CategoriaId = (int)cmbCategorias.SelectedValue
            };

            bool exito = false;

            try
            {
                if (_idProductoSeleccionado == null)
                {
                    exito = await _apiService.CrearProductoAsync(producto);
                    if (exito) MessageBox.Show("Producto creado correctamente.");
                }
                else
                {
                    producto.Id = _idProductoSeleccionado.Value;
                    exito = await _apiService.ActualizarProductoAsync(producto);
                    if (exito) MessageBox.Show("Producto actualizado correctamente.");
                }

                if (exito)
                {
                    LimpiarFormulario();
                    await CargarProductos();
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el producto. Verifique la API.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvProductos.Rows[e.RowIndex];

                var productoSeleccionado = (Producto)fila.DataBoundItem;

                _idProductoSeleccionado = productoSeleccionado.Id;
                txtNombre.Text = productoSeleccionado.Nombre;
                txtPrecio.Text = productoSeleccionado.Precio.ToString();
                txtStock.Text = productoSeleccionado.Stock.ToString();
                cmbCategorias.SelectedValue = productoSeleccionado.CategoriaId;

                btnGuardar.Text = "Actualizar";
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idProductoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un producto de la lista primero.");
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro de eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    bool exito = await _apiService.EliminarProductoAsync(_idProductoSeleccionado.Value);
                    if (exito)
                    {
                        MessageBox.Show("Producto eliminado.");
                        LimpiarFormulario();
                        await CargarProductos();
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            _idProductoSeleccionado = null;
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            cmbCategorias.SelectedIndex = -1;
            btnGuardar.Text = "Guardar";
            txtNombre.Focus();
        }

        private void btnIrCategoria_Click(object sender, EventArgs e)
        {
            Categorias categoria = new Categorias();
            categoria.ShowDialog();
        }

    }

}
