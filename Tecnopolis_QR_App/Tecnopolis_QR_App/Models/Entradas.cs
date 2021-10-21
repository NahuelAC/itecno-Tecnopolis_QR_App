using System;

namespace Tecnopolis_QR_App.Models
{
    public class Entradas
    {
        public int idEntradas { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaV { get; set; }
        public DateTime Fecha { get; set; }
        public string email { get; set; }
        public string Evento { get; set; }
        public int Visitantes { get; set; }
        public int idEventos { get; set; }
        public string idProvincia { get; set; }
        public int idLocalidad { get; set; }
    }
}