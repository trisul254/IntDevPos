using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Modelos
{
    public class Principal
    {
        public string OrderreferenceId { get; set; }
        public string CustIdentification { get; set; }
        public string custPurchaseOrder { get; set; }
        public string Date { get; set; }
        public string observations { get; set; }
        public string dlvDate { get; set; }
        public string totalAmount { get; set; }
        public string domicileAmount { get; set; }
        public string paymTerm { get; set; }
        public Customer customer { get; set; }
        public List<SalesLinesList> salesLinesList { get; set; }
    }
}
