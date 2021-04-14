using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    public class FichaIndividualController : ApiController
    {
        [Route("api/getFichaPessoa/{id}")]
        public IHttpActionResult GetFichaPessoa(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            JObject json = FichaIndividual.GetFichaPessoa(id);
            if (json==null)
            {
                return BadRequest(); //Os erros considerados aqui são principalmente um id de alguem que não exista
            }
            return Ok(json);
            
        }
        [Route("api/getFichaStaff/{id}")]
        public IHttpActionResult GetFichaStaff(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            
            JObject json = FichaIndividual.GetFichaStaff(id);
            if (json == null)
            {
                return BadRequest(); //Os erros considerados aqui são principalmente um id de alguem que não exista
            }
            return Ok(json);

        }
        [Route("api/getFichaOrador/{id}")]
        public IHttpActionResult GetFichaOrador(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            JObject json = FichaIndividual.GetFichaOrador(id);
            if (json == null)
            {
                return BadRequest(); //Os erros considerados aqui são principalmente um id de alguem que não exista, ou que não esteja associado como orador ainda
            }
            return Ok(json);

        }
    }
}