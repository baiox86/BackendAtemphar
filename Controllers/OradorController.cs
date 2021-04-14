
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
    public class OradorController : ApiController
    {
        
        /*GETS*/

        [Route("api/getAllOradores")]
        public IHttpActionResult GetStaff()
        {
            List<Orador> oradores = new Orador().GetAllOradores();
            return Ok(oradores);
        }

        /*ORADOR*/

        [Route("api/postOrador")]
        public IHttpActionResult POST([FromBody]Orador orador)
        {
            if (!Orador.CreateOrador(orador))
            {
                return BadRequest("Erro ao criar Orador!");
            }
            return Created("Orador",orador);
        }
    }
}
