using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Inquerito
{
    public class Questao
    {
        public int id { get; set; }
        public string tipoVar { get; set; }
        public string questao { get; set; }
        public ValoresIntervalos[] intervalo { get; set; }

        public static Questao FromDB(MySqlDataReader reader)
        {
            int id = (int)reader["idApoio"];
            string tipoVar = reader["tipoVar"].ToString();
            string questao = reader["questao"].ToString();
            return new Questao
            {
                id=id,
                tipoVar=tipoVar,
                questao=questao
            };
        }
        public static long[] GetIdsQuestoesPorInquerito(long idI)
        {
            List<long> lista = new List<long>();


            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT idQuestao FROM inquerito_questao where idInquerito=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idI);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(long.Parse((reader["idQuestao"].ToString())));
                    }
                }
                conn.Close();
            }

            return lista.ToArray();
        }
        public static long CriarQuestao(Questao questao)
        {
            long idLast = -1;
            //Depois de os intervalos estarem feitos(ou se não for intervalo)
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO questao VALUES(@id,@tipoVar,@questao)", conn))
                    {
                       

                        sqlCommand.Parameters.AddWithValue("@id", DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@tipoVar", questao.tipoVar);
                        sqlCommand.Parameters.AddWithValue("@questao", questao.questao);
                        sqlCommand.ExecuteNonQuery();
                        idLast = sqlCommand.LastInsertedId;
                    }
                    conn.Close();
                }
                return idLast;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Questao: " + e.Message);
                return idLast;
            }
        }
        public static Questao CreateFromJobj(JToken json)
        {
            Questao questao = new Questao
            {
                tipoVar = json["tipoVar"].ToString(),
                questao = json["questao"].ToString()
            };
            Debug.Write(questao.tipoVar+ ' ' +questao.id);
            if (questao.tipoVar.ToUpper()=="INTERVALO")
            {
                List<ValoresIntervalos> intervaloAux = new List<ValoresIntervalos>();
                foreach(var valor in json["intervalo"])
                {

                    intervaloAux.Add(new ValoresIntervalos(-1,valor.ToString()));
                }

                questao.intervalo = intervaloAux.ToArray();
            }


            return questao;
        }


    }
}
