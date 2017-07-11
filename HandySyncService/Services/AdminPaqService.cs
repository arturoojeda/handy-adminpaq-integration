using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.InteropServices;
using HandySyncService.Model;
using System.Globalization;
using System.Configuration;
using HandySyncService.Services;
using System.Runtime.ExceptionServices;

namespace HandySyncService.Services
{
    public class AdminPaqService : IAdminPaqService
    {
        private static readonly Lazy<AdminPaqService> lazy =
            new Lazy<AdminPaqService>(() => new AdminPaqService());

        public static AdminPaqService Instance { get { return lazy.Value; } }

        private AdminPaqService()
        {
        }

        #region AdminPAQ Constants
        //___________________DECLARACIÓN DE CONSTANTES DE LONGITUD____________________
        //
        // Se adiciona 1 (+ 1) a la longitud original contemplando
        // el caracter nulo necesario al final de la cadena.
        public const int kLongCodigo = 30 + 1;
        public const int kLongNombre = 60 + 1;
        public const int kLongNombreProducto = 255 + 1;
        public const int kLongFecha = 23 + 1;
        public const int kLongAbreviatura = 3 + 1;
        public const int kLongCodValorClasif = 3 + 1;
        public const int kLongTextoExtra = 50 + 1;
        public const int kLongNumSerie = 11 + 1;
        public const int kLongReferencia = 20 + 1;
        public const int kLongSeries = 30 + 1;
        public const int kLongDescripcion = 60 + 1;
        public const int kLongNumeroExtInt = 6 + 1;
        public const int kLongCodigoPostal = 6 + 1;
        public const int kLongTelefono = 15 + 1;
        public const int kLongEmailWeb = 50 + 1;
        public const int kLongRFC = 20 + 1;
        public const int kLongCURP = 20 + 1;
        public const int kLongDesCorta = 20 + 1;
        public const int kLongDenComercial = 50 + 1;
        public const int kLongRepLegal = 50 + 1;
        #endregion


