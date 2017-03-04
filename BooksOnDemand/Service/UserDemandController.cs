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
        /// <summary>
        /// POST: Add a book demand for an specific user.
        /// TODO: POST: api/userdemand/ Params: UserDemand Object  - Make Documentation 
        /// </summary>
        /// <param name="postRequest"></param>
        /// <returns></returns>
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

        /// <summary>
        /// GET: Get if a book is already demanded by a user.
        /// TODO: api/userdemand?userId={userId}&bookId={bookId} Documentation
        /// </summary> 
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// GET: Get User demands Object that contains demanded books for an specific user. 
        /// TODO: api/userdemand/{id} Documentation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
