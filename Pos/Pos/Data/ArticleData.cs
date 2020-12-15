using Pos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Data
{
    public class ArticleData
    {
        readonly SQLiteAsyncConnection _database;

        public ArticleData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Article>().Wait();
        }

        public Task<List<Article>> GetArticlesAsync()
        {
            return _database.Table<Article>().ToListAsync();
        }

        public Task<Article> GetArticleAsync(int id)
        {
            return _database.Table<Article>()
                            .Where(i => i.arid == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveArticleAsync(Article note)
        {
            if (note.arid != 0)
            {
                return _database.UpdateAsync(note);
            }
            else
            {
                return _database.InsertAsync(note);
            }
        }
        public Task<int> SaveListArticlesAsync(List<Article> note)
        {
          
                return _database.InsertAllAsync(note);
          
        }

        public Task<int> DeleteArticleAsync(Article note)
        {
            return _database.DeleteAsync(note);
        }
        public Task<int> FillOverTableAsync(List<Article> note)
        {
            _database.DropTableAsync<Article>().Wait();
            _database.CreateTableAsync<Article>().Wait();

            return _database.InsertAllAsync(note);

            //if (note.Count == 0) { return null; }

            //try
            //{
            //    return _database.InsertOrReplaceAsync(note);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return null;
            //}
        }
    }
}

