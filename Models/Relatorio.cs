using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Relatorio
    {
        public static List<JObject> GetRelatorioContagemEventos()
        {

            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_num_eventos_tipo_view", conn))
                {


                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> contagens = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.tipoEvento = reader["tipoEvento"].ToString();
                        json.totalTipoEventos = int.Parse(reader["totalTipoEventos"].ToString());
                        contagens.Add(json);
                    }

                    return contagens;
                }
            }
        }

        public static List<JObject> GetRelatorioTopDonativos()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_top_donativos_view LIMIT 5", conn))
                {


                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.idPessoa =(int) reader["idPessoa"];
                        json.nome = reader["nome"].ToString();
                        json.totalDonativos = (decimal)reader["totalDonativos"];
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }

        public static List<JObject> GetRelatorioStaffApoios()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_top_apoios_staff_view LIMIT 5", conn))
                {


                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nome = reader["nome"].ToString();
                        json.idStaff = (int)reader["idStaff"];
                        json.totalApoios = int.Parse(reader["totalApoios"].ToString());
                        json.totalPontuacao = (decimal)reader["totalPontuacao"];
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }
        

        public static List<JObject> GetRelatorioOradoresPorNumPalestras()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_num_eventos_orador_view LIMIT 5", conn))
                {


                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nome = reader["nome"].ToString();
                        json.totalEventosApresentados = int.Parse(reader["totalEventosApresentados"].ToString());
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }
        public static List<JObject> GetRelatorioOradoresParticipacoes()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_oradores_participacoes_view LIMIT 5", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nome = reader["nome"].ToString();
                        json.totalParticipacoesOutrosEventos = int.Parse(reader["totalParticipacoesOutrosEventos"].ToString());
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }

        public static List<JObject> GetRelatorioOradoresParticipacoesEventoEspeciais()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_oradores_participacoes_eventos_especiais_view LIMIT 5", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nome = reader["nome"].ToString();
                        json.totalParticipacoesEventosEspeciais = int.Parse(reader["totalParticipacoesEventosEspeciais"].ToString());
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }

        public static List<JObject> GetRelatorioOradoresApoiosFinanceiros()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_oradores_donativos_view LIMIT 5", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nome = reader["nome"].ToString();
                        json.totalDonativos = (decimal)reader["totalDonativos"];
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }

        public static List<JObject> GetRelatorioEmpresariosApoiosFinanceirosPessoal()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from top_donativos_empresarios_pessoal_view LIMIT 5", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nome = reader["nome"].ToString();
                        json.totalDonativos = int.Parse(reader["totalDonativos"].ToString());
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }

        public static List<JObject> GetRelatorioEmpresariosApoiosFinanceirosEmpresa()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from top_donativos_empresarios_empresa_view LIMIT 5", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nome = reader["nome"].ToString();
                        json.nomeEmpresa = reader["nomeEmpresa"].ToString();
                        json.totalDonativos = int.Parse(reader["totalDonativos"].ToString());
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }

        public static List<JObject> GetRelatorioStaffApoiosEspeciais()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_top_apoios_staff_especiais_view", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nome = reader["nome"].ToString();
                        json.idStaff = (int)reader["idStaff"];
                        json.totalApoios = int.Parse(reader["totalApoios"].ToString());
                        json.totalPontuacao = int.Parse(reader["totalPontuacao"].ToString());
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }

        public static List<JObject> GetRelatorioEventosPorParticipacoes()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from relatorio_evento_by_participacoes_view", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> tops = new List<JObject>();
                    dynamic json;


                    while (reader.Read())
                    {
                        json = new JObject();
                        json.nomeEvento = reader["nomeEvento"].ToString();
                        json.numParticipantes = int.Parse(reader["numParticipantes"].ToString());
                        tops.Add(json);
                    }

                    return tops;
                }
            }
        }

        public static List<JObject> GetRelatoriosPessoasPorPresencas()
        {
            List<JObject> lista = new List<JObject>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select * from relatorio_top_participacoes_view ", conn))
                {


                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        int idParticipante = (int)reader["idParticipante"];
                        string nome = (string)reader["nome"];
                        long totalParts = (long)reader["totalEventos"];

                        dynamic json = new JObject();

                        json.idPessoa = idParticipante;
                        json.nome = nome;
                        json.totalEventos = totalParts;
                        lista.Add(json);


                    }


                    return lista;
                    }
                }
            }
        
    public static List<JObject> GetRelatoriosStaffPorApoiosEspeciais()
        {
            List<JObject> lista = new List<JObject>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
        {
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand("select * from relatorio_top_apoios_staff_especiais_view ", conn))
            {


                MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {



                        int idStaff = (int)reader["idStaff"];
                        string nome = (string)reader["nome"];
                        long totalApoios = (long)reader["totalApoios"];
                        Decimal totalPontuacao = (decimal)reader["totalPontuacao"];

                        dynamic json = new JObject();

                        json.idStaff = idStaff;
                        json.nome = nome;
                        json.totalApoios = totalApoios;
                        json.totalPontuacao = totalPontuacao;
                        lista.Add(json);
                    }
                return lista;
            }
        }
    }
        public static List<JObject> GetRelatorioOradoresPublicoSeu()
        {
            List<JObject> lista = new List<JObject>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select * from relatorio_oradores_referrals_palestras_view ", conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int idOrador = (int)reader["idOrador"];
                        string nomeOrador = (string)reader["nomeOrador"];
                        int totalRefs = int.Parse(reader["totalReferralsEmEventos"].ToString());

                        dynamic json = new JObject();

                        json.idOrador = idOrador;
                        json.nomeOrador = nomeOrador;
                        json.totalReferalsEmEventos =totalRefs;
                        lista.Add(json);
                    }
                    return lista;
                }
            }
        }
        public static List<JObject> GetRelatorioEventoPresencas()
        {
            List<JObject> lista = new List<JObject>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select * from relatorio_evento_by_participacoes_view", conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int numParticipantes = int.Parse(reader["numParticipantes"].ToString());
                        string nomeEvento = (string)reader["nomeEvento"];
                     

                        dynamic json = new JObject();
                        json.nomeEvento = nomeEvento;
                        json.numParticipantes = numParticipantes;
                        lista.Add(json);
                    }
                    return lista;
                }
            }
        }
    }   

}
