using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksOnDemand.Models
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}