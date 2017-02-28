using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksOnDemand.Models
{
    public class Book
    {
        public ObjectId Id { get; set; }
        public List<string> Authors { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        public List<ObjectId> UserDemands { get; set; }
    }
}