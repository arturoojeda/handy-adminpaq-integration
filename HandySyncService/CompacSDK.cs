using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService
{
    class CompacSDK
    {
        #region constants

        public const int kLongFecha = 24;
        public const int kLongSerie = 12;
        public const int kLongCodigo = 31;
        public const int kLongNombre = 61;
        public const int kLongReferencia = 21;
        public const int kLongDescripcion = 61;
        public const int kLongCuenta = 101;
        public const int kLongMensaje = 3001;
        public const int kLongNombreProducto = 256;
        public const int kLongAbreviatura = 4;
        public const int kLongCodValorClasif = 4;
        public const int kLongDenComercial = 51;
        public const int kLongRepLegal = 51;
        public const int kLongTextoExtra = 51;
        public const int kLongRFC = 21;
        public const int kLongCURP = 21;
        public const int kLongDesCorta = 21;
        public const int kLongNumeroExtInt = 7;
        public const int kLongNumeroExpandido = 31;
        public const int kLongCodigoPostal = 7;
        public const int kLongTelefono = 16;
        public const int kLongEmailWeb = 51;

        public const int kLongSelloSat = 176;
        public const int kLonSerieCertSAT = 21;
        public const int kLongFechaHora = 36;
        public const int kLongSelloCFDI = 176;
        public const int kLongCadOrigComplSAT = 501;
        public const int kLongitudUUID = 37;
        public const int kLongitudRegimen = 101;
        public const int kLongitudMoneda = 61;
        public const int kLongitudFolio = 17;
        public const int kLongitudMonto = 31;
        public const int kLogitudLugarExpedicion = 401;

        #endregion

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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongSerie)]
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tMovimiento
        {
            public int aConsecutivo;
            public Double aUnidades;
            public Double aPrecio;
            public Double aCosto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public String aCodProdSer;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public String aCodAlmacen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongReferencia)]
            public String aReferencia;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public String aCodClasificacion;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tProduto
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public string cCodigoProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongNombre)]
            public string cNombreProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongNombreProducto)]
            public string cDescripcionProducto;
            public int cTipoProducto; // 1 = Producto, 2 = Paquete, 3 = Servicio
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongFecha)]
            public string cFechaAltaProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongFecha)]
            public string cFechaBaja;
            public int cStatusProducto; // 0 - Baja Lógica, 1 - Alta
            public int cControlExistencia;
            public int cMetodoCosteo; // 1 = Costo Promedio en Base a Entradas, 2 = Costo Promedio en Base a Entradas Almacen, 3 = Último costo, 4 = UEPS, 5 = PEPS, 6 = Costo específico, 7 = Costo Estandar
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public string cCodigoUnidadBase;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodigo)]
            public string cCodigoUnidadNoConvertible;
            public double cPrecio1;
            public double cPrecio2;
            public double cPrecio3;
            public double cPrecio4;
            public double cPrecio5;
            public double cPrecio6;
            public double cPrecio7;
            public double cPrecio8;
            public double cPrecio9;
            public double cPrecio10;
            public double cImpuesto1;
            public double cImpuesto2;
            public double cImpuesto3;
            public double cRetencion1;
            public double cRetencion2;
            // N.D.8386 La estructura debe recibir el nombre de la característica padre. (ALRH)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongNombre)]
            public string cNombreCaracteristica1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongNombre)]
            public string cNombreCaracteristica2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongNombre)]
            public string cNombreCaracteristica3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodValorClasif)]
            public string cCodigoValorClasificacion1;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodValorClasif)]
            public string cCodigoValorClasificacion2;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodValorClasif)]
            public string cCodigoValorClasificacion3;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodValorClasif)]
            public string cCodigoValorClasificacion4;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodValorClasif)]
            public string cCodigoValorClasificacion5;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongCodValorClasif)]
            public string cCodigoValorClasificacion6;//[ kLongCodValorClasif + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongTextoExtra)]
            public string cTextoExtra1;//[ kLongTextoExtra + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongTextoExtra)]
            public string cTextoExtra2;//[ kLongTextoExtra + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongTextoExtra)]
            public string cTextoExtra3;//[ kLongTextoExtra + 1 ];
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = kLongFecha)]
            public string cFechaExtra;//[ kLongFecha + 1 ];
            public double cImporteExtra1;
            public double cImporteExtra2;
            public double cImporteExtra3;
            public double cImporteExtra4;
        }

        [DllImport("MGW_SDK.DLL")]
        public static extern int fInicializaSDK();
        [DllImport("MGW_SDK.DLL")]
        public static extern void fTerminaSDK();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fSetNombrePAQ(String aNombrePAQ);
        [DllImport("MGW_SDK.DLL")]
        public static extern int fAbreEmpresa(string sEmpresa);
        [DllImport("MGW_SDK.DLL")]
        public static extern void fCierraEmpresa();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosPrimerCteProv();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosSiguienteCteProv();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosUltimoCteProv();
        [DllImport("MGW_SDK.DLL", CharSet = CharSet.None)]
        public static extern int fLeeDatoCteProv(string aCampo, StringBuilder aValor, int aLen);
        [DllImport("MGW_SDK.DLL", CharSet = CharSet.None)]
        public static extern int fSetDatoCteProv(string aCampo, string aValor);
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosBOFCteProv();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosEOFCteProv();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fInsertaCteProv();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fEditaCteProv();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fGuardaCteProv();
        [DllImport("MGW_SDK.DLL", CharSet = CharSet.None)]
        public static extern void fError(int aNumError, StringBuilder aMensaje, int aLen);
        [DllImport("MGW_SDK.DLL")]
        public static extern int fBuscaCteProv(string lCodCteProv);

        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosPrimerProducto();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosSiguienteProducto();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosUltimoProducto();
        [DllImport("MGW_SDK.DLL", CharSet = CharSet.None)]
        public static extern int fLeeDatoProducto(string aCampo, StringBuilder aValor, int aLen);
        [DllImport("MGW_SDK.DLL", CharSet = CharSet.None)]
        public static extern int fSetDatoProducto(string aCampo, string aValor);
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosBOFProducto();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosEOFProducto();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fInsertaProducto();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fEditaProducto();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fGuardaProducto();

        [DllImport("MGW_SDK.DLL")]
        public static extern int fBuscaIdAgente(int lCodigoAgente);
        [DllImport("MGW_SDK.DLL", CharSet = CharSet.None)]
        public static extern int fLeeDatoAgente(string aCampo, StringBuilder aValor, int aLen);

        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosPrimerDireccion();
        [DllImport("MGW_SDK.DLL", CharSet = CharSet.None)]
        public static extern int fBuscaDireccionCteProv(string aCodCteProv, int aTipoDireccion);
        [DllImport("MGW_SDK.DLL", CharSet = CharSet.None)]
        public static extern int fLeeDatoDireccion(string aCampo, StringBuilder aValor, int aLen);
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosBOFDireccion();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosEOFDireccion();
        [DllImport("MGW_SDK.DLL")]
        public static extern int fPosSiguienteDireccion();

        [DllImport("MGW_SDK.dll")]
        public static extern Int32 fAltaDocumento(ref Int32 aIdDocumento, ref tDocumento atDocumento);

        [DllImport("MGW_SDK.dll")]
        public static extern Int32 fAltaMovimiento(Int32 aIdDocumento, ref Int32 aIdMovimiento, ref tMovimiento atMovimiento);

        [DllImport("MGW_SDK.dll")]
        public static extern Int32 fAfectaDocto(ref tDocumento atDocumento, bool aAfectarDocumento);

        [DllImport("MGW_SDK.dll")]
        public static extern Int32 fAfectaDocto_Param([MarshalAs(UnmanagedType.LPStr)] string aCodConcepto, [MarshalAs(UnmanagedType.LPStr)] string aSerie, double aFolio, bool aAfecta);

        [DllImport("MGW_SDK.dll")]
        //public static extern Int32 fSiguienteFolio(string aCodigoConcepto, StringBuilder aSerie, [In,Out]ref double aFolio);
        public static extern Int32 fSiguienteFolio([MarshalAs(UnmanagedType.LPStr)] string aCodigoConcepto, [MarshalAs(UnmanagedType.LPStr)] StringBuilder aSerie, ref double aFolio);

        public static string GetError(int iError)
        {
            StringBuilder sMensaje = new StringBuilder(512);

            if (iError != 0)
            {
                CompacSDK.fError(iError, sMensaje, 512);
            }
            return sMensaje.ToString();
        }

        [HandleProcessCorruptedStateExceptions]
        public static bool RunCompacMethod<T>(Func<T> method)
        {
            Logger log = new Logger();
            string methodName = method.GetMethodInfo().Name;
            try
            {
                method();
                return true;
            }
            catch(Exception ex)
            {
                log.Error(String.Format("Error running Compac method: {0}  Message: {1}", methodName, ex.Message));
                return false;
            }
        }
    }
}
