using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService.Model
{
    [DataContract]
    public class Order
    {
        public string ClientCode { get; set; }
        public double Amount { get; set; }
        public List<string> ProductCodes { get; set; }
        public List<double> ProductPrices { get; set; }
        public List<int> ProductQuantities { get; set; }
    }
}
