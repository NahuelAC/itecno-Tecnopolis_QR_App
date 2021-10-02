using System;
using SQLite;

namespace Tecnopolis_QR_App.Models
{
    public class Clientes
    {
        [PrimaryKey,AutoIncrement] public int id { get; set; }
        public string espectaculo_id { get; set; }
        [MaxLength(10)] public string dni { get; set; }
        public DateTime fechayhora { get; set; }
        [MaxLength(10)] public string personas { get; set; }
        [MaxLength(10)] public string sala { get; set; }
    }
}