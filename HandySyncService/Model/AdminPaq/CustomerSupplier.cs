using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HandySyncService.Model
{
    [DataContract]
    public class CustomerSupplier
    {
        public string cCodigoCliente { get; set; }
        public string cRazonSocial { get; set; }
        public string cFechaAlta { get; set; }
        public string cRFC { get; set; }
        public string cCURP { get; set; }
        public string cDenComercial { get; set; }
        public string cRepLegal { get; set; }
        public string cNombreMoneda { get; set; }
        public long cListaPreciosCliente { get; set; } // No 0
        public double cDescuentoMovto { get; set; }
        public long cBanVentaCredito { get; set; }      // 0 = No se permite venta a crédito, 1 = Se permite venta a crédito
        public string cCodigoValorClasificacionCliente1 { get; set; }
        public string cCodigoValorClasificacionCliente2 { get; set; }
        public string cCodigoValorClasificacionCliente3 { get; set; }
        public string cCodigoValorClasificacionCliente4 { get; set; }
        public string cCodigoValorClasificacionCliente5 { get; set; }
        public string cCodigoValorClasificacionCliente6 { get; set; }
        public long cTipoCliente { get; set; } // 1 - Cliente, 2 - Cliente/Proveedor, 3 - Proveedor
        public long cEstatus { get; set; } // 0. Inactivo, 1. Activo
        public string cFechaBaja { get; set; }
        public string cFechaUltimaRevision { get; set; }
        public double cLimiteCreditoCliente { get; set; }
        public long cDiasCreditoCliente { get; set; }
        public long cBanExcederCredito { get; set; } // 0 = No se permite exceder crédito, 1 = Se permite exceder el crédito
        public double cDescuentoProntoPago { get; set; }
        public long cDiasProntoPago { get; set; }
        public double cInteresMoratorio { get; set; }
        public long cDiaPago { get; set; }
        public long cDiasRevision { get; set; }
        public string cMensajeria { get; set; }
        public string cCuentaMensajeria { get; set; }
        public long cDiasEmbarqueCliente { get; set; }
        public string cCodigoAlmacen { get; set; }
        public string cCodigoAgenteVenta { get; set; }
        public string cCodigoAgenteCobro { get; set; }
        public long cRestriccionAgente { get; set; }
        public double cImpuesto1 { get; set; }
        public double cImpuesto2 { get; set; }
        public double cImpuesto3 { get; set; }
        public double cRetencionCliente1 { get; set; }
        public double cRetencionCliente2 { get; set; }
        public string cCodigoValorClasificacionProveedor1 { get; set; }
        public string cCodigoValorClasificacionProveedor2 { get; set; }
        public string cCodigoValorClasificacionProveedor3 { get; set; }
        public string cCodigoValorClasificacionProveedor4 { get; set; }
        public string cCodigoValorClasificacionProveedor5 { get; set; }
        public string cCodigoValorClasificacionProveedor6 { get; set; }
        public double cLimiteCreditoProveedor { get; set; }
        public long cDiasCreditoProveedor { get; set; }
        public long cTiempoEntrega { get; set; }
        public long cDiasEmbarqueProveedor { get; set; }
        public double cImpuestoProveedor1 { get; set; }
        public double cImpuestoProveedor2 { get; set; }
        public double cImpuestoProveedor3 { get; set; }
        public double cRetencionProveedor1 { get; set; }
        public double cRetencionProveedor2 { get; set; }
        public long cBanInteresMoratorio { get; set; } // 0 = No se le calculan intereses moratorios al cliente, 1 = Si se le calculan intereses moratorios al cliente.
        public string cTextoExtra1 { get; set; }
        public string cTextoExtra2 { get; set; }
        public string cTextoExtra3 { get; set; }
        public string cFechaExtra { get; set; }
        public double cImporteExtra1 { get; set; }
        public double cImporteExtra2 { get; set; }
        public double cImporteExtra3 { get; set; }
        public double cImporteExtra4 { get; set; }
    }
}
