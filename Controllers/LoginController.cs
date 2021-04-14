using API.Auth;
using API.Filters;
using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace API.Controllers
{
    public class LoginController : ApiController
    {
        [Route("api/login")]
        public HttpResponseMessage Post([FromBody]AuthUser user)
        {
            if(user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
             "Invalid Request");
            }

            if (CheckUser(user.Email, user.Password))
            {
      
                return Request.CreateResponse(HttpStatusCode.OK, JObject.Parse("{'token':'" + JwtAuthManager.GenerateJWTToken(user.Email) + "'}")
              , JsonMediaTypeFormatter.DefaultMediaType);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized,
             "Unauthorized");
            }
        }

        [JwtAuthentication]
        [Route("api/validateToken")]
        public HttpResponseMessage GetValidateToken()
        {
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public bool CheckUser(string username, string password)
        {
            return AuthUser.Authenticate(username,password) != null;
        }
    }
}
