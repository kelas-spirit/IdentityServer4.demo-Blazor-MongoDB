using Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Test.Service
{
    public interface ITestService
    {
        Task<List<Customer>> GetCustomersAsync(string token);
    }
    public class TestService: ITestService
    {
        public TestService()
        {
        }
        public async Task<List<Customer>> GetCustomersAsync(string token)
        {

            HttpClient client = new HttpClient();
            var apiUrl = "http://localhost:52368/api/customer/all";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            try
            {
                var response = await client.GetAsync(apiUrl);
                var content = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(content);
                return customers;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
