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

        public Task SaveClientesAsync(Clientes clients)
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

        public Task<List<Clientes>> GetClienteByDni(string dni)
        {
            return db.Table<Clientes>().Where(c => c.dni == dni).ToListAsync();
        }

        public Task<List<Clientes>> GetAllClientes()
        {
            return db.Table<Clientes>().ToListAsync();
        }
        public Task SaveEntradasAsync(Entradas entrada)
        {
            if (entrada.idEntradas==0)
            {
                return db.InsertAsync(entrada);
            }
            else
            {
                return null;
            }
        }

        public Task<List<Entradas>> GetEntradaByDni(string dni)
        {
            return db.Table<Entradas>().Where(c => c.DNI == dni).ToListAsync();
        }

        public Task<List<Entradas>> GetAllEntradas()
        {
            return db.Table<Entradas>().ToListAsync();
        }
        
    }
}