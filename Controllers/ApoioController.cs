using API.Filters;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [JwtAuthentication]
    public class ApoioController: ApiController
    {
        [Route("api/getApoiosStaff/{id}")]
        public IHttpActionResult GetApoioByStaff(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(Apoio.GetApoiosByStaff(id));
        }
        [Route("api/getApoiosEvento/{id}")]
        public IHttpActionResult GetApoioByEvento(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(Apoio.GetApoiosEvento(id));
        }

        [Route("api/postApoio")]
        public IHttpActionResult PostApoio([FromBody]Apoio apoio)
        {
            bool check = Apoio.CreateApoio(apoio);
            if (check)
            {
                return Created("Apoio",apoio);
            }
            return BadRequest();
        }
        /*PUTS*/
        [Route("api/editApoio/{id}")]
        public IHttpActionResult PutApoio([FromBody]Apoio apoio,int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            if (!Apoio.EditApoio(id, apoio))
            {
                return BadRequest("Erro ao editar Apoio!");
            }

            return Ok();
        }
    }
}
