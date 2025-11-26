using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTienda.Entidades
{
    public class ArticuloDTO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        public decimal PrecioPublico { get; set; }
        public int Cantidad { get; set; }
        public string Observacion { get; set; }
        public decimal Subtotal => Costo * Cantidad;
    }
}
