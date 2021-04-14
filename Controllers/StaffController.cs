
using API.Models.Pessoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Filters;

namespace API.Controllers
{
    [JwtAuthentication]
    public class StaffController : ApiController
    {
        [Route("api/getAllStaff")]
        public IHttpActionResult GetStaff()
        {
            List<Staff> staff = new Staff().GetAllStaff();
            return Ok(staff);
        }

    }
}
