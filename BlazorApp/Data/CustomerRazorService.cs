using BlazorApp.Models;
using Data.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Data
{
    public interface ICustomerRazorService
    {
        Task<ResponseCustomerApiModel> GetCustomersAsync(string token);
        Task<ResponseCustomerApiModel> GetCustomerAsync(string id, string token);
        Task<int> DeleteAsync(string id, string token);
        Task<int> UpdateAsync(Models.Customer customer, string id, string token);
        Task<int> CreateAsync(Models.Customer customer, string token);
        bool CheckForApiToken();
        Task<string> GetApiToken(string username, string password);

    }
    public class CustomerRazorService: ICustomerRazorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerRazorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseCustomerApiModel> GetCustomersAsync(string token)
        {
            
            HttpClient client = new HttpClient();
            var apiUrl = "http://localhost:52368/api/customer/all";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer "+token);
            try
            {
                var response = await client.GetAsync(apiUrl);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ResponseCustomerApiModel
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                }
                var content = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Models.Customer>>(content);
                var model = new ResponseCustomerApiModel
                {
                    Customers = customers,
                    StatusCode = (int)response.StatusCode
                };
                return model;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
        public async Task<ResponseCustomerApiModel> GetCustomerAsync(string id, string token)
        {
            HttpClient client = new HttpClient();
            var apiUrl = "http://localhost:52368/api/customer/" + id ;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            // var response = await client.GetStringAsync(apiUrl);
            var response = await client.GetAsync(apiUrl);
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new ResponseCustomerApiModel
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            var content = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Models.Customer>(content);
            var model = new ResponseCustomerApiModel
            {
                Customer = customer,
                StatusCode = (int)response.StatusCode
            };
            return model;
        }
        public async Task<int> DeleteAsync( string id, string token)
        {
            try
            {
                var apiUrl = "http://localhost:52368/api/customer/" + id;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var response = await client.DeleteAsync(apiUrl);
                     return (int)response.StatusCode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<int> UpdateAsync(Models.Customer customer,string id, string token)
        {
            try
            {
                var apiUrl = "http://localhost:52368/api/customer/"+id;
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var response = await client.PutAsync(apiUrl, content);
                    return (int)response.StatusCode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<int> CreateAsync(Models.Customer customer, string token)
        {
            try
            {
                var apiUrl = "http://localhost:52368/api/customer/create";
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var response = await client.PostAsync(apiUrl, content);
                    return (int)response.StatusCode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool CheckForApiToken()
        {
            var apiToken = _httpContextAccessor.HttpContext.Session.GetString("apiToken");
            
            if (!string.IsNullOrEmpty(apiToken))
            {
                return true;
            }

            return false;
        }
        public async Task<string> GetApiToken(string username, string password)
        {
            try
            {
                var apiUrl = "https://demo.identityserver.io/connect/token";
                using (HttpClient client = new HttpClient())
                {
                    var byteArray = Encoding.ASCII.GetBytes(username+":"+password);
                    client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));
                    var parameters = new Dictionary<string, string> {
                        { "grant_type", "client_credentials" }
                    };
                    var encodedContent = new FormUrlEncodedContent(parameters);
                    var response = await client.PostAsync(apiUrl, encodedContent);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var contents = await response.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<Models.TokenResponse>(contents);
                        // Set Token to session 
                        return token.access_token;
                    }
                   
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
