using HandySyncService.Model;
using HandySyncService.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService.Services
{
    class HandyService : IHandyService
    {
        private static readonly Lazy<HandyService> lazy =
            new Lazy<HandyService>(() => new HandyService());

        public static HandyService Instance { get { return lazy.Value; } }

        private HandyService()
        {
        }
       
        //Data to access Handy API.  Needed in config file.
        private string username = ConfigProxy.HandyUser;
        private string password = ConfigProxy.HandyPassword;
        private string baseURL = ConfigProxy.HandyURL;

        private Logger log = new Logger();

        //NOT USED
        public List<Client> getClient(string since)
        {
            log.Info("Obteniendo listado de clientes");
            List<Client> clientes = null;
            JObject response;
            //Si la fecha no se especifica o el formato de la fecha es incorrecto entonces el parámetro será ignorado y se regresará un listado de todos los clientes
            if (since != null)
            {
                try
                {
                    //Se intenta convertir la fecha a milisegundos desde 1 Enero de 1970.
                    //La fecha se espera en formato dd/MM/yyyy HH:mm:ss.ff, 
                    //por ello se especifica que la cultura es es-MX, para evitar que si la computadora tiene 
                    //una configuración regional distinta la fecha se interprete de manera distinta y pueda llevar a errores.
                    DateTime date = DateTime.Parse(since, System.Globalization.CultureInfo.CreateSpecificCulture("es-MX"));
                    since = "&since=" + date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                }
                catch (Exception ex)
                {
                    //Si un error sucede se ignorará el parámetro.
                    since = "";
                }
            }
            try
            {
                string url = baseURL + "api/customer/list?enabled=true" + since; //Se crea la URL de acceso al API.
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password)); //Se agregan los datos de login a la cabezera HTTP.
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();//Se realiza la petición.
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();//Lectura de la respuesta
                    clientes = new List<Client>();//Listado donde se almacenarán los clientes.
                    response = JObject.Parse(result);//La respuesta viene en formato JSON, se pasa a un objeto de este tipo para poder acceder a los datos de manera más fácil.
                    for (int i = 0; i < response["customers"].Count(); i++)
                    {//Se recorre el objeto para buscar y extraer la información.
                        Client c = new Client();//Objeto temporal para alamacenar los datos del registro del ciclo actual.
                        c.accuracy = (double)response["customers"][i]["accuracy"];
                        c.address = (string)response["customers"][i]["address"];
                        c.city = (string)response["customers"][i]["city"];
                        c.code = (string)response["customers"][i]["code"];
                        c.comments = (string)response["customers"][i]["comments"];
                        c.description = (string)response["customers"][i]["description"];
                        c.discount = (double)response["customers"][i]["discount"];
                        c.email = (string)response["customers"][i]["email"];
                        c.enabled = (bool)response["customers"][i]["enabled"];
                        c.is_mobile = (bool)response["customers"][i]["mobile"];
                        c.is_prospect = (bool)response["customers"][i]["is_prospect"];
                        c.latitude = (double)response["customers"][i]["latitude"];
                        c.longitude = (double)response["customers"][i]["longitude"];
                        c.owner = (string)response["customers"][i]["owner"];
                        c.phoneNumber = (string)response["customers"][i]["phone_number"];
                        c.postalCode = (string)response["customers"][i]["postal_code"];
                        c.zone_description = (string)response["customers"][i]["zone_description"];
                        //Se determina si el registro es nuevo o es una modificación, esto se logra comparando fechas de creación y actualización,
                        //si las fechas son iguales el cliente es nuevo, si no entonces fue editado.
                        if (((string)response["customers"][i]["date_created"]) == ((string)response["customers"][i]["last_updated"]))
                        {
                            c.isNew = true;
                        }
                        else
                        {
                            c.isNew = false;
                        }
                        clientes.Add(c);//Se agrega el objeto temporal al arreglo mayor antes de que sea eliminado.
                    }
                }
            }
            catch (Exception ex)
            {
                //Algo falló al procesar el listado.
                log.Error("Error obteniendo listado de clientes: Exception: " + ex);
                response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
            }
            //Algo falló del otro lado del API.
            if (((bool)response["error"]))
                log.Error("Error obteniendo listado de clientes: " + (string)response["message"]);

            return clientes;
        }
        
        //NOT USED
        public string saveClient(Client cliente)
        {
            log.Info("Guardando un cliente");
            JObject response = null;
            string url = baseURL + "api/customer/createOrUpdate";
            string request = "";
            string result = "--";
            try
            {
                //Convierte los datos del objeto Client en un arreglo tipo Json.
                JObject json = new JObject(new JProperty("customers", new JArray(JObject.Parse(JsonConvert.SerializeObject(cliente)))));
                request = json.ToString();
                //Agrega credenciales de autenticación y las cabezeras a la petición.
                CredentialCache credentialCache = new CredentialCache();
                credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential(username, password));
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Credentials = credentialCache;
                httpWebRequest.Timeout = 30000;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json.ToString());
                    streamWriter.Flush();
                    streamWriter.Close();
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Lectura y conversión Json de la respuesta.
                        result = streamReader.ReadToEnd();
                        response = JObject.Parse(result);
                    }
                }
            }
            catch (Exception ex)
            {
                //Algo falló al insertar el cliente.
                log.Error("Error guardando un cliente: Exception: " + ex + ". **** Cadena procesada: " + request);
                response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
            }
            //Algo falló del otro lado del API.
            if (((bool)response["error"]))
                log.Error("Error guardando un cliente: " + (string)response["message"] + ". **** Cadena enviada: " + request);

            return result;
        }
        
        /// <summary>
        /// Method to save a group of clients in Handy.
        /// </summary>
        /// <param name="clientes">List of clients to save in Handy.</param>
        /// <returns>0 if Success</returns>
        public string SaveClients(List<Client> clientes)
        {
            log.Info("Saving one or more clients.");
            JObject response = null;
            string url = baseURL + "api/customer/createOrUpdate";
            string request = "";
            string result = "--";
            //Si el listado contiene más de 100 clientes se procesará por partes de hasta 100 registros por parte.
            if (clientes.Count > 100)
            {
                log.Info("Many clients to save (" + clientes.Count + ").  They will be splitted in groups of 100.");
                int y = 0;
                for (int i = 0; i < clientes.Count(); i += 100)
                {
                    List<Client> lapso = new List<Client>();
                    for (int j = 0; j < 100; j++, y++)
                    {
                        if (y == clientes.Count)
                            break;
                        lapso.Add(clientes[y]);
                    }
                    try
                    {
                        //Se convierten los datos a un arreglo Json.
                        JObject json = new JObject(new JProperty("customers", new JArray(from c in lapso select new JObject(JObject.Parse(JsonConvert.SerializeObject(c))))));
                        request = json.ToString();
                        //Agrega credenciales de autenticación y las cabezeras a la petición.
                        CredentialCache credentialCache = new CredentialCache();
                        credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential(username, password));
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        httpWebRequest.ContentType = "text/json";
                        httpWebRequest.Method = "POST";
                        httpWebRequest.Credentials = credentialCache;
                        httpWebRequest.Timeout = 30000;
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(json.ToString());
                            streamWriter.Flush();
                            streamWriter.Close();
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                                log.Trace("Clients result: " + result);
                                response = JObject.Parse(result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error inserting group of clients: " + ex + ". **** String sent: " + request);
                        response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(response["error"])))
                        if (((bool)response["error"]))
                            log.Error("Error inserting group of clients:  " + (string)response["message"] + ". **** String sent: " + request);
                }
            }
            else
            { //El listado contiene menos de 100 elementos
                if (clientes.Count > 0)
                {
                    log.Info("There are " + clientes.Count + " clients.");
                    try
                    {
                        //Se convierten los datos a un arreglo Json.
                        JObject json = new JObject(new JProperty("customers", new JArray(from c in clientes select new JObject(JObject.Parse(JsonConvert.SerializeObject(c))))));
                        request = json.ToString();
                        //Agrega credenciales de autenticación y las cabezeras a la petición.
                        CredentialCache credentialCache = new CredentialCache();
                        credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential(username, password));
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        httpWebRequest.ContentType = "text/json";
                        httpWebRequest.Method = "POST";
                        httpWebRequest.Credentials = credentialCache;
                        httpWebRequest.Timeout = 30000;
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(json.ToString());
                            streamWriter.Flush();
                            streamWriter.Close();
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                                log.Trace("Clients result: " + result);
                                response = JObject.Parse(result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error inserting group of clients: : " + ex + ". **** String sent: " + request);
                        response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(response["error"])))
                        if (((bool)response["error"]))
                            log.Error("Error inserting group of clients: : " + (string)response["message"] + ". **** String sent: " + request);
                }
                else
                {
                    log.Info("No clients to sync.");
                }
            }
            return result;
        }

        //NOT USED
        public List<HandyProduct> getProducts(string since)
        {
            log.Info("Obteniendo un listado de productos desde " + since);
            JObject response = null;
            List<HandyProduct> products = null;
            //Si la fecha no se especifica o el formato de la fecha es incorrecto entonces el parámetro será ignorado y se regresará un listado de todos los productos
            if (since != null)
            {
                try
                {
                    //Se intenta convertir la fecha a milisegundos desde 1 Enero de 1970.
                    //La fecha se espera en formato dd/MM/yyyy HH:mm:ss.ff, 
                    //por ello se especifica que la cultura es es-MX, para evitar que si la computadora tiene 
                    //una configuración regional distinta la fecha se interprete de manera distinta y pueda llevar a errores.
                    DateTime date = DateTime.Parse(since, System.Globalization.CultureInfo.CreateSpecificCulture("es-MX"));
                    since = "&since=" + date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                }
                catch (Exception ex)
                {
                    //Si un error sucede se ignorará el parámetro.
                    since = "";
                }
            }
            try
            {
                string url = baseURL + "api/product/list?enabled=true" + since;
                //this work for read only, not for write
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password)); //Se agregan los datos de login a la cabezera HTTP.
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();//Se realiza la petición.
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    products = new List<HandyProduct>();
                    response = JObject.Parse(result);
                    for (int i = 0; i < response["products"].Count(); i++)
                    {
                        HandyProduct p = new HandyProduct();
                        p.code = (string)response["products"][i]["code"];
                        p.description = (string)response["products"][i]["description"];
                        p.product_family = (string)response["products"][i]["family"]["description"];
                        p.enabled = (bool)response["products"][i]["enabled"];
                        p.price = (double)response["products"][i]["price"];
                        p.apply_discounts = (bool)response["products"][i]["apply_discounts"];
                        p.details = (string)response["products"][i]["details"];
                        //Se determina si el registro es nuevo o es una modificación, esto se logra comparando fechas de creación y actualización,
                        //si las fechas son iguales el cliente es nuevo, si no entonces fue editado.
                        if (((string)response["products"][i]["date_created"]) == ((string)response["products"][i]["last_updated"]))
                        {
                            p.isNew = true;
                        }
                        else
                        {
                            p.isNew = false;
                        }
                        products.Add(p);//Se agrega el objeto temporal al arreglo mayor antes de que sea eliminado.
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error obteniendo el listado de productos: Exception: " + ex);
                response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
            }
            if (((bool)response["error"]))
                log.Error("Error obteniendo el listado de productos: " + (string)response["message"]);

            return products;
        }

        //NOT USED
        public string saveProduct(Product product)
        {
            log.Info("Guardando un producto");
            JObject response = null;
            string url = baseURL + "api/product/createOrUpdate";
            string request = "";
            string result = "--";
            JObject json = new JObject();
            try
            {
                //Convierte los datos del objeto Product en un arreglo tipo Json.
                json = JObject.Parse(JsonConvert.SerializeObject(product));
                request = json.ToString();
                //Agrega credenciales de autenticación y las cabezeras a la petición.
                CredentialCache credentialCache = new CredentialCache();
                credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential(username, password));
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Credentials = credentialCache;
                httpWebRequest.Timeout = 30000;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json.ToString());
                    streamWriter.Flush();
                    streamWriter.Close();
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Lectura y conversión Json de la respuesta.
                        result = streamReader.ReadToEnd();
                        log.Trace("Products result: " + result);
                        response = JObject.Parse(result);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error guardando un producto: Exception: " + ex + ". **** Cadena enviada: " + request);
                response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
            }
            if (((bool)response["error"]))
                log.Error("Error guardando un producto: " + (string)response["message"] + ". **** Cadena enviada: " + request);
            return result;
        }
        
        /// <summary>
        /// Method to save a group of products in Handy.
        /// </summary>
        /// <param name="products">List of products to be saved in Handy.</param>
        /// <returns>0 if success</returns>
        public string SaveProducts(List<HandyProduct> products)
        {
            log.Info("Saving list of products");
            JObject response = null;
            string url = baseURL + "api/product/createOrUpdate";
            string request = "";
            string result = "--";
            //Si el listado contiene más de 100 productos se procesará por partes de hasta 100 registros por parte.
            if (products.Count > 100)
            {
                log.Info("Many products to save (" + products.Count + "). They will be splitted in groups of 100.");
                int y = 0;
                for (int i = 0; i < products.Count(); i += 100)
                {

                    List<HandyProduct> lapso = new List<HandyProduct>();
                    for (int j = 0; j < 100; j++, y++)
                    {
                        if (y == products.Count)
                            break;
                        lapso.Add(products[y]);
                    }
                    //Se convierten los datos a un arreglo Json.
                    JObject json = new JObject(new JProperty("products", new JArray(from p in lapso select new JObject(JObject.Parse(JsonConvert.SerializeObject(p))))));
                    //log.debug(json.ToString().Replace("\"","\\\""));

                    string text = "Data sent to handy: " + json.ToString();
                    // WriteAllText creates a file, writes the specified string to the file, 
                    // and then closes the file.
                    System.IO.File.WriteAllText(@"C:\Users\Public\logs\handyLog-" + i + ".txt", text);

                    request = json.ToString();
                    try
                    {
                        //Agrega credenciales de autenticación y las cabezeras a la petición.
                        CredentialCache credentialCache = new CredentialCache();
                        credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential(username, password));
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        httpWebRequest.ContentType = "text/json";
                        httpWebRequest.Method = "POST";
                        httpWebRequest.Credentials = credentialCache;
                        httpWebRequest.Timeout = 30000;
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(json.ToString());
                            streamWriter.Flush();
                            streamWriter.Close();
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                                log.Trace("Products result: " + result);
                                response = JObject.Parse(result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error saving group of products: : Exception: " + ex + ". **** String sent: " + request);
                        response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
                    }

                    if (((bool)response["error"]))
                        log.Error("Error saving group of products: " + (string)response["message"] + ". **** String sent: " + request);
                }
            }
            else
            {//El listado contiene menos de 100 elementos
                if (products.Count > 0)
                {
                    //Se convierten los datos a un arreglo Json.
                    JObject json = new JObject(new JProperty("products", new JArray(from p in products select new JObject(JObject.Parse(JsonConvert.SerializeObject(p))))));
                    request = json.ToString();
                    log.Debug("Info to send in a json object format: " + json.ToString());
                    try
                    {
                        //Agrega credenciales de autenticación y las cabezeras a la petición.
                        CredentialCache credentialCache = new CredentialCache();
                        credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential(username, password));
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        httpWebRequest.ContentType = "text/json";
                        httpWebRequest.Method = "POST";
                        httpWebRequest.Credentials = credentialCache;
                        httpWebRequest.Timeout = 30000;
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(json.ToString());
                            streamWriter.Flush();
                            streamWriter.Close();
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                                log.Trace("Product result: " + result);
                                response = JObject.Parse(result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error saving group of products: Exception: " + ex + ". **** String sent: " + request);
                        response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
                    }
                    if (((bool)response["error"]))
                        log.Error("Error saving group of products: " + (string)response["message"] + ". **** String sent: " + request);
                }
                else
                {
                    log.Info("No products to sync.");
                }
            }
            return result;
        }
        
        /// <summary>
        /// Method to retrieve a list of orders from a specific date.
        /// </summary>
        /// <param name="since">Date from which the orders will be retrieved.
        /// Format:  dd/MM/yyyy hh:mm:ss
        /// </param>
        /// <returns>0 if success</returns>
        public string GetSalesOrders(string since)
        {
            log.Debug("Getting list of orders since " + since);
            JObject response = null;
            //Si la fecha no se especifica o el formato de la fecha es incorrecto entonces el parámetro será ignorado y se regresará un listado de todos los pedidos
            if (since != null)
            {
                try
                {
                    //Se intenta convertir la fecha a milisegundos desde 1 Enero de 1970.
                    //La fecha se espera en formato dd/MM/yyyy HH:mm:ss.ff, 
                    //por ello se especifica que la cultura es es-MX, para evitar que si la computadora tiene 
                    //una configuración regional distinta la fecha se interprete de manera distinta y pueda llevar a errores.
                    DateTime date = DateTime.Parse(since, System.Globalization.CultureInfo.CreateSpecificCulture("es-MX"));
                    since = "?since=" + date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                }
                catch (Exception ex)
                {
                    //Si un error sucede se ignorará el parámetro.
                    since = "";
                }
            }
            string url = baseURL + "api/salesOrder/list" + since;
            try
            {
                //this work for read only, not for write

                log.Trace("Starting API communication");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password)); //Se agregan los datos de login a la cabezera HTTP.

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();//Se realiza la petición.

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    response = JObject.Parse(result);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error getting list of orders. Exception: " + ex);
                response = JObject.Parse("{error:true,message:\"Exception: see logs\"}");
            }
            if (((bool)response["error"]))
                log.Error("Error getting list of orders: " + (string)response["message"]);

            return response.ToString();
        }
    }
}
