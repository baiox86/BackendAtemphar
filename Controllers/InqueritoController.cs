
using API.Models.Pessoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Filters;
using API.Models.Inquerito;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json;

namespace API.Controllers
{
    [JwtAuthentication]
    public class InqueritoController : ApiController
    {
        //Corda do "molho" de respostas
        int i = 1;

        [Route("api/postInquerito")]
        [HttpPost]
        public IHttpActionResult PostInquerito([FromBody]JObject json)
        {

            Debug.WriteLine(json);
            long check = Inquerito.CriarInquerito(json);
            if (check ==-1)
            {
                return BadRequest();
            }
            return Created("Inquerito",json);
            
        }
        [Route("api/postAssociarInquerito")]
        [HttpPost]
        public IHttpActionResult PostAssociarInquerito([FromBody] JObject j)
        {
            int idEvento = (int)j["idEvento"];
            int idInquerito = (int)j["idInquerito"];

            long check = Inquerito.MapEventoInquerito(idEvento,idInquerito);
            if (check == -1)
            {
                return BadRequest();
            }
            return Created("Inquerito","Evento associado");

        }
        [Route("api/getAllInqueritos")]
        public IHttpActionResult GetInqueritos()
        {
            return Ok(Inquerito.GetAllInqueritos());

        }
        [Route("api/getInquerito/{id}")]
        public IHttpActionResult GetInqueritos([FromUri]int id)
        {
            return Ok(Inquerito.GetInquerito(id));
        }

        [Route("api/getAllInqueritosEvento")]
        public IHttpActionResult GetInqueritosEvento()
        {
            return Ok(Inquerito.GetAllInqueritosEvento());

        }
        [Route("api/getRespostasInqueritoQuestao/{idInquerito}/{idEvento}/{idQuestao}")]
        public IHttpActionResult GetRespostasInqueritoQuestao(int idInquerito, int idEvento, int idQuestao)
        {
            return Ok(Resposta.GetAllRespostasInqueritoQuestao(idInquerito,idEvento,idQuestao));

        }

        [Route("api/getRespostasInquerito/{idInquerito}/{idEvento}")]
        public IHttpActionResult GetRespostasInquerito(int idInquerito, int idEvento)
        {
            return Ok(Resposta.GetAllRespostasInquerito(idInquerito,idEvento));

        }

        [Route("api/editEstadoInquerito/{idInquerito}/{idEvento}")]
        public IHttpActionResult PatchEstadoInquerito(int idInquerito,int idEvento)
        {
            int result = Inquerito.UpdateEstadoInquerito(idInquerito, idEvento);

            if (result == -1)
            {
                return BadRequest();
            }else if(result == 0)
            {
                return NotFound();
            }

            return Ok();

        }
    }
}
