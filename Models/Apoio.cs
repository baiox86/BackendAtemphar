using API.Models.Pessoas;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Apoio
    {
        public int idApoio { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }
        public string descricao { get; set; }
        public int pontos { get; set; }
        public int idStaff { get; set; }
        public int idEvento { get; set; }



        public static Apoio FromDB(MySqlDataReader reader)
        {
            int idApoio = (int)reader["idApoio"];
            string dataInicio = reader["dataInicio"].ToString();
            string dataFim = reader["dataFim"].ToString();
            string descricao = reader["descricao"].ToString();
            int pontos = int.Parse((string)reader["pontos"].ToString());
            int idStaff = (int)reader["idStaff"];
            int idEvento = (int)reader["idEvento"];

            return new Apoio()
            {
                idApoio = idApoio,
                dataInicio = dataInicio,
                dataFim = dataFim,
                descricao = descricao,
                pontos = pontos,
                idStaff = idStaff,
                idEvento = idEvento

            };
        }
        
      public static int GetTotalPontosStaff(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT sum(pontos) as pontos FROM apoio where idEvento=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    if (reader["pontos"]==null)
                    {
                        return 0;
                    }

                    return int.Parse((string)reader["pontos"].ToString()); 
                }
            }
        }

        public static List<Apoio> GetApoiosEvento(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM apoio where idEvento=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Apoio> apoios = new List<Apoio>();

                    while (reader.Read())
                    {
                        apoios.Add(Apoio.FromDB(reader));
                    }

                    return apoios;
                }
            }
        }
        public static List<JObject> GetApoiosByStaff(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select a.*, e.nomeEvento, e.tipoEvento from apoio a join evento e on e.idEvento = a.idEvento WHERE a.idStaff =@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<JObject> apoios = new List<JObject>();
                    dynamic json = new JObject();

                    while (reader.Read())
                    {
                        json.idApoio = reader.GetInt32("idApoio");
                        json.idStaff = reader.GetInt32("idStaff");
                        json.idEvento = reader.GetInt32("idEvento");
                        json.descricao = reader.GetString("descricao");
                        json.dataInicio = reader.GetString("dataInicio");
                        json.dataFim = reader.GetString("dataFim");
                        json.pontos = reader.GetInt32("pontos");
                        json.nomeEvento = reader.GetString("nomeEvento");
                        json.tipoEvento = reader.GetString("tipoEvento");

                        apoios.Add(json);
                    }

                    return apoios;
                }
            }
        }
        public static bool CreateApoio(Apoio apoio)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO apoio VALUES(@id,@idStaff,@idEvento,@descricao,@dataInicio,@dataFim,@pontos)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@idStaff", apoio.idStaff);
                        sqlCommand.Parameters.AddWithValue("@idEvento", apoio.idEvento);
                        sqlCommand.Parameters.AddWithValue("@descricao", apoio.descricao);
                        sqlCommand.Parameters.AddWithValue("@dataInicio", apoio.dataInicio);
                        sqlCommand.Parameters.AddWithValue("@dataFim", apoio.dataFim);
                        sqlCommand.Parameters.AddWithValue("@pontos",apoio.pontos);
                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Apoio: " + e.Message);
                return false;
            }

        }


        public static bool EditApoio(int id,Apoio apoio)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("UPDATE `apoio` SET idStaff = @idStaff, idEvento = @idEvento, descricao =  @descricao, dataInicio = @dataInicio, dataFim = @dataFim, pontos= @pontos WHERE idApoio = @idApoio", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idApoio", id);
                        sqlCommand.Parameters.AddWithValue("@idStaff", apoio.idStaff);
                        sqlCommand.Parameters.AddWithValue("@idEvento", apoio.idEvento);
                        sqlCommand.Parameters.AddWithValue("@descricao", apoio.descricao);
                        sqlCommand.Parameters.AddWithValue("@dataInicio", apoio.dataInicio);
                        sqlCommand.Parameters.AddWithValue("@dataFim", apoio.dataFim);
                        sqlCommand.Parameters.AddWithValue("@pontos", apoio.pontos);


                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao editar Apoio: " + e.Message);
                return false;
            }
        }
    }
}