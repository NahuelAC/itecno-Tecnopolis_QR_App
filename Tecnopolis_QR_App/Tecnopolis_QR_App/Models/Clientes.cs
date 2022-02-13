using System;
using SQLite;

namespace Tecnopolis_QR_App.Models
{
    public class Clientes
    {
        [PrimaryKey, AutoIncrement] public int id { get; set; }
        public string espectaculo_id { get; set; }
        [MaxLength(15)] public string dni { get; set; }
        public DateTime fechayhora { get; set; }
        public DateTime? Show { get; set; }
    }
}