#nullable enable
using System;
using SQLite;

namespace Tecnopolis_QR_App.Models
{
    public class Entradas
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
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
        public string idLocalidad { get; set; }
        public DateTime? Show { get; set; }
        public string Sid { get; set; }
    }
}