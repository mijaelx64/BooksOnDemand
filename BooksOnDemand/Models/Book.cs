using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksOnDemand.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<string> Authors { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        //public List<ObjectId> UserDemands { get; set; }
    }
}