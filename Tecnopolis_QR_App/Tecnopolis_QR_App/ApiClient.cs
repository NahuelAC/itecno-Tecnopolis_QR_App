using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Tecnopolis_QR_App.Models;

namespace Tecnopolis_QR_App
{
    public class ApiClient
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<List<Entradas>> GetAllTickets()
        {
            HttpResponseMessage res = 
                await _client.GetAsync("http://itecno.com.ar:3000/api/tecnopolis/tickets/all");
            
            string resBody = await res.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<Entradas>>(resBody);
        }
        
        public static async Task<List<Entradas>> GetTicketsByDateNow()
        {
            HttpResponseMessage res =
                await _client.GetAsync($"http://itecno.com.ar:3000/api/tecnopolis/tickets/bydate/{DateTime.Now.ToString("yyyy-MM-dd")}");
            
            string resBody = await res.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<Entradas>>(resBody);
        }

        public static async Task<List<Entradas>> GetTicketsByDni(string dni)
        {
            HttpResponseMessage res =
                await _client.GetAsync($"http://itecno.com.ar:3000/api/tecnopolis/tickets/bydni/{dni}/");
            
            string resBody = await res.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<Entradas>>(resBody);
        }

        public static async Task<List<Entradas>> PutTicket(string idEntrada, DateTime Show)
        {
            HttpResponseMessage res =
                await _client.PutAsync($"http://itecno.com.ar:3000/api/tecnopolis/tickets/show/{idEntrada}/{Show.ToString("yyyy-M-dd hh:mm:ss")}/{CrossDeviceInfo.Current.Id}", null);
            
            string resBody = await res.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<Entradas>>(resBody);
        }
        
        public static async Task<Eventos> GetEventoById(int id)
        {
            HttpResponseMessage res = await _client.GetAsync($"http://itecno.com.ar:3000/api/tecnopolis/eventos/byid/{id}");
            string resBody = await res.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<Eventos>>(resBody).First();

            return data;
        }
    }
}