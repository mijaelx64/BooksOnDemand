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

            var userDemandObj = collection.Find<UserDemand>(filter).FirstOrDefault();

            if (userDemandObj!=null)
            {
                return Ok(userDemandObj.BooksDemands.Contains(bookId));
            }

            return Ok(false);
        }

        public IHttpActionResult GetUserDemand(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("An Id must be set in order to find your request.");
            }

            try
            {
                var collection = CrossoverContext.Database.GetCollection<UserDemand>("UserDemand");

                FilterDefinition<UserDemand> filter = Builders<UserDemand>.Filter.Eq(p => p.UserId, userId);

                var userDemandObj = collection.Find<UserDemand>(filter).FirstOrDefault();

                if (userDemandObj != null)
                {
                    return Ok(userDemandObj);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
