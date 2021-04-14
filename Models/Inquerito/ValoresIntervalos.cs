using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Inquerito
{
    public class ValoresIntervalos
    {

        public int id { get; set; }
        public string valor { get; set; }

        public ValoresIntervalos(int id, string valor)
        {
            this.id = id;
            this.valor = valor;
        }

        public static JArray GetAllValoresQuestao(int idQuestao)
        {
            JArray VI = new JArray();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select valor from intervalo_valor i join questao_valor q on i.idValor = q.idValor where q.idQuestao=@id; ", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idQuestao);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        VI.Add(reader["valor"].ToString());
                    }
                    conn.Close();
                }
            }
            return VI;
        }

        public static List<ValoresIntervalos> GetAllValores()
        {
            List<ValoresIntervalos> VI = new List<ValoresIntervalos>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM intervalo_valor ", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        VI.Add(ValoresIntervalos.FromDB(reader));
                    }
                    conn.Close();
                }
            }
            return VI;
        }
        public static long CriarValor(string valor)
        {
            long idLast = -1;
            //Depois de os intervalos estarem feitos(ou se não for intervalo)
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO intervalo_valor VALUES(@id,@valor)", conn))
                    {

                        sqlCommand.Parameters.AddWithValue("@id", DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@valor", valor);

                        sqlCommand.ExecuteNonQuery();
                        idLast = sqlCommand.LastInsertedId;
                    }
                    conn.Close();
                }
                return idLast;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Valor: " + e.Message);
                return idLast;
            }
        }

         public static bool MapIntervaloQuestao(long idValor,long idQuestao)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO questao_valor VALUES(@idValor,@idQuestao)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idValor", idValor);
                        sqlCommand.Parameters.AddWithValue("@idQUestao", idQuestao);
                        sqlCommand.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {

                Debug.WriteLine("F: " + e.Message);
                return false;
            }

        }
        public static ValoresIntervalos FromDB(MySqlDataReader reader)
        {
            int id = (int)reader["idValor"];
            string valor = reader["valor"].ToString();


            ValoresIntervalos VI = new ValoresIntervalos(id, valor);
            

            return VI;
        }
    }
}