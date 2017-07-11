using HandySyncService.Model;
using HandySyncService.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace HandySyncService
{
    public class BaseProcess
    {
        static Logger log = new Logger();
        static Process process = null;

        private static void SyncProducts(string initialDate)
        {
            int pResult = 0;
            log.Trace("Products");
            List<Product> lstProducts = new List<Product>();
            try
            {
                pResult = AdminPaqService.Instance.GetProductChanges(ref lstProducts, initialDate);
                log.Trace("Searching... Since: " + initialDate);
                if (pResult != 0)
                    log.Info("No products found.");
                else
                {
                    log.Debug("Products to sync: " + lstProducts.Count);
                    List<HandyProduct> products = new List<HandyProduct>();
                    foreach (Product oProduct in lstProducts)
                    {
                        try
                        {
                            HandyProduct hProduct = oProduct.ToHandyProduct();
                            products.Add(hProduct);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error processing product: " + oProduct.cCodigoProducto + "; Error: " + ex.Message);
                        }
                    }
                    log.Info("Products processed. Response: " + HandyService.Instance.SaveProducts(products));
                }
            }
            catch(Exception ex)
            {
                log.Error("Error processing products: " + ex.Message);
                throw ex;
            }
        }
        
        private static void SyncClients(string initialDate)
        {
            log.Trace("Clients");
            int cResult = 0;
            List<CustomerSupplier> lstClients = new List<CustomerSupplier>();
            try
            {
                cResult = AdminPaqService.Instance.GetClientChanges(ref lstClients, initialDate);
                if (cResult == 0)
                {
                    log.Debug("Clients to sync: " + lstClients.Count);
                    int counter = 0;
                    List<Client> clients = new List<Client>();
                    foreach (CustomerSupplier custSupplier in lstClients)
                    {
                        counter++;
					    try
                        {
                            Client hCliente = custSupplier.ToHandyClient();
					        log.Trace("Client: " + hCliente.code + " : " + hCliente.address + ". Agent: " + hCliente.zone_description + " index : " + counter);
						    clients.Add(hCliente);
				        }
					    catch (Exception exC)
					    {
					        log.Error("Error processing client: " + custSupplier.cCodigoCliente + "; Error: " + exC.Message);
                        }
                    }
                    log.Info("Clients processed. Response: " + HandyService.Instance.SaveClients(clients));
                }
                else
                {
                    log.Info("No clients found.");
                }
            }
            catch(Exception ex)
            {
                log.Error("Error getting client changes; Error: " + ex.Message);
                throw ex;
            }
        }

        public static void SyncOrders(string initialDate)
        {
            log.Trace("Orders");
            
            try
            {
                string salesOrders = HandyService.Instance.GetSalesOrders(initialDate);
                    log.Debug("Orders obtained since " + initialDate);
                    log.Trace(salesOrders);
                    JObject response = JObject.Parse(salesOrders);
                    log.Debug("Orders count: " + response["salesOrders"].Count());
                    for (int i = 0; i < response["salesOrders"].Count(); i++)
                    {
                        try
                        {
                            string customerCode = response["salesOrders"][i]["customer_code"].ToString();
                            double amount = 0;
                            List<double> price = new List<double>();
                            List<int> qty = new List<int>();
                            List<string> pCode = new List<string>();
                            log.Debug("Order to " + response["salesOrders"][i]["customer_code"] + ", products: " + response["salesOrders"][i]["items"].Count());
                            for (int j = 0; j < response["salesOrders"][i]["items"].Count(); j++)
                            {
                                amount += (Convert.ToDouble(response["salesOrders"][i]["items"][j]["price"].ToString()) * Convert.ToInt32(response["salesOrders"][i]["items"][j]["quantity"].ToString()));
                                price.Add(Convert.ToDouble(response["salesOrders"][i]["items"][j]["price"].ToString()));
                                qty.Add(Convert.ToInt32(response["salesOrders"][i]["items"][j]["quantity"].ToString()));
                                pCode.Add(response["salesOrders"][i]["items"][j]["product_code"].ToString());
                            }
                            int oResult = 0;
                            log.Debug("Order will be saved to customer code: " + customerCode + ", amount " + amount);
                            Order order = new Order()
                            {
                                ClientCode = customerCode,
                                Amount = amount,
                                ProductCodes = pCode,
                                ProductPrices = price,
                                ProductQuantities = qty
                            };

                            oResult = AdminPaqService.Instance.SaveOrder(order);
                            if (oResult == 0)
                                log.Info("Order saved correctly.");
                            else
                                log.Error("Unable to save order. Error: " + CompacSDK.GetError(oResult));
                        }
                        catch(Exception ex)
                        {
                            log.Error("Error processing Order " + (i+1).ToString() + " Error: " + ex.Message);
                        }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error processing Orders. Error: " + ex.Message);
                throw ex;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public static void MainProcess()
        {
            process = Process.GetCurrentProcess();
            
            try
            {
                ConfigProxy.OpenConfigFile();
            }
            catch(Exception ex)
            {
                log.Error("Unable to open config file: " + ex.Message);
                Console.ReadKey();
                return;
            }
            int waitTime = ConfigProxy.SyncPeriodMinutes * 60 * 1000;
            log.Debug("Starting service.");
            log.Trace("Initializing...");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            while (true)
            {
                //Adding a higher level Exception handling in case the finally block throws an exception
                try
                {
                    try
                    {
                        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
                        Directory.SetCurrentDirectory(ConfigProxy.AdminPaqPath);
                        string initialDate = (ConfigProxy.FirstSync) ? "01/01/1990 00:00:00.00" : ConfigProxy.LastSync;
                        int iResult = 0;
                        if (ConfigProxy.FirstSync)
                        {
                            log.Trace("First Sync");
                            ConfigProxy.FirstSync = false;
                        }
                        log.Info("Initializing Compac SDK.");
                        iResult = CompacSDK.fInicializaSDK();
                        log.Info("Memory usage: " + process.WorkingSet64);
                        if (iResult != 0)
                            throw new HandySyncException("Unable to initialize SDK. Check AdminPAQ path specified in config file.");

                        log.Info("Opening company with Compac SDK in Path: " + ConfigProxy.AdminPaqCompanyPath);
                        iResult = CompacSDK.fAbreEmpresa(ConfigProxy.AdminPaqCompanyPath);
                        log.Info("Memory usage: " + process.WorkingSet64);
                        if (iResult != 0)
                            throw new HandySyncException("Unable to open company files. Check AdminPAQ path specified in config file.");

                        log.Trace("Company opened successfully: " + ConfigProxy.AdminPaqCompanyPath);

                        //Sync Products
                        if (ConfigProxy.SyncProducts)
                            SyncProducts(initialDate);
                        else
                            log.Info("Products didn't sync because the application is not configured to do so. ");
                        log.Info("Memory usage: " + process.WorkingSet64);
                        //Sync Clients
                        if (ConfigProxy.SyncClients)
                            SyncClients(initialDate);
                        else
                            log.Info("Clients didn't sync because the application is not configured to do so. ");
                        log.Info("Memory usage: " + process.WorkingSet64);
                        //Sync Orders
                        if (ConfigProxy.SyncOrders)
                            SyncOrders(initialDate);
                        else
                            log.Info("Orders didn't sync because the application is not configured to do so. ");
                        log.Info("Memory usage: " + process.WorkingSet64);
                        ConfigProxy.LastSync = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ff");
                    }
                    catch (HandySyncException he)
                    {
                        log.Fatal(he.Message);
                    }
                    catch (ExternalException eex)
                    {
                        log.Error("External Exception ocurred. Code: " + eex.ErrorCode + " Message: " + eex.Message);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error in main process: " + ex.Message);
                    }
                    finally
                    {
                        try
                        {
                            log.Info("Closing company. [fCierraEmpresa]");
                            CompacSDK.fCierraEmpresa();
                            log.Info("Memory usage: " + process.WorkingSet64);
                            try
                            {
                                log.Info("Terminating Compac SDK. [fTerminaSDK]");
                                CompacSDK.fTerminaSDK();
                                log.Info("Memory usage: " + process.WorkingSet64);
                            }
                            catch (ExternalException eex)
                            {
                                log.Error("External Exception ocurred while trying to finalize Compac SDK. Code: " + eex.ErrorCode + " Message: " + eex.Message);
                            }
                            catch (Exception ex2)
                            {
                                log.Error("Error in finally block while trying to finalize Compac SDK: " + ex2.Message);
                            }
                        }
                        catch (ExternalException eex)
                        {
                            log.Error("External Exception ocurred while trying to close company. Code: " + eex.ErrorCode + " Message: " + eex.Message);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error in finally block while trying to close company: " + ex.Message);    
                        }
                        finally
                        {
                            //Waits the specified time for next sync
                            log.Info("Iteration finished.  Waiting specified time for next cycle.");
                            GC.Collect();
                            Thread.Sleep(waitTime);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error in finally block while trying to finalize Compac SDK: " + ex.Message);
                    try
                    {
                        log.Info("Attempting to terminate Compac SDK. [fTerminaSDK]");
                        CompacSDK.fTerminaSDK();
                        log.Info("Memory usage: " + process.WorkingSet64);
                    }
                    catch (Exception ex1)
                    {
                        log.Error("Error while attempting to terminate Compac SDK: " + ex1.Message);
                    }
                    finally
                    {
                        GC.Collect();
                        Thread.Sleep(waitTime);
                    }
                }
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public static void MainProcess2()
        {
            process = Process.GetCurrentProcess();
            try
            {
                ConfigProxy.OpenConfigFile();
            }
            catch (Exception ex)
            {
                log.Error("Unable to open config file: " + ex.Message);
                Console.ReadKey();
                return;
            }
            int waitTime = ConfigProxy.SyncPeriodMinutes * 60 * 1000;
            log.Debug("Starting service.");
            log.Trace("Initializing...");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
                //Adding a higher level Exception handling in case the finally block throws an exception
                try
                {
                    try
                    {
                        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
                        Directory.SetCurrentDirectory(ConfigProxy.AdminPaqPath);
                        string initialDate = (ConfigProxy.FirstSync) ? "01/01/1990 00:00:00.00" : ConfigProxy.LastSync;
                        int iResult = 0;
                        if (ConfigProxy.FirstSync)
                        {
                            log.Trace("First Sync");
                            ConfigProxy.FirstSync = false;
                        }
                        log.Info("Initializing Compac SDK.");
                        iResult = CompacSDK.fInicializaSDK();
                        log.Info("Memory usage: " + process.WorkingSet64);
                        if (iResult != 0)
                            throw new HandySyncException("Unable to initialize SDK. Check AdminPAQ path specified in config file.");

                        log.Info("Opening company with Compac SDK in Path: " + ConfigProxy.AdminPaqCompanyPath);
                        iResult = CompacSDK.fAbreEmpresa(ConfigProxy.AdminPaqCompanyPath);
                        log.Info("Memory usage: " + process.WorkingSet64);
                        if (iResult != 0)
                            throw new HandySyncException("Unable to open company files. Check AdminPAQ path specified in config file.");

                        log.Trace("Company opened successfully: " + ConfigProxy.AdminPaqCompanyPath);

                        //Sync Products
                        if (ConfigProxy.SyncProducts)
                            SyncProducts(initialDate);
                        else
                            log.Info("Products didn't sync because the application is not configured to do so. ");
                        log.Info("Memory usage: " + process.WorkingSet64);
                        //Sync Clients
                        if (ConfigProxy.SyncClients)
                            SyncClients(initialDate);
                        else
                            log.Info("Clients didn't sync because the application is not configured to do so. ");
                        log.Info("Memory usage: " + process.WorkingSet64);
                        //Sync Orders
                        if (ConfigProxy.SyncOrders)
                            SyncOrders(initialDate);
                        else
                            log.Info("Orders didn't sync because the application is not configured to do so. ");
                        log.Info("Memory usage: " + process.WorkingSet64);
                        ConfigProxy.LastSync = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ff");
                    }
                    catch (HandySyncException he)
                    {
                        log.Fatal(he.Message);
                    }
                    catch (ExternalException eex)
                    {
                        log.Error("External Exception ocurred. Code: " + eex.ErrorCode + " Message: " + eex.Message);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error in main process: " + ex.Message);
                    }
                    finally
                    {
                        try
                        {
                            log.Info("Closing company. [fCierraEmpresa]");
                            CompacSDK.fCierraEmpresa();
                            log.Info("Memory usage: " + process.WorkingSet64);
                            try
                            {
                                log.Info("Terminating Compac SDK. [fTerminaSDK]");
                                CompacSDK.fTerminaSDK();
                                log.Info("Memory usage: " + process.WorkingSet64);
                            }
                            catch (ExternalException eex)
                            {
                                log.Error("External Exception ocurred while trying to finalize Compac SDK. Code: " + eex.ErrorCode + " Message: " + eex.Message);
                            }
                            catch (Exception ex2)
                            {
                                log.Error("Error in finally block while trying to finalize Compac SDK: " + ex2.Message);
                            }
                        }
                        catch (ExternalException eex)
                        {
                            log.Error("External Exception ocurred while trying to close company. Code: " + eex.ErrorCode + " Message: " + eex.Message);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error in finally block while trying to close company: " + ex.Message);
                        }
                        finally
                        {
                            //Waits the specified time for next sync
                            log.Info("Iteration finished.  Waiting specified time for next cycle.");
                            GC.Collect();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error in finally block while trying to finalize Compac SDK: " + ex.Message);
                    try
                    {
                        log.Info("Attempting to terminate Compac SDK. [fTerminaSDK]");
                        CompacSDK.fTerminaSDK();
                        log.Info("Memory usage: " + process.WorkingSet64);
                    }
                    catch (Exception ex1)
                    {
                        log.Error("Error while attempting to terminate Compac SDK: " + ex1.Message);
                    }
                    finally
                    {
                        GC.Collect();
                    }
                }
        }

    }
}
