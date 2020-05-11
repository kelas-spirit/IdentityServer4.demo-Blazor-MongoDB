using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models
{
    public class ResponseCustomerApiModel
    {
        public Models.Customer Customer { get; set; }
        public List<Models.Customer> Customers { get; set; }
        public int StatusCode { get; set; }
    }
}
