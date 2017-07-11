using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService.Model
{
    [DataContract]
    public class Product
    {
        public string cCodigoProducto { get; set; }
        public string cNombreProducto { get; set; }
        public string cDescripcionProducto { get; set; }
        public string cDesccorta { get; set; }
        public long cTipoProducto { get; set; }
        public string cFechaAltaProducto { get; set; }
        public string cFechaBaja { get; set; }
        public long cStatusProducto { get; set; }
        public long cControlExistencia { get; set; }
        public long cMetodoCosteo { get; set; }
        public string cCodigoUnidadBase { get; set; }
        public string cCodigoUnidadNoConvertible { get; set; }
        public double cPrecio1 { get; set; }
        public double cPrecio2 { get; set; }
        public double cPrecio3 { get; set; }
        public double cPrecio4 { get; set; }
        public double cPrecio5 { get; set; }
        public double cPrecio6 { get; set; }
        public double cPrecio7 { get; set; }
        public double cPrecio8 { get; set; }
        public double cPrecio9 { get; set; }
        public double cPrecio10 { get; set; }
        public double cImpuesto1 { get; set; }
        public double cImpuesto2 { get; set; }
        public double cImpuesto3 { get; set; }
        public double cRetencion1 { get; set; }
        public double cRetencion2 { get; set; }
        public string cNombreCaracteristica1 { get; set; }
        public string cNombreCaracteristica2 { get; set; }
        public string cNombreCaracteristica3 { get; set; }
        public string cCodigoValorClasificacion1 { get; set; }
        public string cCodigoValorClasificacion2 { get; set; }
        public string cCodigoValorClasificacion3 { get; set; }
        public string cCodigoValorClasificacion4 { get; set; }
        public string cCodigoValorClasificacion5 { get; set; }
        public string cCodigoValorClasificacion6 { get; set; }
        public string cTextoExtra1 { get; set; }
        public string cTextoExtra2 { get; set; }
        public string cTextoExtra3 { get; set; }
        public string cFechaExtra { get; set; }
        public double cImporteExtra1 { get; set; }
        public double cImporteExtra2 { get; set; }
        public double cImporteExtra3 { get; set; }
        public double cImporteExtra4 { get; set; }
        public string cCodaltern { get; set; }
    }
}
