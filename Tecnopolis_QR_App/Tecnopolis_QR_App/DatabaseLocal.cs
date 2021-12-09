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

        public Task<int> SaveEntradaAsync(Entradas e)
        {
            if (e.idEntradas==0)
            {
                return db.InsertAsync(e);
            }
            else
            {
                return null;
            }
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
    }
}