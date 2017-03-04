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
    public class LoginController : ApiController
    {
        /// <summary>
        /// Login user by username and password. Get userId.
        /// POST: api/Login
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>UserID</returns>
        public IHttpActionResult PostLogin(LoginViewModel loginInfo)
        {
            var collections = CrossoverContext.Database.GetCollection<User>("User");

            var resultObj = collections.Find<User>(new BsonDocument()).ToList()
                .Where(x => x.Username.Equals(loginInfo.UserName) && x.Password.Equals(loginInfo.Password))
                .FirstOrDefault();

            if (resultObj != null)
            {
                return Ok(resultObj.Id);
            }

            return Ok();
        }
    }

    public class RegisterController : ApiController
    {
        /// <summary>
        /// POST: Register User by sending RegisterViewModel object. 
        /// api/register/ RegisterViewModel - Make Documentation
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IHttpActionResult PostRegister(RegisterViewModel user)
        {
            try
            {
                var collections = CrossoverContext.Database.GetCollection<User>("User");
                collections.InsertOne(new User() { Username = user.UserName, Password = user.Password });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
