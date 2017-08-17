using HandySyncService.Model;
using HandySyncService.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService
{
    public static class ExtensionMethods
    {
        public static HandyProduct ToHandyProduct(this Product value)
        {
            HandyProduct producto = new HandyProduct();
            producto.code = value.cCodigoProducto;
            producto.barcode = value.cCodaltern;
            producto.description = (value.cNombreProducto == " " || value.cNombreProducto == "") ? "--" : value.cNombreProducto; // se usa el nombre de producto de AdminPAQ para la descripción en Handy, al ser un campo obligatorio en Handy se pone un valor default
            producto.details = (value.cDesccorta == " " || value.cDesccorta == "") ? "--" : value.cDesccorta; // Se usa descripción corta porque no se pudo acceder a la descripción detallada de AdminPAQ
            producto.enabled = (value.cStatusProducto == 1) ? true : false;

            Console.WriteLine(ConfigProxy.PriceId);
            Console.WriteLine(ConfigProxy.PriceId == "2");
            switch (ConfigProxy.PriceId)
            {
                case "1": producto.price = value.cPrecio1; break;
                case "2": producto.price = value.cPrecio2; break;
                case "3": producto.price = value.cPrecio3; break;
                case "4": producto.price = value.cPrecio4; break;
                case "5": producto.price = value.cPrecio5; break;
                case "6": producto.price = value.cPrecio6; break;
                case "7": producto.price = value.cPrecio7; break;
                case "8": producto.price = value.cPrecio8; break;
                case "9": producto.price = value.cPrecio9; break;
                case "10": producto.price = value.cPrecio10; break;
                default: producto.price = value.cPrecio2; break;
            }
 
            producto.product_family = "Generales";
            return producto;
        }

        [HandleProcessCorruptedStateExceptions]
        public static Client ToHandyClient(this CustomerSupplier value)
        {
            Client client = new Client();
            client.code = value.cCodigoCliente;
            client.accuracy = 10;
            client.description = value.cRazonSocial;
            client.comments = "RFC: " + value.cRFC;
            client.enabled = (value.cEstatus == 1) ? true : false;
            client.isNew = false;
            client.discount = value.cDescuentoMovto;
            //Las direcciones en Compaq se guardan por separado, es necesario volver a conectarse para buscar el domicilio
            Logger log = new Logger();	//Inicializa logs
            try
            {
                Directory.SetCurrentDirectory(ConfigProxy.AdminPaqPath); //Cambia al directorio de AdminPAQ para acceder al SDK
                int iResult = CompacSDK.fInicializaSDK();// Inicialización del SDK

                Address address = new Address();	//nuevo objeto para almacenar la dirección del cliente
                if (iResult == 0)
                {
                    iResult = CompacSDK.fAbreEmpresa(ConfigProxy.AdminPaqCompanyPath); //Apertura de la empresa
                    if (iResult == 0)
                    {
                        string agent = "";
                        iResult = AdminPaqService.Instance.GetAgent(value.cCodigoCliente, ref agent);

                        if (iResult == 0)
                        {
                            client.zone_description = agent;
                            log.Debug(value.cRazonSocial + " Agente encontrado: " + agent);
                        }
                        else
                        {
                            client.zone_description = "Zona General";
                            log.Warn(value.cRazonSocial + "No se pudo obtener agente. Error: " + iResult + ", " + CompacSDK.GetError(iResult));
                        }

                        iResult = AdminPaqService.Instance.GetAddress(ref address, value.cCodigoCliente, 0); //búsqueda de la dirección fiscal del cliente
                        if (iResult == 0)
                        {
                            client.address = address.cNombreCalle + " " + address.cNumeroExterior + " " + address.cNumeroInterior + " " + address.cColonia + ", " + address.cEstado + ", " + address.cPais;
                            client.postalCode = address.cCodigoPostal;
                            client.city = address.cCiudad;
                            client.phoneNumber = address.cTelefono1;
                        }
                        else
                        {
                            log.Warn("Dirección no encontrada para cliente " + value.cCodigoCliente);
                        }
                    }
                    else
                    {
                        log.Warn("No se pudo abrir la empresa para buscar dirección y agente del cliente. Error: " + AdminPaqService.Instance.GetError(iResult));
                    }
                }
                else
                {
                    log.Error("No se pudo inicializar el SDK para obtener direcciones de clientes");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error al tratar de obtener la dirección de un cliente: " + client.code + ". Exepción: " + ex);
                throw ex;
            }
            return client;
        }
    }
}
