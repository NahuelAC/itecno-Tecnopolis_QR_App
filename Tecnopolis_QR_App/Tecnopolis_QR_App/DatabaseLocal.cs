using System;
using System.Threading.Tasks;
using SQLite;
using Tecnopolis_QR_App.Models;

namespace Tecnopolis_QR_App
{
    public class DatabaseLocal
    { 
        SQLiteAsyncConnection db;

        public DatabaseLocal(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Clientes>().Wait();
        }

        public Task<int> SaveClientesAsync(Clientes clients)
        {
            if (clients.id==0)
            {
                return db.InsertAsync(clients);
            }
            else
            {
                return null;
            }
        }

        //public Task<Clientes> GetClienteByDniAsync(string Dni, DateTime Dt)
        public Task<Clientes> GetClienteByDniAsync(string Dni)
        {
            return db.Table<Clientes>().Where(a => a.dni == Dni).FirstOrDefaultAsync();
        }
    }
}