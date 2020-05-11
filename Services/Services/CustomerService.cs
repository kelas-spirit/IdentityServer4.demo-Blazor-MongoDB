using Data.Model;
using Data.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public interface ICustomerService
    {
        List<Customer> Get();
        Customer Get(string id);
        Customer Create(Customer customer);
        void Update(string id, Customer customerIn);
        void Remove(Customer customerIn);
        void Remove(string id);
    }
    public class CustomerService: ICustomerService
    {
        private readonly IMongoCollection<Customer> _customers;
        public CustomerService(ICustomerDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _customers = database.GetCollection<Customer>(settings.CustomersCollectionName);
        }
        public List<Customer> Get() =>
            _customers.Find(customer => true).ToList();

        public Customer Get(string id) =>
            _customers.Find<Customer>(customer => customer.Id == id).FirstOrDefault();

        public Customer Create(Customer customer)
        {
            _customers.InsertOne(customer);
            return customer;
        }

        public void Update(string id, Customer customerIn) =>
            _customers.ReplaceOne(customer => customer.Id == id, customerIn);

        public void Remove(Customer customerIn) =>
            _customers.DeleteOne(customer => customer.Id == customerIn.Id);

        public void Remove(string id) =>
            _customers.DeleteOne(customer => customer.Id == id);
    }
}
