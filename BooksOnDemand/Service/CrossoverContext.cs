using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksOnDemand.Service
{
    public static class CrossoverContext
    {
        private const string DBNAME = "Crossover";
        private const string CONNECTIONSTRING = "mongodb://localhost:27017";

        private static MongoClient _client;
        private static IMongoDatabase _database;

        /// <summary>
        /// Mongo DB Reference
        /// </summary>
        public static IMongoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _client = new MongoClient(CONNECTIONSTRING);
                    _database = _client.GetDatabase(DBNAME);
                }
                return _database;
            }
        }
    }
}