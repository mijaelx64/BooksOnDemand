using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksOnDemand.Models
{
    public class UserDemand
    {
        public UserDemand()
        {
            BooksDemands = new List<string>();
        }
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public List<string> BooksDemands { get; set; }
    }
}