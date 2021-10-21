using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tecnopolis_QR_App.Models;

namespace Tecnopolis_QR_App
{
    public class ApiClient
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<Entradas>> TApiGetTicketsAll()
        {
            HttpResponseMessage res = await client.GetAsync("http://itecno.com.ar:3000/api/tecnopolis/tickets/all");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> TApiGetTicketsByDni(string dni)
        {
            HttpResponseMessage res = await client.GetAsync($"http://itecno.com.ar:3000/api/tecnopolis/tickets/bydni/{dni}");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> TApiGetTicketsByDate(string date)
        {
            HttpResponseMessage res = await client.GetAsync($"http://itecno.com.ar:3000/api/tecnopolis/tickets/bydate/{date}");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }public static async Task<List<Entradas>> CCKApiGetTicketsAll()
        {
            HttpResponseMessage res = await client.GetAsync("http://itecno.com.ar:3000/api/cck/tickets/all");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> CCKApiGetTicketsByDni(string dni)
        {
            HttpResponseMessage res = await client.GetAsync($"http://itecno.com.ar:3000/api/cck/tickets/bydni/{dni}");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }
        public static async Task<List<Entradas>> CCKApiGetTicketsByDate(string date)
        {
            HttpResponseMessage res = await client.GetAsync($"http://itecno.com.ar:3000/api/cck/tickets/bydate/{date}");
            res.EnsureSuccessStatusCode();
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Entradas>>(resBody);
            
            return data;
        }

    }
}