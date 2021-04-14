using API.Models;
using API.Models.Pessoas;
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
    public class EmpresaController : ApiController
    {
        /*GETS*/

        [Route("api/getAllEmpresas")]
        public IHttpActionResult GetAll()
        {
            return Ok(Empresa.GetAll());
        }

        [Route("api/getDadosEmpresa/{id}")]
        public IHttpActionResult GetDadosEmpresa(int id)
        {
            if(id <= 0)
            {
                return NotFound();
            }

            Empresa empresa = Empresa.GetDados(id);

            if(empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }

        [Route("api/getDadosEmpresariosEmpresa/{id}")]
        public IHttpActionResult GetDadosEmpresario(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            JObject empresario = Empresa.GetDadosEmpresario(id);

            if(empresario == null)
            {
                return NotFound();
            }

            return Ok(empresario);
        }

        [Route("api/getPessoasNaoEmEmpresa/{id}")]
        public IHttpActionResult GetPessoasNaoEmEmpresa(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(Empresa.GetPessoasNaoEmEmpresa(id));
        }

        /*POSTS*/

        [Route("api/postEmpresa")]
        public IHttpActionResult postEmpresa([FromBody]Empresa empresa)
        {
            if (!Empresa.CreateEmpresa(empresa))
            {
                return BadRequest("Erro ao criar Empresa!");
            }
            return Created("Empresa", empresa);
        }
    }
}
