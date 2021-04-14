using API.Models.Pessoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class PessoaUnauthController : ApiController
    {
        [Route("api/postPessoaInscricao/{token}")]
        public IHttpActionResult POST([FromBody]Pessoa pessoa, string token)
        {
            long id = new Pessoa().CreatePessoaInscricao(pessoa, token);
            if (id < 0)
            {
                return BadRequest();
            }
            return Created("Pessoa",id);
        }
    }
}
