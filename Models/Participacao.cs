using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace API.Models
{
    public class Participacao
    {
        public int idParticipante { get; set; }
        public int idEvento { get; set; }
        public bool flagPrimeiroEvento { get; set; }
        public bool flagCompareceu { get; set; }

        public string nome { get; set; }


        public static List<Participacao> GetParticipacoesEvento(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM participacoes_evento_view where idEvento =@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Participacao> participacoes = new List<Participacao>();

                    while (reader.Read())
                    {
                        participacoes.Add(Participacao.FromDB(reader));
                    }

                    return participacoes;

                }
            }
        }


        public static List<Participacao> GetParticipacoes(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM participacoes_evento_view where idPessoa=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Participacao> participacoes = new List<Participacao>();

                    while (reader.Read())
                    {
                        participacoes.Add(Participacao.FromDB(reader));
                    }

                    return participacoes;

                }
            }
        }
        public static long GetNumParticipacoes(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT count(*) as numParticipacoes FROM participacoes_evento_view where idPessoa=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    if (reader["numParticipacoes"] == null)
                    {
                        return 0;
                    }
                    return (long)reader["numParticipacoes"];

                }
            }
        }
        public static List<Participacao> GetParticipacoesEmEventos()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM participacoes_evento_view", conn))
                { 
                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Participacao> participacoes = new List<Participacao>();

                    while (reader.Read())
                    {
                        participacoes.Add(Participacao.FromDB(reader));
                    }

                    return participacoes;

                }
            }
        }



        public static bool DeleteParticipacao(int idE,int idP)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Delete from registoevento where idParticipante=@idP AND idEvento=@idE", conn))
                {
                    cmd.Parameters.AddWithValue("@idP", idP);
                    cmd.Parameters.AddWithValue("@idE", idE);
                    try
                    {
                        int check = cmd.ExecuteNonQuery();
                        Debug.Write("VALOR DO CHECK: "+check);
                        if (check == 0)
                        {
                            return false;
                        }
                        return true;

                    }
                    catch (Exception)
                    {

                        return false;
                    }
              

                }
            }

        }


        public static Participacao FromDB(MySqlDataReader reader)
        {
           
            int idParticipante = reader.GetInt32("idParticipante");
            int idEvento = reader.GetInt32("idEvento");
            bool flagPrimeiroEvento = (bool)reader["flagPrimeiroEvento"];
            bool flagCompareceu = reader["flagCompareceu"] == DBNull.Value ? false : reader.GetBoolean("flagCompareceu");
            string nome = reader["nome"].ToString();
            
            return new Participacao()
            {
                idParticipante = idParticipante,
                idEvento = idEvento,
                flagPrimeiroEvento = flagPrimeiroEvento,
                flagCompareceu = flagCompareceu,
                nome = nome
            };
        }


        public static int CreateParticipacao(Participacao participacao)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO `registoevento` (idParticipante, idEvento, flagPrimeiroEvento,flagCompareceu) VALUES (@idParticipante, @idEvento, @flagPrimeiroEvento,@flagCompareceu)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idParticipante", participacao.idParticipante);
                        sqlCommand.Parameters.AddWithValue("@idEvento", participacao.idEvento);
                        sqlCommand.Parameters.AddWithValue("@flagPrimeiroEvento", participacao.flagPrimeiroEvento);
                        sqlCommand.Parameters.AddWithValue("@flagCompareceu", participacao.flagCompareceu);

                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao registar Participação: " + e.Message);

                if (e.Message.StartsWith("Duplicate"))
                {
                    return -2;
                }

                 return -1;
            }
        }

        public static int CreateParticipacaoEmail(JObject participacao)
        {
            try
            {
                int idPessoa;
                /*OBTEM DADOS DA PESSOA*/
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT idPessoa FROM pessoa WHERE email=@email", conn))
                    {
                        cmd.Parameters.AddWithValue("@email", participacao["email"].ToString());
                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (!reader.Read())
                        {
                            return 0;
                        }

                        idPessoa =(int)reader["idPessoa"];

                    }


                    conn.Close();
                }


                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO `registoevento` (idParticipante, idEvento) VALUES (@idParticipante, @idEvento)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idParticipante", idPessoa);
                        sqlCommand.Parameters.AddWithValue("@idEvento", participacao["idEvento"]);

                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }

                /*ENVIA EMAIL DE CONFIRMAÇÃO*/
            
               if(!SMTPClient.RegistoEventoEmail(participacao["email"].ToString(), new Evento.Evento().GetEvento((int)participacao["idEvento"]))){
                    return -1;
                }
                


                return 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao registar Participação: " + e.Message);

                if (e.Message.StartsWith("Duplicate"))
                {
                    return -2;
                }

                return -1;
            }
        }


        public static int CreateParticipacaoTelemovel(JObject participacao)
        {
            try
            {
                int idPessoa;
                string email;
                /*OBTEM DADOS DA PESSOA*/
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT idPessoa,email FROM pessoa WHERE telefone=@telemovel", conn))
                    {
                        cmd.Parameters.AddWithValue("@telemovel", participacao["telemovel"].ToString());
                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (!reader.Read())
                        {
                            return 0;
                        }

                        idPessoa = (int)reader["idPessoa"];
                        email = reader["email"].ToString();

                    }


                    conn.Close();
                }


                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO `registoevento` (idParticipante, idEvento) VALUES (@idParticipante, @idEvento)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idParticipante", idPessoa);
                        sqlCommand.Parameters.AddWithValue("@idEvento", participacao["idEvento"]);

                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }

                /*ENVIA EMAIL DE CONFIRMAÇÃO*/
                if(email != null)
                {

                    if (!SMTPClient.RegistoEventoEmail(email, new Evento.Evento().GetEvento((int)participacao["idEvento"])))
                    {
                        return -1;
                    }
                }

                

                return 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao registar Participação: " + e.Message);

                if (e.Message.StartsWith("Duplicate"))
                {
                    return -2;
                }

                return -1;
            }
        }

        public static int RegistarComparecimentoInscricaoEventoParticipante(int idEvento, int idParticipante)
        {
            try
            {
                int nrRows = 0;
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("UPDATE `registoevento` SET flagCompareceu = 1 - flagCompareceu WHERE idParticipante = @idParticipante AND idEvento = @idEvento", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idParticipante", idParticipante);
                        sqlCommand.Parameters.AddWithValue("@idEvento", idEvento);

                        nrRows = sqlCommand.ExecuteNonQuery();
                    }

                       
                    conn.Close();
                }

                if(nrRows <= 0)
                {
                    return -1;
                }
              

                return 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao registar comparecimento de Participação: " + e.Message);

                if (e.Message.StartsWith("Duplicate"))
                {
                    return -2;
                }

                return -1;
            }
        }

        public static int RegistarComparecimentoInscricaoEvento(int idEvento)
        {
            try
            {
                int nrRows = 0;
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("UPDATE `registoevento` SET flagCompareceu = 1 - flagCompareceu WHERE idEvento = @idEvento", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idEvento", idEvento);

                        nrRows = sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }

                if (nrRows <= 0)
                {
                    return -1;
                }


                return 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao registar comparecimento de Participação: " + e.Message);

                if (e.Message.StartsWith("Duplicate"))
                {
                    return -2;
                }

                return -1;
            }
        }
    }
}