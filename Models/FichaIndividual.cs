using API.Models.Donativos;
using API.Models.Pessoas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class FichaIndividual
    {
        /*FICHA INDIVIDUAL PESSOA REGULAR*/
        public static JObject GetFichaPessoa(int id)
        {
            try
            {
                dynamic json = new JObject();
                json.pessoa = JObject.FromObject(Pessoa.GetPessoa(id));
                json.pessoasTrouxe = Pessoa.GetPessoaQueTrouxe(id);
                json.totalDoado = DonativoPessoa.GetTotalDonativosPessoa(id);
                json.numParticipacoes = Participacao.GetNumParticipacoes(id);
                return json;
            }
            catch (Exception e)
            {
                Debug.Write("F: "+e.Message);
                return null;
            }
        }
        public static JObject GetFichaStaff(int id)
        {
            try
            {
                dynamic json = new JObject();
                
                List<JObject> apoios = Apoio.GetApoiosByStaff(id);
                json["apoios"] = JToken.FromObject(apoios);
                json.totalPontos = Apoio.GetTotalPontosStaff(id);
                return json;
            }
            catch (Exception e)
            {
                Debug.Write("F: " + e.Message);
                return null;
            }
        }
        public static JObject GetFichaOrador(int id)
        {
            try
            {
                dynamic json = new JObject();
                string especialidade = Orador.GetEspecialidadeOrador(id);

                if (especialidade==null)
                {
                    return null;
                }
                json.especialidade = especialidade;
                json.eventosNormaisApresentados = Orador.GetTotalEventosNormaisApresentados(id);
               
                return json;
            }
            catch (Exception e)
            {
                Debug.Write("F: " + e.Message);
                return null;
            }
        }

    }

}