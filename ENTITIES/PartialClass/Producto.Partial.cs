using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES
{
    public partial class Producto
    {
        public string descripcionCategoria { get { return this.categoria.nombre; } }
    }
}
