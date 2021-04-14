
﻿
using API.Filters;
using API.Models.Pessoas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
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
    public class PessoaController : ApiController
    {
        [Route("api/getAllPessoas")]
        public HttpResponseMessage GetAllPessoas()
        {
            List<Pessoa> pessoas = new Pessoa().GetAllPessoas();

            return Request.CreateResponse(HttpStatusCode.OK, pessoas);
        }

        [Route("api/getAllPessoasIdNome")]
        public HttpResponseMessage GetAllPessoasIdNome()
        {
            List<JObject> pessoas = new Pessoa().GetAllPessoasIdNome();

            return Request.CreateResponse(HttpStatusCode.OK, pessoas);
        }

        [Route("api/getAllParticipantes")]
        public IHttpActionResult GetAllParticipantes()
        {
            return Ok(Participante.GetAllParticipantes());
        }


        [Route("api/getDadosPessoa/{id}")]
        public HttpResponseMessage GetDadosPessoas([FromUri]int id)
        {
            Pessoa pessoa =  Pessoa.GetPessoa(id);

            if(pessoa == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, pessoa);
        }





        [Route("api/getPessoasTrouxe/{id}")]
        public IHttpActionResult GetPessoasTrouxe(int id)
        {

            if (id <= 0)
            {
                return NotFound();
            }

            int nrPessoas = Pessoa.GetPessoaQueTrouxe(id);

            if (nrPessoas == -1)
            {
                return NotFound();
            }


            return Ok(nrPessoas);
        }
        [Route("api/getAllPessoasNaoOradores")]
        public IHttpActionResult GetAllNaoOradores()
        {
            return Ok(new Pessoa().GetAllNaoOradores());
        }


        [Route("api/getDadosPrimeiroEventoPessoa/{id}")]
        public IHttpActionResult GetDadosPrimeiroEvento(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            JObject json = Pessoa.GetPrimeiroEvento(id);

            if (json == null)
            {
                return NotFound();
            }

            return Ok(json);
        }

        [Route("api/getAllEmpresarios")]
        public IHttpActionResult GetAllEmpresarios()
        {
            return Ok(Empresario.GetAll());
        }


        [Route("api/getAllPessoasNaoStaff")]
        public IHttpActionResult GetAllPessoasNotStaff()
        {
            return Ok(Pessoa.GetAllPessoasNotStaff());
        }
        [Route("api/postEmpresario")]
        public IHttpActionResult PostEmpresario([FromBody]Empresario empresario)
        {
            bool check = Empresario.CreateEmpresario(empresario);
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }


        [Route("api/getDadosEmpresario/{id}")]
        public IHttpActionResult GetDadosEmpresario(int id)
        {

            if (id <= 0)
            {
                return NotFound();
            }

            Empresario empresario = Empresario.GetDados(id);

            if (empresario == null)
            {
                return NotFound();
            }

            return Ok(empresario);
        }
        [Route("api/editPessoa/{id}")]
        public IHttpActionResult PutPessoa([FromUri]int id,[FromBody]Pessoa pessoa)
        {

            if (id <= 0)
            {
                return NotFound();
            }

            if (pessoa == null)
            {
                return NotFound();
            }

            bool check = Pessoa.EditPessoa(pessoa,id);

            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }

        /*POSTS*/
        [Route("api/postPessoa")]
        public IHttpActionResult POST([FromBody]Pessoa pessoa)
        {
            long id = new Pessoa().CreatePessoa(pessoa);
            if (id == -1)
            {
                return BadRequest();
            }
            else if (id == -2)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            return Created("Pessoa", id);
        }


    }
}