        #region Private Methods
        private string GetStringField(string sTable, string sFieldName, int iValueSize)
        {
            string sValue = "";
            StringBuilder refValor = new StringBuilder(iValueSize);
            int iResult = 0;
            switch (sTable)
            {
                case "ClientSupplier":
                    iResult = CompacSDK.fLeeDatoCteProv(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Product":
                    iResult = CompacSDK.fLeeDatoProducto(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Address":
                    iResult = CompacSDK.fLeeDatoDireccion(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Agent":
                    iResult = CompacSDK.fLeeDatoAgente(sFieldName, refValor, refValor.Capacity);
                    break;
            }

            if (iResult == 0)
                sValue = refValor.ToString();
            else
            {
                sValue = string.Format("#Error#-{0}", GetError(iResult)) + ". Errno: " + iResult;
                Console.WriteLine("Field " + sFieldName + " in table " + sTable + " is invalid");
            }
            return sValue;
        }
        private long GetLongField(string sTable, string sFieldName)
        {
            long lValue = 0;
            StringBuilder refValor = new StringBuilder(25);
            int iResult = 0;
            switch (sTable)
            {
                case "ClientSupplier":
                    iResult = CompacSDK.fLeeDatoCteProv(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Product":
                    iResult = CompacSDK.fLeeDatoProducto(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Address":
                    iResult = CompacSDK.fLeeDatoDireccion(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Agent":
                    iResult = CompacSDK.fLeeDatoAgente(sFieldName, refValor, refValor.Capacity);
                    break;
            }

            if (iResult == 0)
            {
                string sValue = refValor.ToString();
                if (sValue.Length != 0)
                {
                    lValue = long.Parse(sValue);
                }
                else
                {
                    lValue = -1;
                }
            }
            else
            {
                lValue = -2;
                Console.WriteLine(string.Format("#Error#-{0}", GetError(iResult)) + ". Errno: " + iResult);
                Console.WriteLine("Field " + sFieldName + " in table " + sTable + " is invalid");
            }
            return lValue;
        }
        private double GetDoubleField(string sTable, string sFieldName)
        {
            double dValue = 0;
            StringBuilder refValor = new StringBuilder(25);
            int iResult = 0;
            switch (sTable)
            {
                case "ClientSupplier":
                    iResult = CompacSDK.fLeeDatoCteProv(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Product":
                    iResult = CompacSDK.fLeeDatoProducto(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Address":
                    iResult = CompacSDK.fLeeDatoDireccion(sFieldName, refValor, refValor.Capacity);
                    break;
                case "Agent":
                    iResult = CompacSDK.fLeeDatoAgente(sFieldName, refValor, refValor.Capacity);
                    break;
            }

            if (iResult == 0)
            {
                string sValue = refValor.ToString();
                if (sValue.Length != 0)
                    dValue = double.Parse(sValue);
                else
                    dValue = -1;
            }
            else
            {
                dValue = -2;
                Console.WriteLine(string.Format("#Error#-{0}", GetError(iResult)) + ". Errno: " + iResult);
                Console.WriteLine("Field " + sFieldName + " in table " + sTable + " is invalid");
            }
            return dValue;
        }
        private void SetField(string sTable, string sFieldName, string sValue, string sDataType)
        {
            if (sValue == null || sValue == "")
            {
                switch (sDataType)
                {
                    case "string":
                        sValue = "";
                        break;
                    case "long":
                    case "double":
                        sValue = "0";
                        break;
                    case "date":
                        sValue = "12/30/1899";// MM/dd/yyyy HH:mm:ss:fff
                        break;
                }

            }
            int iResult = -1;
            switch (sTable)
            {
                case "ClientSupplier":
                    iResult = CompacSDK.fSetDatoCteProv(sFieldName, sValue);
                    break;
                case "Product":
                    iResult = CompacSDK.fSetDatoProducto(sFieldName, sValue);
                    break;
            }
            if (iResult != 0)
            {
                //Reportar error
                string sError = GetError(iResult);
                Console.WriteLine("Error writing field " + ((iResult == -1) ? " - Incorrect Table." : (sFieldName + ": " + sError)));
            }
        }

        #endregion

        #region General purpose methods
        public string GetError(int iError)
        {
            string sResult = "";
            StringBuilder sbError = new StringBuilder(351);
            CompacSDK.fError(iError, sbError, sbError.Capacity);
            sResult = sbError.ToString();
            return sResult;
        }
        #endregion

        #region Structs


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tDocumento
        {
            public Double aFolio;
            public int aNumMoneda;
            public Double aTipoCambio;
            public Double aImporte;
            public Double aDescuentoDoc1;
            public Double aDescuentoDoc2;
            public int aSistemaOrigen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public String aCodConcepto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongSeries)]
            public String aSerie;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongFecha)]
            public String aFecha;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public String aCodigoCteProv;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public String aCodigoAgente;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongReferencia)]
            public String aReferencia;
            public int aAfecta;
            public int aGasto1;
            public int aGasto2;
            public int aGasto3;

        }


        #endregion

        #region Communication methods
        /// <summary>
        /// Method to get the latest clients from AdminPaq from a given date.
        /// </summary>
        /// <param name="lstClientes">List of clients</param>
        /// <param name="sFrom">Date from which AdminPaq will get the latest clients</param>
        /// <returns>Returns 0 if the method is successful</returns>
        [HandleProcessCorruptedStateExceptions]
        public int GetClientChanges(ref List<CustomerSupplier> lstClientes, string sFrom)
        {
            Logger log = new Logger();
            log.Trace("Searching for clients");
            int iReturn = 0;
            try
            {
                int iResult = CompacSDK.fPosPrimerCteProv();
                DateTime dtFrom;
                if (sFrom != "")
                    dtFrom = DateTime.Parse(sFrom, System.Globalization.CultureInfo.CreateSpecificCulture("es-MX"));
                else
                    dtFrom = DateTime.MinValue;
                if (iResult == 0)
                {
                    log.Trace("Everything is all right, we can start");
                    do
                    {
                        CustomerSupplier oCteProv = new CustomerSupplier();
                        oCteProv.cFechaAlta = GetStringField("ClientSupplier", "cFechaAlta", kLongFecha);
                        string[] test = oCteProv.cFechaAlta.Split(new Char[] { '/' });
                        if (test[0].Length == 1)
                            test[0] = "0" + test[0];
                        if (test[1].Length == 1)
                            test[1] = "0" + test[1];
                        oCteProv.cFechaAlta = test[0] + "/" + test[1] + "/" + test[2];

                        DateTime dtAlta = DateTime.ParseExact(oCteProv.cFechaAlta, "MM/dd/yyyy HH:mm:ss:fff", CultureInfo.InvariantCulture);
                        if (dtAlta.Date >= dtFrom.Date)
                        {
                            oCteProv.cCodigoCliente = GetStringField("ClientSupplier", "cCodigoCliente", kLongCodigo);
                            //log.trace("Client found: " + oCteProv.cCodigoCliente);//esta línea puede generar muchas lineas de Logger, en especial cuando se corre la primera sincronización, evaluar su utilidad antes de descomentar.
                            oCteProv.cRazonSocial = GetStringField("ClientSupplier", "cRazonSocial", kLongNombre);
                            oCteProv.cRFC = GetStringField("ClientSupplier", "cRFC", kLongRFC);
                            oCteProv.cCURP = GetStringField("ClientSupplier", "cCURP", kLongCURP);
                            oCteProv.cDenComercial = GetStringField("ClientSupplier", "cDenComercial", kLongDenComercial);
                            oCteProv.cRepLegal = GetStringField("ClientSupplier", "cRepLegal", kLongRepLegal);
                            oCteProv.cNombreMoneda = GetStringField("ClientSupplier", "cNombreMoneda", kLongNombre);
                            oCteProv.cListaPreciosCliente = GetLongField("ClientSupplier", "cListaPreciosCliente");
                            oCteProv.cDescuentoMovto = GetDoubleField("ClientSupplier", "cDescuentoMovto");
                            oCteProv.cBanVentaCredito = GetLongField("ClientSupplier", "cBanVentaCredito");
                            oCteProv.cCodigoValorClasificacionCliente1 = GetStringField("ClientSupplier", "cCodigoValorClasificacionCliente1", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionCliente2 = GetStringField("ClientSupplier", "cCodigoValorClasificacionCliente2", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionCliente3 = GetStringField("ClientSupplier", "cCodigoValorClasificacionCliente3", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionCliente4 = GetStringField("ClientSupplier", "cCodigoValorClasificacionCliente4", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionCliente5 = GetStringField("ClientSupplier", "cCodigoValorClasificacionCliente5", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionCliente6 = GetStringField("ClientSupplier", "cCodigoValorClasificacionCliente6", kLongCodValorClasif);
                            oCteProv.cTipoCliente = GetLongField("ClientSupplier", "cTipoCliente");
                            oCteProv.cEstatus = GetLongField("ClientSupplier", "cEstatus");
                            oCteProv.cFechaBaja = GetStringField("ClientSupplier", "cFechaBaja", kLongFecha);
                            oCteProv.cFechaUltimaRevision = GetStringField("ClientSupplier", "cFechaUltimaRevision", kLongFecha);
                            oCteProv.cLimiteCreditoCliente = GetDoubleField("ClientSupplier", "cLimiteCreditoCliente");
                            oCteProv.cDiasCreditoCliente = GetLongField("ClientSupplier", "cDiasCreditoCliente");
                            oCteProv.cBanExcederCredito = GetLongField("ClientSupplier", "cBanExcederCredito");
                            oCteProv.cDescuentoProntoPago = GetDoubleField("ClientSupplier", "cDescuentoProntoPago");
                            oCteProv.cDiasProntoPago = GetLongField("ClientSupplier", "cDiasProntoPago");
                            oCteProv.cInteresMoratorio = GetDoubleField("ClientSupplier", "cInteresMoratorio");
                            oCteProv.cDiaPago = GetLongField("ClientSupplier", "cDiaPago");
                            oCteProv.cDiasRevision = GetLongField("ClientSupplier", "cDiasRevision");
                            oCteProv.cMensajeria = GetStringField("ClientSupplier", "cMensajeria", kLongDesCorta);
                            oCteProv.cCuentaMensajeria = GetStringField("ClientSupplier", "cCuentaMensajeria", kLongDescripcion);
                            oCteProv.cDiasEmbarqueCliente = GetLongField("ClientSupplier", "cDiasEmbarqueCliente");
                            oCteProv.cCodigoAlmacen = GetStringField("ClientSupplier", "cCodigoAlmacen", kLongDescripcion);
                            oCteProv.cCodigoAgenteVenta = GetStringField("ClientSupplier", "cCodigoAgenteVenta", kLongCodigo);
                            oCteProv.cCodigoAgenteCobro = GetStringField("ClientSupplier", "cCodigoAgenteCobro", kLongCodigo);
                            oCteProv.cRestriccionAgente = GetLongField("ClientSupplier", "cRestriccionAgente");
                            oCteProv.cImpuesto1 = GetDoubleField("ClientSupplier", "cImpuesto1");
                            oCteProv.cImpuesto2 = GetDoubleField("ClientSupplier", "cImpuesto2");
                            oCteProv.cImpuesto3 = GetDoubleField("ClientSupplier", "cImpuesto3");
                            oCteProv.cRetencionCliente1 = GetDoubleField("ClientSupplier", "cRetencionCliente1");
                            oCteProv.cRetencionCliente2 = GetDoubleField("ClientSupplier", "cRetencionCliente2");
                            oCteProv.cCodigoValorClasificacionProveedor1 = GetStringField("ClientSupplier", "cCodigoValorClasificacionProveedor1", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionProveedor2 = GetStringField("ClientSupplier", "cCodigoValorClasificacionProveedor2", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionProveedor3 = GetStringField("ClientSupplier", "cCodigoValorClasificacionProveedor3", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionProveedor4 = GetStringField("ClientSupplier", "cCodigoValorClasificacionProveedor4", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionProveedor5 = GetStringField("ClientSupplier", "cCodigoValorClasificacionProveedor5", kLongCodValorClasif);
                            oCteProv.cCodigoValorClasificacionProveedor6 = GetStringField("ClientSupplier", "cCodigoValorClasificacionProveedor6", kLongCodValorClasif);
                            oCteProv.cLimiteCreditoProveedor = GetDoubleField("ClientSupplier", "cLimiteCreditoProveedor");
                            oCteProv.cDiasCreditoProveedor = GetLongField("ClientSupplier", "cDiasCreditoProveedor");
                            oCteProv.cTiempoEntrega = GetLongField("ClientSupplier", "cTiempoEntrega");
                            oCteProv.cDiasEmbarqueProveedor = GetLongField("ClientSupplier", "cDiasEmbarqueProveedor");
                            oCteProv.cImpuestoProveedor1 = GetDoubleField("ClientSupplier", "cImpuestoProveedor1");
                            oCteProv.cImpuestoProveedor2 = GetDoubleField("ClientSupplier", "cImpuestoProveedor2");
                            oCteProv.cImpuestoProveedor3 = GetDoubleField("ClientSupplier", "cImpuestoProveedor3");
                            oCteProv.cRetencionProveedor1 = GetDoubleField("ClientSupplier", "cRetencionProveedor1");
                            oCteProv.cRetencionProveedor2 = GetDoubleField("ClientSupplier", "cRetencionProveedor2");
                            oCteProv.cBanInteresMoratorio = GetLongField("ClientSupplier", "cBanInteresMoratorio");
                            oCteProv.cTextoExtra1 = GetStringField("ClientSupplier", "cTextoExtra1", kLongTextoExtra);
                            oCteProv.cTextoExtra2 = GetStringField("ClientSupplier", "cTextoExtra2", kLongTextoExtra);
                            oCteProv.cTextoExtra3 = GetStringField("ClientSupplier", "cTextoExtra3", kLongTextoExtra);
                            oCteProv.cFechaExtra = GetStringField("ClientSupplier", "cFechaExtra", kLongFecha);
                            oCteProv.cImporteExtra1 = GetDoubleField("ClientSupplier", "cImporteExtra1");
                            oCteProv.cImporteExtra2 = GetDoubleField("ClientSupplier", "cImporteExtra2");
                            oCteProv.cImporteExtra3 = GetDoubleField("ClientSupplier", "cImporteExtra3");
                            oCteProv.cImporteExtra4 = GetDoubleField("ClientSupplier", "cImporteExtra4");
                            lstClientes.Add(oCteProv);
                        }
                    } while (CompacSDK.fPosSiguienteCteProv() == 0);
                    log.Trace("No more clients");
                }
                else
                {
                    log.Trace("ERROR: it was no possible get the clients, error code: " + iResult);
                    iReturn = iResult;
                }
            }
            catch (Exception ex)
            {
                log.Error("ERROR in method GetClientChanges: " + ex.Message);
                throw ex;
            }
            return iReturn;
        }

        // Not used
        public int AddClient(CustomerSupplier oCteProv)
        {
            int iReturn = 0;
            int iResult = 0;
            iResult = CompacSDK.fInsertaCteProv();
            if (iResult == 0)
            {
                SetField("ClientSupplier", "cCodigoCliente", oCteProv.cCodigoCliente, "string");
                SetField("ClientSupplier", "cRazonSocial", oCteProv.cRazonSocial, "string");
                SetField("ClientSupplier", "cFechaAlta", oCteProv.cFechaAlta, "date");
                SetField("ClientSupplier", "cCURP", oCteProv.cCURP, "string");
                SetField("ClientSupplier", "cDenComercial", oCteProv.cDenComercial, "string");
                SetField("ClientSupplier", "cRepLegal", oCteProv.cRepLegal, "string");
                SetField("ClientSupplier", "cNombreMoneda", oCteProv.cNombreMoneda, "string");
                SetField("ClientSupplier", "cListaPrecioCliente", oCteProv.cListaPreciosCliente.ToString(), "long");
                SetField("ClientSupplier", "cDescuentoMovto", oCteProv.cDescuentoMovto.ToString(), "double");
                SetField("ClientSupplier", "cBanVentaCredito", oCteProv.cBanVentaCredito.ToString(), "long");
                SetField("ClientSupplier", "cCodigoValorClasificacionCliente1", oCteProv.cCodigoValorClasificacionCliente1, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionCliente2", oCteProv.cCodigoValorClasificacionCliente2, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionCliente3", oCteProv.cCodigoValorClasificacionCliente3, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionCliente4", oCteProv.cCodigoValorClasificacionCliente4, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionCliente5", oCteProv.cCodigoValorClasificacionCliente5, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionCliente6", oCteProv.cCodigoValorClasificacionCliente6, "string");
                SetField("ClientSupplier", "cTipoCliente", oCteProv.cTipoCliente.ToString(), "long");
                SetField("ClientSupplier", "cEstatus", oCteProv.cEstatus.ToString(), "long");
                SetField("ClientSupplier", "cFechaBaja", oCteProv.cFechaBaja, "date");
                SetField("ClientSupplier", "cFechaUltimaRevision", oCteProv.cFechaUltimaRevision, "date");
                SetField("ClientSupplier", "cLimiteCreditoCliente", oCteProv.cLimiteCreditoCliente.ToString(), "double");
                SetField("ClientSupplier", "cDiasCreditoCliente", oCteProv.cDiasCreditoCliente.ToString(), "long");
                SetField("ClientSupplier", "cBanExcederCredito", oCteProv.cBanExcederCredito.ToString(), "long");
                SetField("ClientSupplier", "cDescuentoProntoPago", oCteProv.cDescuentoProntoPago.ToString(), "double");
                SetField("ClientSupplier", "cDiasProntoPago", oCteProv.cDiasProntoPago.ToString(), "long");
                SetField("ClientSupplier", "cInteresMoratorio", oCteProv.cInteresMoratorio.ToString(), "double");
                SetField("ClientSupplier", "cDiaPago", oCteProv.cDiaPago.ToString(), "long");
                SetField("ClientSupplier", "cDiasRevision", oCteProv.cDiasRevision.ToString(), "long");
                SetField("ClientSupplier", "cMensajeria", oCteProv.cMensajeria, "string");
                SetField("ClientSupplier", "cCuentaMensajeria", oCteProv.cCuentaMensajeria, "string");
                SetField("ClientSupplier", "cDiasEmbarqueCliente", oCteProv.cDiasEmbarqueCliente.ToString(), "long");
                SetField("ClientSupplier", "cCodigoAlmacen", oCteProv.cCodigoAlmacen, "string");
                SetField("ClientSupplier", "cCodigoAgenteVenta", oCteProv.cCodigoAgenteVenta, "string");
                SetField("ClientSupplier", "cCodigoAgenteCobro", oCteProv.cCodigoAgenteCobro, "string");
                SetField("ClientSupplier", "cRestriccionAgente", oCteProv.cRestriccionAgente.ToString(), "long");
                SetField("ClientSupplier", "cImpuesto1", oCteProv.cImpuesto1.ToString(), "double");
                SetField("ClientSupplier", "cImpuesto2", oCteProv.cImpuesto2.ToString(), "double");
                SetField("ClientSupplier", "cImpuesto3", oCteProv.cImpuesto3.ToString(), "double");
                SetField("ClientSupplier", "cRetencionCliente1", oCteProv.cRetencionCliente1.ToString(), "double");
                SetField("ClientSupplier", "cRetencionCliente2", oCteProv.cRetencionCliente2.ToString(), "double");
                SetField("ClientSupplier", "cCodigoValorClasificacionProveedor1", oCteProv.cCodigoValorClasificacionProveedor1, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionProveedor2", oCteProv.cCodigoValorClasificacionProveedor2, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionProveedor3", oCteProv.cCodigoValorClasificacionProveedor3, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionProveedor4", oCteProv.cCodigoValorClasificacionProveedor4, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionProveedor5", oCteProv.cCodigoValorClasificacionProveedor5, "string");
                SetField("ClientSupplier", "cCodigoValorClasificacionProveedor6", oCteProv.cCodigoValorClasificacionProveedor6, "string");
                SetField("ClientSupplier", "cLimiteCreditoProveedor", oCteProv.cLimiteCreditoProveedor.ToString(), "double");
                SetField("ClientSupplier", "cDiasCreditoProveedor", oCteProv.cDiasCreditoProveedor.ToString(), "long");
                SetField("ClientSupplier", "cTiempoEntrega", oCteProv.cTiempoEntrega.ToString(), "long");
                SetField("ClientSupplier", "cDiasEmbarqueProveedor", oCteProv.cDiasEmbarqueProveedor.ToString(), "long");
                SetField("ClientSupplier", "cImpuestoProveedor1", oCteProv.cImpuestoProveedor1.ToString(), "double");
                SetField("ClientSupplier", "cImpuestoProveedor2", oCteProv.cImpuestoProveedor2.ToString(), "double");
                SetField("ClientSupplier", "cImpuestoProveedor3", oCteProv.cImpuestoProveedor3.ToString(), "double");
                SetField("ClientSupplier", "cRetencionProveedor1", oCteProv.cRetencionProveedor1.ToString(), "double");
                SetField("ClientSupplier", "cRetencionProveedor2", oCteProv.cRetencionProveedor2.ToString(), "double");
                SetField("ClientSupplier", "cBanInteresMoratorio", oCteProv.cBanInteresMoratorio.ToString(), "long");
                SetField("ClientSupplier", "cTextoExtra1", oCteProv.cTextoExtra1, "string");
                SetField("ClientSupplier", "cTextoExtra2", oCteProv.cTextoExtra2, "string");
                SetField("ClientSupplier", "cTextoExtra3", oCteProv.cTextoExtra3, "string");
                SetField("ClientSupplier", "cFechaExtra", oCteProv.cFechaExtra, "date");
                SetField("ClientSupplier", "cImporteExtra1", oCteProv.cImporteExtra1.ToString(), "double");
                SetField("ClientSupplier", "cImporteExtra2", oCteProv.cImporteExtra2.ToString(), "double");
                SetField("ClientSupplier", "cImporteExtra3", oCteProv.cImporteExtra3.ToString(), "double");
                SetField("ClientSupplier", "cImporteExtra4", oCteProv.cImporteExtra4.ToString(), "double");
                iResult = CompacSDK.fGuardaCteProv();
                iReturn = iResult;
            }
            return iReturn;
        }

        /// <summary>
        /// Method that retrieves the latest products from AdminPaq from a given date.
        /// </summary>
        /// <param name="lstProducts">Ref: List of products retrieved</param>
        /// <param name="sFrom">Date from which the products will be retrieved.</param>
        /// <returns>0 in case of Success</returns>
        [HandleProcessCorruptedStateExceptions]
        public int GetProductChanges(ref List<Product> lstProducts, string sFrom)
        {
            Logger log = new Logger();
            int iReturn = 0;
            try
            {
                int iResult = CompacSDK.fPosPrimerProducto();
                DateTime dtFrom;

                if (sFrom != "")
                    dtFrom = DateTime.Parse(sFrom, System.Globalization.CultureInfo.CreateSpecificCulture("es-MX"));
                else
                    dtFrom = DateTime.MinValue;
                if (iResult == 0)
                {
                    //int testLimit = 0;
                    do
                    {
                        Product oProduct = new Product();

                        oProduct.cFechaAltaProducto = GetStringField("Product", "cFechaAltaProducto", kLongFecha);
                        string[] test = oProduct.cFechaAltaProducto.Split(new Char[] { '/' });
                        if (test[0].Length == 1)
                            test[0] = "0" + test[0];
                        if (test[1].Length == 1)
                            test[1] = "0" + test[1];
                        oProduct.cFechaAltaProducto = test[0] + "/" + test[1] + "/" + test[2];
                        DateTime dtAlta = DateTime.ParseExact(oProduct.cFechaAltaProducto, "MM/dd/yyyy HH:mm:ss:fff", CultureInfo.InvariantCulture);

                        if (dtAlta.Date >= dtFrom.Date)
                        {
                            oProduct.cCodigoProducto = GetStringField("Product", "cCodigoProducto", kLongCodigo);
                            oProduct.cNombreProducto = GetStringField("Product", "cNombreProducto", kLongNombre);
                            oProduct.cDescripcionProducto = GetStringField("Product", "cDescripcionProducto", kLongNombreProducto);
                            oProduct.cDesccorta = GetStringField("Product", "cDesccorta", kLongNombreProducto);
                            oProduct.cTipoProducto = GetLongField("Product", "cTipoProducto");
                            oProduct.cFechaAltaProducto = GetStringField("Product", "cFechaAltaProducto", kLongFecha);
                            oProduct.cFechaBaja = GetStringField("Product", "cFechaBaja", kLongFecha);
                            oProduct.cStatusProducto = GetLongField("Product", "cStatusProducto");
                            oProduct.cControlExistencia = GetLongField("Product", "cControlExistencia");
                            oProduct.cMetodoCosteo = GetLongField("Product", "cMetodoCosteo");
                            oProduct.cCodigoUnidadBase = GetStringField("Product", "cCodigoUnidadBase", kLongCodigo);
                            oProduct.cCodigoUnidadNoConvertible = GetStringField("Product", "cCodigoUnidadNoConvertible", kLongCodigo);
                            oProduct.cPrecio1 = GetDoubleField("Product", "cPrecio1");
                            oProduct.cPrecio2 = GetDoubleField("Product", "cPrecio2");
                            oProduct.cPrecio3 = GetDoubleField("Product", "cPrecio3");
                            oProduct.cPrecio4 = GetDoubleField("Product", "cPrecio4");
                            oProduct.cPrecio5 = GetDoubleField("Product", "cPrecio5");
                            oProduct.cPrecio6 = GetDoubleField("Product", "cPrecio6");
                            oProduct.cPrecio7 = GetDoubleField("Product", "cPrecio7");
                            oProduct.cPrecio8 = GetDoubleField("Product", "cPrecio8");
                            oProduct.cPrecio9 = GetDoubleField("Product", "cPrecio9");
                            oProduct.cPrecio10 = GetDoubleField("Product", "cPrecio10");
                            oProduct.cImpuesto1 = GetDoubleField("Product", "cImpuesto1");
                            oProduct.cImpuesto2 = GetDoubleField("Product", "cImpuesto2");
                            oProduct.cImpuesto3 = GetDoubleField("Product", "cImpuesto3");
                            oProduct.cRetencion1 = GetDoubleField("Product", "cRetencion1s");
                            oProduct.cRetencion2 = GetDoubleField("Product", "cRetencion2");
                            oProduct.cNombreCaracteristica1 = GetStringField("Product", "cNombreCaracteristica1", kLongAbreviatura);
                            oProduct.cNombreCaracteristica2 = GetStringField("Product", "cNombreCaracteristica2", kLongAbreviatura);
                            oProduct.cNombreCaracteristica3 = GetStringField("Product", "cNombreCaracteristica3", kLongAbreviatura);
                            oProduct.cCodigoValorClasificacion1 = GetStringField("Product", "CIDVALOR01", 11);
                            oProduct.cCodigoValorClasificacion2 = GetStringField("Product", "CIDVALOR02", 11);
                            oProduct.cCodigoValorClasificacion3 = GetStringField("Product", "CIDVALOR03", 11);
                            oProduct.cCodigoValorClasificacion4 = GetStringField("Product", "CIDVALOR04", 11);
                            oProduct.cCodigoValorClasificacion5 = GetStringField("Product", "CIDVALOR05", 11);
                            oProduct.cCodigoValorClasificacion6 = GetStringField("Product", "CIDVALOR06", 11);
                            oProduct.cTextoExtra1 = GetStringField("Product", "cTextoExtra1", kLongTextoExtra);
                            oProduct.cTextoExtra2 = GetStringField("Product", "cTextoExtra2", kLongTextoExtra);
                            oProduct.cTextoExtra3 = GetStringField("Product", "cTextoExtra3", kLongTextoExtra);
                            oProduct.cFechaExtra = GetStringField("Product", "cFechaExtra", kLongFecha);
                            oProduct.cImporteExtra1 = GetDoubleField("Product", "cImporteExtra1");
                            oProduct.cImporteExtra2 = GetDoubleField("Product", "cImporteExtra2");
                            oProduct.cImporteExtra3 = GetDoubleField("Product", "cImporteExtra3");
                            oProduct.cImporteExtra4 = GetDoubleField("Product", "cImporteExtra4");
                            oProduct.cCodaltern = GetStringField("Product", "cCodaltern", kLongCodigo);
                            lstProducts.Add(oProduct);
                        }
                    } while (CompacSDK.fPosSiguienteProducto() == 0);
                }
                else
                {
                    iReturn = iResult;
                    Console.WriteLine("No first product");
                }
            }
            catch (Exception ex)
            {
                log.Error("ERROR in method GetProductChanges: " + ex.Message);
                throw ex;
            }
            return iReturn;
        }

        /// <summary>
        /// Method that retrieves the Address of a client from AdminPaq.
        /// </summary>
        /// <param name="address">Ref:  Address retrieved from AdminPaq.</param>
        /// <param name="code">Code of client.</param>
        /// <param name="type">Type of client.</param>
        /// <returns>0 if success.</returns>
        public int GetAddress(ref Address address, string code, int type)
        {
            Logger log = new Logger();
            //int iReturn = 0;
            int iResult = CompacSDK.fBuscaDireccionCteProv(code, type);
            if (iResult != 0)
            {
                log.Warn("Search error: " + GetError(iResult) + ". Code" + code);
            }
            else
            {
                address.cTipoCatalogo = GetLongField("Address", "cTipoCatalogo");
                address.cTipoDireccion = GetLongField("Address", "cTipoDireccion");
                address.cNombreCalle = GetStringField("Address", "cNombreCalle", kLongDescripcion);
                address.cNumeroExterior = GetStringField("Address", "cNumeroExterior", kLongDescripcion);
                address.cNumeroInterior = GetStringField("Address", "cNumeroInterior", kLongDescripcion);
                address.cColonia = GetStringField("Address", "cColonia", kLongDescripcion);
                address.cCodigoPostal = GetStringField("Address", "cCodigoPostal", kLongDescripcion);
                address.cTelefono1 = GetStringField("Address", "cTelefono1", kLongDescripcion);
                address.cTelefono2 = GetStringField("Address", "cTelefono2", kLongDescripcion);
                address.cTelefono3 = GetStringField("Address", "cTelefono3", kLongDescripcion);
                address.cTelefono4 = GetStringField("Address", "cTelefono4", kLongDescripcion);
                address.cEmail = GetStringField("Address", "cEmail", kLongDescripcion);
                address.cDireccionWeb = GetStringField("Address", "cDireccionWeb", kLongDescripcion);
                address.cCiudad = GetStringField("Address", "cCiudad", kLongDescripcion);
                address.cEstado = GetStringField("Address", "cEstado", kLongDescripcion);
                address.cPais = GetStringField("Address", "cPais", kLongDescripcion);
                address.cTextoExtra = GetStringField("Address", "cTextoExtra", kLongDescripcion);
                address.cCodCteProv = code;
            }

            return iResult;
        }

        /// <summary>
        /// Method that saves the new order to AdminPaq.
        /// </summary>
        /// <param name="cliente">Code of client.</param>
        /// <param name="importe">Total amount of order.</param>
        /// <param name="pCod">List of product codes.</param>
        /// <param name="pPrice">List of proruct prices.</param>
        /// <param name="pQty">List of product quantities.</param>
        /// <returns></returns>
        [HandleProcessCorruptedStateExceptions]
        public int SaveOrder(Order order)
        {
            int retValue = 0;
            Logger log = new Logger();
            try
            {
                log.Trace("Saving order of client " + order.ClientCode);
                double lFolioDocto = 0;
                string concepto = ConfigProxy.OrderConceptCode; //Order code for AdmiPAQ, default = 2
                StringBuilder lSerieDocto = new StringBuilder(12);
                retValue = CompacSDK.fSiguienteFolio(concepto, lSerieDocto, ref lFolioDocto);

                int lIdDocumento = 0;
                if (retValue != 0)
                {
                    log.Error("Unable to get folio number. " + GetError(retValue));
                }
                else
                    log.Debug("Folio number obtained: " + lFolioDocto);

                CompacSDK.tDocumento ltDocumento = new CompacSDK.tDocumento();
                ltDocumento.aCodConcepto = concepto;
                ltDocumento.aSerie = "";
                ltDocumento.aFolio = lFolioDocto;
                string lFechaDocto = DateTime.Today.ToString("MM/dd/yyyy");
                ltDocumento.aFecha = lFechaDocto;
                ltDocumento.aCodigoCteProv = order.ClientCode;
                ltDocumento.aCodigoAgente = "(Ninguno)";
                ltDocumento.aSistemaOrigen = 0; //205=COMERCIAL,0=AdminPAQ
                ltDocumento.aNumMoneda = 1;
                ltDocumento.aTipoCambio = 1;
                ltDocumento.aImporte = order.Amount;
                ltDocumento.aDescuentoDoc1 = 0;
                ltDocumento.aDescuentoDoc2 = 0;
                ltDocumento.aAfecta = 0;
                ltDocumento.aReferencia = "";

                log.Debug("Client: " + ltDocumento.aCodigoCteProv + ". Concept: " + ltDocumento.aCodConcepto + ". Date: " + ltDocumento.aFecha);

                retValue = CompacSDK.fAltaDocumento(ref lIdDocumento, ref ltDocumento);
                log.Debug("Error Num: " + retValue + ". Id Doc " + lIdDocumento);
                if (retValue == 0)
                {
                    log.Debug("Document created, id: " + lIdDocumento);
                    for (int i = 0; i < order.ProductCodes.Count; i++)
                    {
                        if (order.ProductCodes[i] == "null")
                            continue;

                        CompacSDK.tMovimiento ltMovimiento = new CompacSDK.tMovimiento();
                        int lIdMovimiento = 0;
                        string lCodigoAlmacen = ConfigProxy.WarehouseCode;

                        ltMovimiento.aCodAlmacen = lCodigoAlmacen;
                        ltMovimiento.aConsecutivo = i;
                        ltMovimiento.aCodProdSer = order.ProductCodes[i];
                        ltMovimiento.aUnidades = order.ProductQuantities[i];
                        ltMovimiento.aPrecio = order.ProductPrices[i];
                        ltMovimiento.aCosto = 0;

                        retValue = CompacSDK.fAltaMovimiento(lIdDocumento, ref lIdMovimiento, ref ltMovimiento);
                        if (retValue != 0)
                        {
                            log.Warn("Unable to create movement. Error: " + retValue + ". " + GetError(retValue) + ". Product: " + ltMovimiento.aCodProdSer);
                            return retValue;
                        }
                        log.Trace("Movement inserted: Document Id: " + lIdDocumento + " Movement Id: " + lIdMovimiento + " Product: " + ltMovimiento.aCodProdSer);


                    }
                    retValue = CompacSDK.fAfectaDocto_Param(concepto, lSerieDocto.ToString(), lFolioDocto, true);
                    if (retValue != 0)
                    {
                        log.Warn("Document not affected: Error: " + retValue + ", Message: " + GetError(retValue) + ". Concepto: " + concepto + ", Folio: " + lFolioDocto + " Series: " + lSerieDocto.ToString());
                    }
                    log.Info("Document succesfully created. Folio Number: " + lFolioDocto);
                }
                else
                {
                    log.Error("Error creating new document: " + retValue + ". Message:  " + GetError(retValue));
                }
            }
            catch(Exception ex)
            {
                log.Error("Error saving Order to AdminPaq.  Message: " + ex.Message);
                retValue = -1;
                throw ex;
            }
            return retValue;
        }

        /// <summary>
        /// Method that retrieves the agent of a given client.
        /// </summary>
        /// <param name="clientCode">Code of client</param>
        /// <param name="agent">Ref: Agent retrieved.</param>
        /// <returns>0 if success.</returns>
        public int GetAgent(string clientCode, ref string agent)
        {
            Logger log = new Logger();
            int lError = 0;
            try
            {
                StringBuilder dato = new StringBuilder();
                //lCteProv
                lError = CompacSDK.fBuscaCteProv(clientCode);
                if (lError != 0)
                {
                    log.Warn("Error buscando el cliente: " + clientCode + "." + CompacSDK.GetError(lError));
                    return lError;
                }
                else
                {
                    //Leer dato del cliente rfc
                    lError = CompacSDK.fLeeDatoCteProv("CIDAGENT01", dato, 11);
                    if (lError != 0)
                    {
                        //CompacSDK.GetError(lError);
                        log.Warn("Error buscando agente por id para el cliente " + clientCode + ". Error " + CompacSDK.GetError(lError));
                        return lError;
                    }
                }
                log.Debug("id agente de venta " + dato.ToString());

                if (dato.Length == 0)
                {
                    // No hay agente de venta asignado
                    return -1;
                }

                //posteriormente con los id del agente podrás buscar sus datos de la siguiente forma:

                StringBuilder a = new StringBuilder();
                a.Capacity = 120;

                int agID = Convert.ToInt32(dato.ToString());
                lError = CompacSDK.fBuscaIdAgente(agID);

                if (lError != 0)
                {
                    log.Warn(CompacSDK.GetError(lError));
                    return lError;
                }
                else
                {
                    lError = CompacSDK.fLeeDatoAgente("CNOMBREA01", a, 60);
                    agent = a.ToString();
                    return lError;
                }
            }
            catch (Exception ex)
            {
                log.Warn("Algo fallo al buscar el agente para el cliente " + clientCode + ". Error: " + ex);
                agent = "";
                return -1;
            }
        }

        #endregion

    }
}
