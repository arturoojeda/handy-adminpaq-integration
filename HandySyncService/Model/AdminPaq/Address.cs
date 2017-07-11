using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService.Model
{
    [DataContract]
    public class Address
    {
        public string cCodCteProv { get; set; }
        public long cTipoCatalogo { get; set; }
        public long cTipoDireccion { get; set; }
        public string cNombreCalle { get; set; }
        public string cNumeroExterior { get; set; }
        public string cNumeroInterior { get; set; }
        public string cColonia { get; set; }
        public string cCodigoPostal { get; set; }
        public string cTelefono1 { get; set; }
        public string cTelefono2 { get; set; }
        public string cTelefono3 { get; set; }
        public string cTelefono4 { get; set; }
        public string cEmail { get; set; }
        public string cDireccionWeb { get; set; }
        public string cCiudad { get; set; }
        public string cEstado { get; set; }
        public string cPais { get; set; }
        public string cTextoExtra { get; set; }
    }
}
