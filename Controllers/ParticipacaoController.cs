using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Filters;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    
    public class ParticipacaoController : ApiController
    {
        [JwtAuthentication]
        
        [Route("api/deleteParticipacao/{idE}/{idP}")]
        public IHttpActionResult DeleteParticipacao(int idE,int idP)
        {
            bool check = Participacao.DeleteParticipacao(idE, idP);
            if (check)
            {
                return Ok("Deleted");
            }
            return BadRequest();
        }


        [JwtAuthentication]
        /*POSTS*/
        [Route("api/postParticipacaoEvento")]
        public IHttpActionResult PostParticipacao([FromBody]Participacao participacao)
        {
            int result = Participacao.CreateParticipacao(participacao);
            if (result == -1)
            {
                return BadRequest("Erro ao registar Participação no Evento!");
            }
            else if(result == -2)
            {
                return Conflict();
            }
            return Created("Participacao",participacao);
        }

        [JwtAuthentication]
        /*POSTS*/
        [Route("api/patchComparecimentoInscricao/{idEvento}/{idParticipante}")]
        public IHttpActionResult PatchParticipacaoEvento([FromUri]int idEvento, [FromUri] int idParticipante)
        {
            int result = Participacao.RegistarComparecimentoInscricaoEventoParticipante(idEvento, idParticipante);
            if (result == -1)
            {
                return BadRequest("Erro ao registar comparecimento no Evento!");
            }
            else if (result == -2)
            {
                return Conflict();
            }
            return Ok();
        }

        [JwtAuthentication]
        /*POSTS*/
        [Route("api/patchComparecimentoInscricao/{idEvento}")]
        public IHttpActionResult PatchParticipacaoEventoParticipante([FromUri]int idEvento)
        {
            int result = Participacao.RegistarComparecimentoInscricaoEvento(idEvento);
            if (result == -1)
            {
                return BadRequest("Erro ao registar comparecimento no Evento!");
            }
            else if (result == -2)
            {
                return Conflict();
            }
            return Ok();
        }


        /*POSTS*/
        [Route("api/postParticipacaoEventoEmail")]
        public IHttpActionResult PostParticipacaoEmail([FromBody]JObject participacao)
        {
            Participacao.CreateParticipacaoEmail(participacao);
      
            return Ok();
        }

        [Route("api/postParticipacaoEventoTelemovel")]
        public IHttpActionResult PostParticipacaoTelemovel([FromBody]JObject participacao)
        {
            int check = Participacao.CreateParticipacaoTelemovel(participacao);
            if (check==-1)
            {
                return BadRequest();
            }

            return Created("Participacao",participacao);
        }


    }
}
