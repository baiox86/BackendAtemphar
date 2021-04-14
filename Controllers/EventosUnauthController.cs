using API.Models.Evento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class EventosUnauthController : ApiController
    {
        [Route("api/getDadosEventosByToken/{token}")]
        public IHttpActionResult GetDadosEventoByToken(string token)
        {
            Evento evento = Evento.GetEventoByToken(token);

            if(evento == null)
            {
                return NotFound();
            }

            if(DateTime.Compare(DateTime.Parse(evento.dataEvento),DateTime.Now) <= 0){
                return StatusCode(HttpStatusCode.Gone);
            } 

            return Ok(evento);
        }
    }
}
