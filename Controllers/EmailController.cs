using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;

namespace API.Controllers
{
    public class EmailController : ApiController
    {

        [Route("api/requestPasswordReset")]
        public HttpResponseMessage PostPasswordReset([FromBody]JObject username)
        {
            if (username == null || !username.ContainsKey("username") || username.GetValue("username").ToString().IsEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Invalid Request");
            }

            AuthUser.RequestPasswordReset(username.GetValue("username").ToString());

            return Request.CreateResponse(HttpStatusCode.Created,"Email Sent");
            
        }



        [Route("api/changePassword/{token}")]
        public HttpResponseMessage PutPasswordChange(string token, [FromBody]JObject passwordChange)
        {
            if (passwordChange == null || !passwordChange.ContainsKey("passwordChange") || passwordChange.GetValue("passwordChange").ToString().IsEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            int result = AuthUser.ChangePassword(token, passwordChange.GetValue("passwordChange").ToString());

            if (result == -2)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
            else if (result == -1)
            {
               return Request.CreateResponse(HttpStatusCode.NotFound);

            }
            else if (result == 0)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
        }
    }

}
