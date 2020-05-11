using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Settings
{
    public interface ICustomerDatabaseSettings
    {
        public string CustomersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public class CustomerDatabaseSettings: ICustomerDatabaseSettings
    {
        public string CustomersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
