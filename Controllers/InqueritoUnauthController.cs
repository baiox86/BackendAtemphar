using System.Web.Http;
using API.Filters;
using API.Models.Inquerito;

namespace API.Controllers
{
    public class InqueritoUnauthController : ApiController
    {
        int i = 1;
        // GET: InqueritoUnauth
        [Route("api/getInqueritoToken/{token}")]
        public IHttpActionResult GetInqueritoFromToken([FromUri]string token)
        {
            return Ok(Inquerito.GetInqueritoFromToken(token));
        }



        [Route("api/postResposta/{token}")]
        public IHttpActionResult PostResposta([FromUri]string token,[FromBody]string[] respostas)
        {
            Resposta.parseRespostas(respostas,token,i);
            i++;
            return Created("Inquerito",respostas);

        }
    }
}