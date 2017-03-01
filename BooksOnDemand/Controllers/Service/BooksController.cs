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
        public string Get(int id)
        {
            return "value";
        }
    }
}
