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
    interface IAdminPaqService
    {
        [OperationContract]
        int GetClientChanges(ref List<CustomerSupplier> lstClientes, string sFrom);
        [OperationContract]
        int GetProductChanges(ref List<Product> lstProducts, string sFrom);
        [OperationContract]
        int GetAddress(ref Address address, string code, int type);
        [OperationContract]
        int SaveOrder(Order order);
        [OperationContract]
        int GetAgent(string clientCode, ref string agent);
    }
}
