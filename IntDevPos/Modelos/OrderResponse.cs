using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Modelos
{
    public class OrderResponse
    {
        public string orderReferencecode { get; set; }
        public string OrderId { get; set; }
        public bool Successful { get; set; }
        public string Mensaje { get; set; }
        
    }
}
