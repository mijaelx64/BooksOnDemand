using BooksOnDemand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace BooksOnDemand.Controllers
{
    public static class CrossoverClient
    {
        private const string SERVICEURL = "http://localhost:55464";
        
        public static string GetJSON(string request) {

            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(SERVICEURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = client.GetAsync(request).Result;

                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsStringAsync().Result;
            }

            throw new HttpResponseException(response);
        }

        public static HttpResponseMessage PostRequest<T>(string RequestURI, T content)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(SERVICEURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync(RequestURI, content).Result;
            return response;
        }
    }
}