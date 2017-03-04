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
        /// <summary>
        /// GET: Retrive all books with no search.
        /// TODO: /api/books/ Documentation 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Book> GetAllBooks()
        {
            return GetAllBooks(string.Empty);
        }

        /// <summary>
        /// GET: Retrive all books with no search.
        /// TODO: /api/books?searchString="{searchString}" Documentation 
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
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

        /// <summary>
        /// GET: Get a book by Id.
        /// TODO: /api/books?id="{id}" Documentation 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
