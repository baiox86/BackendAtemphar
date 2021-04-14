using API.Filters;
using API.Models;
using API.Models.Donativos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [JwtAuthentication]
    public class DonativoController : ApiController
    {

        [Route("api/getDonativosEmpresa/{id}")]
        public IHttpActionResult GetDonativosEmpresa(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(DonativoEmpresa.GetDonativosEmpresa(id));
        }
        [Route("api/getDonativosPessoa/{id}")]
        public IHttpActionResult GetDonativosPessoa(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            return Ok(DonativoPessoa.GetDonativosPessoa(id));
        }
        [Route("api/postDonativoPessoa")]
        public IHttpActionResult PostDonativo([FromBody] DonativoPessoa donativo)
        {
            bool check = DonativoPessoa.CreateDonativo(donativo);
            if (check)
            {
                return Created("Donativo",donativo);
            }
            return BadRequest();
        }


        [Route("api/postDonativoEmpresa")]
        public IHttpActionResult postDonativoEmpresa([FromBody]DonativoEmpresa donativo)
        {
            if (!DonativoEmpresa.CreateDonativoEmpresa(donativo))
            {
                return BadRequest("Erro ao criar Donativo de Empresa!");
            }
            return Created("Donativo Empresa",donativo);
        }

    }
}