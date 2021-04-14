
using API.Filters;
using API.Models.Pessoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [JwtAuthentication]
    public class AmigoSocioController : ApiController
    {
        [Route("api/getAllAmigoSocio")]
        public IHttpActionResult GetAllAmigoSocio()
        {
            List<AmigoSocio> amigos = new AmigoSocio().GetAmigosSocio();
            return Ok(amigos);
        }
        [Route("api/getAmigoSocio/{id}")]
        public IHttpActionResult GetAmigoSocio(int id)
        {
            return Ok(new AmigoSocio().GetAmigoSocio(id));
        }

        [Route("api/postAmigoSocio")]
        public IHttpActionResult postAmigoSocio([FromBody]AmigoSocio amigoSocio)
        {
            if (!AmigoSocio.CreateAmigoSocio(amigoSocio))
            {
                return BadRequest("Erro ao criar Amigo Sócio!");
            }
            return Created("AmigoSocio", amigoSocio);
        }



    }
}
