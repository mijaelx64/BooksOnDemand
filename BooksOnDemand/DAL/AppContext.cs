using BooksOnDemand.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BooksOnDemand.DAL
{
    

    public class AppContext
    {
        private const string DBNAME = "Crossover";
        private const string CONNECTIONSTRING = "mongodb://localhost:27017";
        
        private MongoClient _client;
        private MongoServer _server;
        private IMongoDatabase _database;

        public IMongoDatabase Database {
            get {
                if (_database == null)
                {
                    _client = new MongoClient(CONNECTIONSTRING);
                    _database = _client.GetDatabase(DBNAME);
                }
                return _database;
            } 
        }

        public IEnumerable<Book> GetBooks() {
            
            var collections = Database.GetCollection<Book>("Book");
            List<Book> books = new List<Book>();
            
            return collections.Find<Book>(new BsonDocument()).ToEnumerable();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {

            var collections = Database.GetCollection<Book>("Book");

            //return Task.Run(() => collections.FindAsync<Book>(new BsonDocument()));
            //return await collections.FindAsync<Book>(new BsonDocument());

            using (var cursor = await collections.FindAsync<Book>(new BsonDocument()))
            {

                return cursor.ToEnumerable();

                /*while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        // process document
                        count++;
                    }
                }*/
            }

        }

    }
}