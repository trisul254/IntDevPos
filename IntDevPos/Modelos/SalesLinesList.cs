using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Modelos
{
    public class SalesLinesList
    {
        public string productId { get; set; }
        public string salesUnit { get; set; }
        public string batchId { get; set; }
        public int salesQty { get; set; }
        public string siteId { get; set; }
        public string locationId { get; set; }
        public string salesPrice { get; set; }
        public string lineDisc { get; set; }
        public string multiLineDisc { get; set; }
    }
}