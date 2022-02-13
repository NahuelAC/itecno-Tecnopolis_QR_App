#nullable enable
using System;
using SQLite;

namespace Tecnopolis_QR_App.Models
{
    public class Entradas
    {
        public int idEntradas { get; set; }
        public int idEventos { get; set; }
        public string DNI { get; set; }
        public DateTime FechaV { get; set; }
        public int Visitantes { get; set; } 
        public DateTime? Show { get; set; }
    }
}