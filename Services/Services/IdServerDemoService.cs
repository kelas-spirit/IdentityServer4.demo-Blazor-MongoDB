using Data.Dto;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Services.Services
{
    public interface IIdServerDemoService
    {
        Task<string> GetApiToken(string username, string password);
    }
    public class IdServerDemoService: IIdServerDemoService
    {
        public IdServerDemoService()
        {
        }
        public async Task<string> GetApiToken(string username, string password)
        {
            try
            {
                var apiUrl = "https://demo.identityserver.io/connect/token";
                using (HttpClient client = new HttpClient())
                {
                    var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
                    client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));
                    var parameters = new Dictionary<string, string> {
                        { "grant_type", "client_credentials" }
                    };
                    var encodedContent = new FormUrlEncodedContent(parameters);
                    var response = await client.PostAsync(apiUrl, encodedContent);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var contents = await response.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<TokenResponseDTO>(contents);
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
