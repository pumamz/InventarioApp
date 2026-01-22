using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ENTITIES;
using SERVICES;

namespace UI
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
        }

        private void CargarDatos()
        {
            var ser = new SERVICES.APIService();

            var categoria = new Categoria { id = 1, nombre = "Accesorios", descripcion = "Accesorios tecnologicos" };
            var lista_productos = new List<Producto>();

            lista_productos.Add(new Producto { id = 1, nombre = "iPhone", stock = 100, precio = 1200, categoria = categoria });
            lista_productos.Add(new Producto { id = 1, nombre = "iPad", stock = 50, precio = 600, categoria = categoria });
            lista_productos.Add(new Producto { id = 1, nombre = "iMac", stock = 20, precio = 900, categoria = categoria });

            this.productoBindingSource.DataSource = lista_productos;
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
            this.CargarDatos();
        }

        private void productoDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
