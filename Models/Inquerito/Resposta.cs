using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Inquerito
{
    public class Resposta
    {
        public int id  {get;set; }
        public int idQuestao { get; set; }
        public int idInquerito { get; set; }
        public string valorIntroduzido { get; set; }
        public int idResposta { get; set; }

        public static void parseRespostas(string[] respostas, string token,int id)
        {
            string valor = "";
            JObject dados = Inquerito.GetIDInqueritoFromToken(token);
            long[] lista = Questao.GetIdsQuestoesPorInquerito((long)dados["idInquerito"]);
          
            for (int i = 0; i < respostas.Length; i++)
            {
                valor = respostas[i];
                registarResposta(lista[i], (long)dados["idInquerito"], (long)dados["idEvento"], valor, id);
            }
        }

        public static Resposta FromDB(MySqlDataReader reader)
        {
            int id = reader.GetInt32("id");
            int idQuestao = reader.GetInt32("idQuestao");
            int idInquerito = reader.GetInt32("idInquerito");
            string valorIntroduzido = reader.GetString("valorIntroduzido");
            int idResposta = reader.GetInt32("idResposta");


            return new Resposta()
            {
                id = id,
                idQuestao = idQuestao,
                idInquerito = idInquerito,
                valorIntroduzido = valorIntroduzido,
                idResposta = idResposta
            };


        }


        public static List<JObject> GetAllRespostasInqueritoQuestao(int idInquerito, int idEvento, int idQuestao)
        {
            try
            {
                List<JObject> respostas = new List<JObject>();
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("SELECT idInquerito, idEvento, idQuestao, questao, resposta FROM resposta_questao_inquerito WHERE idInquerito = @idInquerito AND idEvento = @idEvento AND idQuestao = @idQuestao", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idInquerito", idInquerito);
                        sqlCommand.Parameters.AddWithValue("@idEvento", idEvento);
                        sqlCommand.Parameters.AddWithValue("@idQuestao", idQuestao);

                        MySqlDataReader reader = sqlCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            dynamic json = new JObject();
                            json.idInquerito = reader.GetInt32("idInquerito");
                            json.idEvento = reader.GetInt32("idEvento");
                            json.idQuestao = reader.GetInt32("idQuestao");
                            json.questao = reader.GetString("questao");
                            json.resposta = reader.GetString("resposta");

                            respostas.Add(json);
                        }

                    }


                    conn.Close();
                }
                return respostas;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao obter respostas por inquerito e questao: " + e.Message);
                return null;
            }
        }

        public static List<JObject> GetAllRespostasInquerito(int idInquerito, int idEvento)
        {
            try
            {
                List<JObject> respostas = new List<JObject>();
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("SELECT idInquerito, idEvento, idQuestao,questao,resposta FROM resposta_questao_inquerito WHERE idInquerito = @idInquerito AND idEvento = @idEvento ", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idInquerito", idInquerito);
                        sqlCommand.Parameters.AddWithValue("@idEvento", idEvento);

                        MySqlDataReader reader = sqlCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            dynamic json = new JObject();
                            json.idInquerito = reader.GetInt32("idInquerito");
                            json.idEvento = reader.GetInt32("idEvento");
                            json.idQuestao = reader.GetInt32("idQuestao");
                            json.questao = reader.GetString("questao");
                            json.resposta = reader.GetString("resposta");

                            respostas.Add(json);
                        }

                    }


                    conn.Close();
                }
                return respostas;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao obter respostas por inquerito: " + e.Message);
                return null;
            }
        }

        public static long registarResposta(long idQuestao,long idInquerito,long idEvento,string valor,int idResposta) {
            long idLast = -1;
                try
                {
                
                    using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                    {
                        conn.Open();
                        using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO resposta VALUES(@id,@idQ,@idI,@idE,@idR,@valor)", conn))
                        {
                        sqlCommand.Parameters.AddWithValue("@id",DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@idQ",idQuestao);
                        sqlCommand.Parameters.AddWithValue("@idI",idInquerito);
                        sqlCommand.Parameters.AddWithValue("@idR",idResposta);
                        sqlCommand.Parameters.AddWithValue("@idE", idEvento);
                        sqlCommand.Parameters.AddWithValue("@valor",valor);
                        sqlCommand.ExecuteNonQuery();

                        idLast = sqlCommand.LastInsertedId;

                        }


                        conn.Close();
                    }
                    return idLast;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Erro ao criar resposta: " + e.Message);
                    return idLast;
                }
        }
    }
}