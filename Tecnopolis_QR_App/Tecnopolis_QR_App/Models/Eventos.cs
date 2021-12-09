using System;

namespace Tecnopolis_QR_App.Models
{
    public class Eventos
    {
        public int idEventos { get; set; }
        public string Evento { get; set; }
        public DateTime Fecha { get; set; }
        public int idSalas { get; set; }
        public int Aforo { get; set; }
        public int Personas { get; set; }
        public DateTime FechaPub { get; set; }
        public DateTime FechaFin { get; set; }
    }
}