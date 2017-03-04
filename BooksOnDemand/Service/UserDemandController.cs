using BooksOnDemand.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BooksOnDemand.Service
{
    public class UserDemandController : ApiController
    {

        [HttpPost]
        public IHttpActionResult PostUserDemand(UserDemand postRequest)
        {
            if (postRequest == null || postRequest.BooksDemands.Count != 1)
            {
                return BadRequest();
            }

            var collection = CrossoverContext.Database.GetCollection<UserDemand>("UserDemand");

            FilterDefinition<UserDemand> filter = Builders<UserDemand>.Filter.Eq(p => p.UserId, postRequest.UserId);
            var obj = collection.Find<UserDemand>(filter).FirstOrDefault();

            if (obj != null)
            {
                var userDemandUpdate = Builders<UserDemand>.
                    Update.Push("BooksDemands", postRequest.BooksDemands[0]);

                collection.UpdateOne(filter, userDemandUpdate);
            }
            else
            {
                collection.InsertOne(postRequest);
            }

            return Ok();
        }

        public IHttpActionResult GetUserDemand(string userId, string bookId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(bookId))
            {
                return BadRequest();
            }

            var collection = CrossoverContext.Database.GetCollection<UserDemand>("UserDemand");

            FilterDefinition<UserDemand> filter = Builders<UserDemand>.Filter.Eq(p => p.UserId, userId);
            return Ok(collection.Find<UserDemand>(filter).FirstOrDefault().BooksDemands.Contains(bookId));
        }



        /*[HttpPost]
        public void DemandBook(string bookId, string userId)
        {
            /*var bookCollection = Database.GetCollection<Book>("Book");

            var bookFilter = Builders<Book>.Filter.Eq(p => p.Id, bookId);
            var bookUpdate = Builders<Book>.Update.Push("UserDemands", ObjectId.Parse(userId));
            bookCollection.UpdateOne(bookFilter, bookUpdate);

            var userCollection = Database.GetCollection<User>("User");

            var userFilter = Builders<User>.Filter.Eq(p => p.Id, ObjectId.Parse(userId));
            var userUpdate = Builders<User>.Update.Push("BookDemands", ObjectId.Parse(bookId));
            userCollection.UpdateOne(userFilter, userUpdate);
        }*/
    }
}
