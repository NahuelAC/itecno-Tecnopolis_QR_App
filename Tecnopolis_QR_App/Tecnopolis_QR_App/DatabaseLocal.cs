using System;
using System.Collections.Generic;
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
            db.CreateTableAsync<Entradas>().Wait();
        }

 

        public Task<List<Entradas>> GetEntradasByDniAsync(string Dni)
        {
            return db.Table<Entradas>().Where(e => e.DNI == Dni).ToListAsync();
        }
        
        public Task<Entradas> GetEntradasByIdAsync(string idEntradas)
        {
            return db.Table<Entradas>().Where(e => e.DNI == idEntradas).FirstOrDefaultAsync();
        }

        public Task<List<Entradas>> GetAllEntradas()
        {
            return db.Table<Entradas>().ToListAsync();
        }
        
        public Task DeleteEntradaAsync(Entradas entrada)
        {
            return db.DeleteAsync(entrada);
        }
        public Task DeleteClienteAsync(Clientes entrada)
        {
            return db.DeleteAsync(entrada);
        }
        public Task DeleteAllClientesAsync()
        {
            return db.DeleteAllAsync<Clientes>();
        }

        public Task SaveClientesAsync(Clientes clients)
        {
            if (clients.id != 0)
            {
                return db.UpdateAsync(clients);
            }
            else
            {
                return db.InsertAsync(clients);
            }
        }

        public Task<List<Clientes>> GetClienteByDni(string dni)
        {
            return db.Table<Clientes>().Where(c => c.dni == dni).ToListAsync();
        }

        public Task<List<Clientes>> GetAllClientes()
        {
            return db.Table<Clientes>().ToListAsync();
        }
    }
}