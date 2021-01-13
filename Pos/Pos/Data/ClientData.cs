using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Data
{
    public class ClientData
    {

        readonly SQLite.SQLiteAsyncConnection _database;

        public ClientData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            try
            {
             _database.CreateTableAsync<Client>().Wait();
            }
            catch (Exception)
            {
                _database.DropTableAsync<Client>().Wait();
                _database.CreateTableAsync<Client>().Wait();
            }

          
        }

        public Task<List<Client>> GetArticlesAsync()
        {
            return _database.Table<Client>().ToListAsync();
        }

        public Task<Client> GetArticleAsync(int id)
        {
            return _database.Table<Client>()
                            .Where(i => i.Clid == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveArticleAsync(Client note)
        {
            if (note.Clid != 0)
            {
                return _database.UpdateAsync(note);
            }
            else
            {
                return _database.InsertAsync(note);
            }
        }
        public Task<int> SaveListArticlesAsync(List<Client> note)
        {

            return _database.InsertAllAsync(note);

        }

        public Task<int> DeleteArticleAsync(Client note)
        {
            return _database.DeleteAsync(note);
        }
        public Task<int> FillOverTableAsync(List<Client> note)
        {
            _database.DropTableAsync<Client>().Wait();
            _database.CreateTableAsync<Client>().Wait();

            return _database.InsertAllAsync(note);
        }
    }
}
