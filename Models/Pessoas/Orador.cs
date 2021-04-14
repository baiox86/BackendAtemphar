using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Pessoas
{
    public class Orador : Pessoa
    {
        public int idOrador { get; set; }
        public string especialidade { get; set; }
        public int totalEventosApresentados { get; set; }
        public string dataPrimeiroEventoApresentado { get; set; }
        public int totalWorkshopsApresentados { get; set; }
        public int totalPalestrasApresentadasAno { get; set; }



        public List<Orador> GetAllOradores()
        {
            List<Orador> oradores = new List<Orador>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM orador_view", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Orador orador = new Orador
                        {
                            id = (int)reader["idOrador"],
                            idOrador = (int)reader["idOrador"],
                            telefone = (string)reader["telefone"],
                            nome = (string)reader["nome"],
                            email = (string)reader["email"],
                            especialidade = (string)reader["especialidade"]

                        };

                        oradores.Add(orador);
                    }
                    conn.Close();
                }
            }
            return oradores;
        }

        public static string GetEspecialidadeOrador(int id)
        {
            string resultado;
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT especialidade FROM orador where idOrador = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    if (reader["especialidade"]==null)
                    {
                        return null;
                    }
                    resultado = (string)reader["especialidade"];
                    conn.Close();
                }
                return resultado;
            }
            }
        /*Se o id pertencer a um orador que não existe ou que não tenha apresentado, devolve 0*/
        public static long GetTotalEventosNormaisApresentados(int id)
        {
            long participacoesNormal;
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select totalEventosApresentados from relatorio_num_eventos_orador_view where idOrador=@id", conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    if (reader["totalEventosApresentados"]==null)
                    {
                        return 0;
                    }
                    participacoesNormal = (long)reader["totalEventosApresentados"];
                    
                    conn.Close();
                }
                
            }
            return participacoesNormal;
        }
        public static bool CreateOrador(Orador orador)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                  {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO orador VALUES(@idOrador,@especialidade)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idOrador", orador.idOrador);
                        sqlCommand.Parameters.AddWithValue("@especialidade", orador.especialidade); 
                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Orador: " + e.Message);
                return false;
            }
        }

    }
}