using BooksOnDemand.Models;
using BooksOnDemand.Service;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BooksOnDemand.Controllers.Service
{
    public class BooksController : ApiController
    {
        public IEnumerable<Book> GetAllBooks()
        {
            return GetAllBooks(string.Empty);
        }

        public IEnumerable<Book> GetAllBooks(string searchString)
        {
            var collection = CrossoverContext.Database.GetCollection<Book>("Book");

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                var result = collection.Find<Book>(new BsonDocument()).ToList()
                    .Where(s => s.Title.ToLower().Contains(searchString)
                                       || s.Authors.Where(x => x.ToLower().Contains(searchString)).Count() >= 1
                                       || s.Publisher.ToLower().Contains(searchString));
                return result;
            }

            return collection.Find<Book>(new BsonDocument()).ToEnumerable();
        }

        public IHttpActionResult GetBook(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("An Id must be set in order to find your request.");
            }

            try
            {
                var collection = CrossoverContext.Database.GetCollection<Book>("Book");
                FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(p => p.Id, id);
                var obj = collection.Find<Book>(filter).FirstOrDefault();

                if (obj != null)
                    return Ok(obj);

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
