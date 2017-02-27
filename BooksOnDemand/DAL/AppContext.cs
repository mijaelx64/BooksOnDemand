using BooksOnDemand.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IList<Book> GetBooks() {
            
            var collections = _database.GetCollection<Book>("Book");
            List<Book> books = new List<Book>();
            var booksCollection = collections.Find<Book>(new BsonDocument()).ToList();

            return books;
        }

    }
}