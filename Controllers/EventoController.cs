using API.Models.Evento;
﻿using API.Models;
using API.Models.Evento;
using API.Models.Trofeu;
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
    [JwtAuthentication]
    public class EventoController : ApiController
    {
        /*GETS*/

        [Route("api/getAllEventos")]
        public IHttpActionResult getAllEventos()
        {
            return Ok(new Evento().GetAllEventos());
        }

        [Route("api/getAllEventosIdNome")]
        public IHttpActionResult getAllEventosIdNome()
        {
            return Ok(new Evento().GetAllEventosIdNome());
        }

        [Route("api/getCustoWorkshop/{id}")]
        public IHttpActionResult getCustoWorkshop([FromUriAttribute]int id)
        {
            return Ok(new Workshop().GetCustoWorkshop(id));
        }
        [Route("api/getDadosEvento/{id}")]
        public IHttpActionResult GetDadosEvento(int id)
        {
            return Ok(new Evento().GetEvento(id));
        }


        [Route("api/getAllEventosOfTipo/{tipoEvento}")]
       public IHttpActionResult GetEventosTipo(string tipoEvento)
        {
            return Ok(new Evento().GetAllEventosTipo(tipoEvento));
        }

        [Route("api/getAllEventosOfTipoIdNome/{tipoEvento}")]
        public IHttpActionResult GetEventosTipoIdNome(string tipoEvento)
        {
            return Ok(new Evento().GetAllEventosTipoIdNome(tipoEvento));
        }

        [Route("api/getEventosParticipante/{id}")]
        public IHttpActionResult GetEventosParticipante(int id)
        {
            if(id <= 0)
            {
                return NotFound();
            }

            return Ok(ParticipacaoComEvento.GetPartipacaoComEvento(id));
        }

        [Route("api/getEventosOrador/{id}")]
        public IHttpActionResult GetEventosOrador(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(Evento.GetEventosOrador(id));
        }

        [Route("api/getPessoasNaoEmEvento/{id}")]
        public IHttpActionResult GetPessoasNaoEvento(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(Evento.GetPessoasNaoEvento(id));

        }

        [Route("api/getDadosParticipantesEvento/{id}")]
        public IHttpActionResult GetDadosParticipanteEvento(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(Participacao.GetParticipacoesEvento(id));

        }

        [Route("api/getAllPalestrasSimposio/{id}")]
        public IHttpActionResult GetAllPalestrasSimposio(int id)
        {
            if(id <= 0)
            {
                return NotFound();
            }

            return Ok(Palestra.GetAllPalestrasSimposio(id));
        }


        [Route("api/getLinkRegistoEvento/{id}")]
        public IHttpActionResult GetLinkRegistoEvento(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            string result = Evento.GetLinkRegistoEvento(id);

            if(result == null)
            {
                return NotFound();
            }


            return Ok(result);
        }
        
        [Route("api/getInscricoesPagamentoEvento/{id}")]
        public IHttpActionResult GetInscricoesPagamentosEvento(int id)
        {
            return Ok(Evento.GetInscricoesPagamentosEvento(id));
        }



        /*POSTS*/

        [Route("api/postPalestraSimposio")]
        public IHttpActionResult PostPalestraSimposio([FromBody]Palestra palestra)
        {
            bool check = Palestra.CreatePalestraSimposio(palestra);

            if (check)
            {
                return Created("Palestra",palestra);
            }
            return BadRequest();
        }


        [Route("api/postPagamentoEvento")]
        public IHttpActionResult PostPagamentoEvento([FromBody]JObject pagamento)
        {
            long result = Palestra.CreatePagamentoEvento(pagamento);
            if (result < 0)
            {
                return BadRequest();
                
            }
            return Created("Pagamento",result);
        }

        [Route("api/postEvento")]
        public IHttpActionResult postEvento([FromBody]Evento evento)
        {
            if (!Evento.CreateEvento(evento))
            {
                return BadRequest("Erro ao criar Evento!");
            }
            return Created("Evento",evento);
        }


        [Route("api/postWorkshop")]
        public IHttpActionResult postWorkshop([FromBody]Workshop workshop)
        {
            if (!Workshop.CreateWorkshop(workshop))
            {
                return BadRequest("Erro ao criar Workshop!");
            }
            return Created("Workshop",workshop);
        }


        /*PUTS*/
        [Route("api/editPagamentoEvento/{idEvento}/{idParticipante}")]
        public IHttpActionResult PutPagamentoEvento(int idEvento, int idParticipante,[FromBody]JObject pagamento)
        {
            int result = Palestra.UpdatePagamentoEvento(idEvento, idParticipante,pagamento);

            if (result < 0)
            {
                return BadRequest();

            }else if(result == 0)
            {
                return NotFound();
            }

            return Ok();
        }




        [Route("api/editEvento/{id}")]
        public IHttpActionResult putEvento([FromBody]Evento evento,int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            int status = Evento.EditEvento(id, evento);

            if (status == -1)
            {
                return BadRequest("Erro ao editar Evento!");
            }
            else if(status == 0)
            {
                return NotFound();
            }
            return Ok();
        }

        /*DELETES*/
        [Route("api/deletePagamentoEvento/{idEvento}/{idParticipante}")]
        public IHttpActionResult deletePagamentoEvento(int idEvento, int idParticipante)
        {
            int result = Evento.DeletePagamentoEvento(idEvento, idParticipante);

            if ( result < 0)
            {
                return BadRequest();
            }
            else if(result == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
