using API.Models;
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
    public class RelatorioController : ApiController
    {

        [Route("api/getRelatorioContagemEventos")]
        public IHttpActionResult GetRelatorioContagemEventos()
        {
            return Ok(Relatorio.GetRelatorioContagemEventos());
        }

        [Route("api/getRelatorioPessoasPorApoiosFinanceiros")]
        public IHttpActionResult GetRelatorioPessoasPorApoiosFinanceiros()
        {
            return Ok(Relatorio.GetRelatorioTopDonativos());
        }

        [Route("api/getRelatorioStaffApoios")]
        public IHttpActionResult GetRelatoriosStaffApoios()
        {
            return Ok(Relatorio.GetRelatorioStaffApoios());
        }

        [Route("api/getRelatorioOradoresPorNumPalestras")]
        public IHttpActionResult GetRelatorioOradoresPorNumPalestras()
        {
            return Ok(Relatorio.GetRelatorioOradoresPorNumPalestras());
        }

        [Route("api/getRelatorioOradoresParticipacoes")]
        public IHttpActionResult GetRelatorioOradoresParticipacoes()
        {
            return Ok(Relatorio.GetRelatorioOradoresParticipacoes());
        }


        [Route("api/getRelatorioOradoresParticipacoesEventosEspeciais")]
        public IHttpActionResult GetRelatorioOradoresParticipacoesEventosEspeciais()
        {
            return Ok(Relatorio.GetRelatorioOradoresParticipacoesEventoEspeciais());
        }


        [Route("api/getRelatorioOradoresApoiosFinanceiros")]
        public IHttpActionResult GetRelatorioOradoresApoiosFinanceiros()
        {
            return Ok(Relatorio.GetRelatorioOradoresApoiosFinanceiros());
        }


        [Route("api/getRelatorioEmpresariosApoiosFinanceirosPessoal")]
        public IHttpActionResult GetRelatorioEmpresariosApoiosFinanceirosPessoal()
        {
            return Ok(Relatorio.GetRelatorioEmpresariosApoiosFinanceirosPessoal());
        }

        [Route("api/getRelatorioEmpresariosApoiosFinanceirosEmpresa")]
        public IHttpActionResult GetRelatorioEmpresariosApoiosFinanceirosEmpresa()
        {
            return Ok(Relatorio.GetRelatorioEmpresariosApoiosFinanceirosEmpresa());
        }

        [Route("api/getRelatorioStaffApoiosEspeciais")]
        public IHttpActionResult GetRelatorioStaffApoiosEspeciais()
        {
            return Ok(Relatorio.GetRelatorioStaffApoiosEspeciais());
        }

        [Route("api/getRelatorioEventosPorParticipacoes")]
        public IHttpActionResult GetRelatorioEventosPorParticipacoes()
        {
            return Ok(Relatorio.GetRelatorioEventosPorParticipacoes());
        }

        [Route("api/getRelatorioPessoasPorPresencas")]
        public IHttpActionResult GetRelatoriosPessoasPorPresencas()
        {
            return Ok(Relatorio.GetRelatoriosPessoasPorPresencas());
        } 
        
        [Route("api/getRelatorioStaffApoiosEsp")]
        public IHttpActionResult GetRelatoriosStaffPorApoio()
        {
            return Ok(Relatorio.GetRelatoriosStaffPorApoiosEspeciais());
        }

        [Route("api/getRelatorioOradoresPorPublicoSeu")]
        public IHttpActionResult GetRelatoriosPorApoioSeu()
        { 
            return Ok(Relatorio.GetRelatorioOradoresPublicoSeu());
        }

        [Route("api/getRelatorioParticipacoesPorEvento")]
        public IHttpActionResult GetRelatorioParticipacoesPorEvento()
        {
            return Ok(Relatorio.GetRelatorioEventoPresencas());
        }
    }
}
