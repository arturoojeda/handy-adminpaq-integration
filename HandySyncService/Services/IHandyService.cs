using HandySyncService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService.Services
{
    [ServiceContract]
    interface IHandyService
    {
        [OperationContract]
        string SaveClients(List<Client> clientes);
        [OperationContract]
        string SaveProducts(List<HandyProduct> products);
        [OperationContract]
        string GetSalesOrders(string since);
    }
}
