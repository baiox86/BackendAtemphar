using API.Models.Pessoas;
using MlkPwgen;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.WebPages;

namespace API.Models.Evento
{
    public class Evento
    {


        public int idEvento { get; set; }
        public string nomeEvento { get; set; }
        public string localEvento { get; set; }
        public string dataEvento { get; set; }
        public string tipoEvento { get; set; }
        public string observacoes { get; set; }
        public int idOrador { get; set; }

        public int idInquerito { get; set; }
        public float custo { get; set; }

        public string tokenRegisto { get; set; }

        /*CRIAR UM LINK DE REGISTO PARA UM EVENTO*/
        public static string GetLinkRegistoEvento(int id)
        {
            int nrRows = 0;

                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM evento WHERE idEvento=@idEvento", conn))
                    {
                        cmd.Parameters.AddWithValue("@idEvento", id);

                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (!reader.Read())
                        {
                            return null;
                        }

                        /*VERIFICA SE O LINK JÁ EXISTE*/
                      if(reader["tokenRegisto"] != DBNull.Value)
                      {
                        return reader["tokenRegisto"].ToString();
                      }
                        
                    }
                    conn.Close();
                }

                string token = PasswordGenerator.Generate(length: 50, allowed: Sets.Alphanumerics);


                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("UPDATE `evento` SET tokenRegisto = @token WHERE idEvento = @idEvento", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@token", token);
                        sqlCommand.Parameters.AddWithValue("@idEvento", id);
                    
                        nrRows = sqlCommand.ExecuteNonQuery();

                        if(nrRows <= 0)
                        {
                            return null;
                        }

                    }

                    conn.Close();
                }

            return token;
        }

        public static long CreatePagamentoEvento(JObject pagamento)
        {
            try
            {
                int nrRows = 0;     
                long lastId = 0;
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO `pagamento_evento` (idEvento, idPessoa, valor, recibo) VALUES (@idEvento, @idPessoa, @valor,@recibo)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idEvento", pagamento["idEvento"]);
                        sqlCommand.Parameters.AddWithValue("@idPessoa", pagamento["idParticipante"]);
                        sqlCommand.Parameters.AddWithValue("@valor", (float)pagamento["valor"]);


                        if (pagamento["recibo"] == null || pagamento["recibo"].ToString().IsEmpty())
                        {
                            sqlCommand.Parameters.AddWithValue("@recibo", DBNull.Value);
                           
                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue("@recibo", (int)pagamento["recibo"]);
                        }

                        nrRows = sqlCommand.ExecuteNonQuery();

                        lastId = sqlCommand.LastInsertedId;
                    }

                    conn.Close();
                }
                if(nrRows <= 0)
                {
                    return -1;
                }
                return lastId;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Pagamento: " + e.Message);
                return -1;
            }
        }


        public static int DeletePagamentoEvento(int idEvento, int idParticpante)
        {
            try
            {
                int nrRows = 0;
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM `pagamento_evento` WHERE idEvento=@idEvento AND idPessoa=@idPessoa", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idEvento", idEvento);
                        sqlCommand.Parameters.AddWithValue("@idPessoa", idParticpante);


                        nrRows = sqlCommand.ExecuteNonQuery();

                    }

                    conn.Close();
                }

                return nrRows;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao remover Pagamento: " + e.Message);
                return -1;
            }
        }

        public static int UpdatePagamentoEvento(int idEvento, int idParticipante, JObject pagamento)
        {
            try
            {
                if(pagamento["valor"] == null)
                {
                    return -1;
                }

                int nrRows = 0;
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("UPDATE  `pagamento_evento` SET valor=@valor, recibo=@recibo WHERE idPessoa=@idPessoa AND idEvento=@idEvento", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idEvento", idEvento);
                        sqlCommand.Parameters.AddWithValue("@idPessoa", idParticipante);
                        sqlCommand.Parameters.AddWithValue("@valor", (float)pagamento["valor"]);

                  

                        if (pagamento["recibo"] == null || pagamento["recibo"].ToString().IsEmpty())
                        {
                            sqlCommand.Parameters.AddWithValue("@recibo", DBNull.Value);
                           
                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue("@recibo", (int)pagamento["recibo"]);
                        }

                        nrRows = sqlCommand.ExecuteNonQuery();
                    }




                    conn.Close();
                }
                if (nrRows <= 0)
                {
                    return 0;
                }
                return 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao atualizar Pagamento: " + e.Message);
                return 1;
            }
        }


        public List<Evento> GetAllEventos()
        {
            List<Evento> eventos = new List<Evento>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select e.idEvento,tokenRegisto,idOrador,nomeEvento,dataEvento,tipoEvento,localEvento,observacoes,idInquerito,custo " +
                    "from evento e left join evento_inquerito i on i.idEvento = e.idEvento " +
                    "left join workshop w on w.idWorkshop = e.idEvento " +
                    "order by idEvento; ", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Evento evento = new Evento
                        {
                            idEvento = (int)reader["idEvento"],
                            nomeEvento = reader["nomeEvento"].ToString(),
                            localEvento = reader["localEvento"].ToString(),
                            dataEvento = reader["dataEvento"].ToString(),
                            observacoes = reader["observacoes"].ToString(),
                            tipoEvento = reader["tipoEvento"].ToString(),




                            tokenRegisto = reader["tokenRegisto"].ToString()

                        };
                        if (reader["idOrador"] != DBNull.Value)
                        {
                            evento.idOrador = (int)reader["idOrador"];
                        }
                        else
                        {
                            evento.idOrador = -1;
                        }
                        if (reader["idInquerito"] != DBNull.Value)
                        {
                            evento.idInquerito = (int)reader["idInquerito"];
                        }
                        else
                        {
                            evento.idInquerito = -1;
                        }
                        if (reader["custo"] != DBNull.Value)
                        {
                            evento.custo = (float)reader["custo"];
                        }
                        else
                        {
                            evento.custo = 0;
                        }
                        eventos.Add(evento);

                    }
                    
                
                    
                }
                
                conn.Close();
            }
            return eventos;
        }

        public List<JObject> GetAllEventosIdNome()
        {
            List<JObject> eventos = new List<JObject>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM evento", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                  
                        dynamic evento = new JObject();
                        evento.id = (int)reader["idEvento"];
                        evento.nome = "NOME: " + reader["nomeEvento"].ToString() + " DATA: " + reader["dataEvento"].ToString() + " LOCAL: " + reader["localEvento"].ToString();
                        evento.tokenRegisto = reader["tokenRegisto"].ToString();
                        eventos.Add(evento);
        

                    }
                    conn.Close();
                }
            }
            return eventos;
        }

        public static List<Evento> GetEventosOrador(int id)
        {

            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select * from evento WHERE idOrador=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Evento> eventos = new List<Evento>();

                    while (reader.Read())
                    {
                        eventos.Add(Evento.FromDB(reader));
                    }

                    return eventos;

                }
            }
        }

        public static List<Pessoa> GetPessoasNaoEvento(int id)
        {

            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT p.* FROM pessoa p WHERE NOT EXISTS (SELECT 1 FROM registoevento e WHERE p.idPessoa = e.idParticipante AND idEvento = @idEvento1) AND NOT EXISTS (SELECT 1 FROM evento e WHERE p.idPessoa = e.idOrador AND idEvento =  @idEvento2)", conn))
                {
                    cmd.Parameters.AddWithValue("@idEvento1", id);
                    cmd.Parameters.AddWithValue("@idEvento2", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Pessoa> pessoas = new List<Pessoa>();

                    while (reader.Read())
                    {
                        pessoas.Add(Pessoa.FromDB(reader));
                    }

                    return pessoas;

                }
            }

        }


        public static Evento FromDB(MySqlDataReader reader)
        {
            int idEvento = (int)reader["idEvento"];
            string nomeEvento = reader["nomeEvento"].ToString();
            string localEvento = reader["localEvento"].ToString();
            string dataEvento = reader["dataEvento"].ToString();
            string observacoes = reader["observacoes"].ToString();
            int idOrador = (int)reader["idOrador"];
            string tipoEvento = reader["tipoEvento"].ToString();

            return new Evento(){
                idEvento = idEvento,
                nomeEvento = nomeEvento,
                localEvento = localEvento,
                dataEvento = dataEvento,
                observacoes = observacoes,
                idOrador = idOrador,
                tipoEvento = tipoEvento
            };


        }

        public Evento GetEvento(int id)
        {
            Evento evento;
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM evento where idEvento = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    evento = new Evento
                    {
                        idEvento = (int)reader["idEvento"],
                        nomeEvento = reader["nomeEvento"].ToString(),
                        localEvento = reader["localEvento"].ToString(),
                        dataEvento = reader["dataEvento"].ToString(),
                        observacoes = reader["observacoes"].ToString(),
                        tipoEvento = reader["tipoEvento"].ToString()
                    };
                    if (reader["idOrador"] != DBNull.Value)
                    {
                        evento.idOrador = (int)reader["idOrador"];
                    }



                    conn.Close();
                }
            }
            return evento;
        }

        public static Evento GetEventoByToken(string token)
        {
            Evento evento;
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM evento where tokenRegisto = @token", conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.Read())
                    {
                        return null;
                    }

                    evento = new Evento
                    {
                        idEvento = (int)reader["idEvento"],
                        nomeEvento = reader["nomeEvento"].ToString(),
                        localEvento = reader["localEvento"].ToString(),
                        dataEvento = reader["dataEvento"].ToString(),
                        observacoes = reader["observacoes"].ToString(),
                        tipoEvento = reader["tipoEvento"].ToString(),
                        tokenRegisto = reader["tokenRegisto"].ToString()

                };
                    if (reader["idOrador"] != DBNull.Value)
                    {
                        evento.idOrador = (int)reader["idOrador"];
                    }



                    conn.Close();
                }
            }
            return evento;
        }

        public static bool CreateEvento(Evento evento)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO `evento` (idOrador, nomeEvento, dataEvento, localEvento, tipoEvento, observacoes) VALUES (@idOrador, @nomeEvento, @dataEvento, @localEvento, @tipoEvento, @observacoes)", conn))
                    {
                        if (evento.idOrador == 0)
                        {
                            sqlCommand.Parameters.AddWithValue("@idOrador", DBNull.Value);

                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue("@idOrador", evento.idOrador);
                        }
                        sqlCommand.Parameters.AddWithValue("@nomeEvento", evento.nomeEvento);
                        sqlCommand.Parameters.AddWithValue("@dataEvento", evento.dataEvento);
                        sqlCommand.Parameters.AddWithValue("@localEvento", evento.localEvento);
                        sqlCommand.Parameters.AddWithValue("@tipoEvento", evento.tipoEvento);
                        sqlCommand.Parameters.AddWithValue("@observacoes", evento.observacoes);

                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Evento: " + e.Message);
                return false;
            }
        }
        public List<Evento> GetAllEventosTipo(string tipoEvento)
        {
            List<Evento> eventos = new List<Evento>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select e.idEvento,tokenRegisto,idOrador,nomeEvento,dataEvento,tipoEvento,localEvento,observacoes,idInquerito,custo " +
                    "from evento e left join evento_inquerito i on i.idEvento = e.idEvento " +
                    "left join workshop w on w.idWorkshop = e.idEvento where e.tipoEvento=@tipoEvento " +
                    "order by idEvento ", conn))
                {
                    cmd.Parameters.AddWithValue("@tipoEvento", tipoEvento);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Evento evento = new Evento
                        {
                            idEvento = (int)reader["idEvento"],
                            nomeEvento = reader["nomeEvento"].ToString(),
                            localEvento = reader["localEvento"].ToString(),
                            dataEvento = reader["dataEvento"].ToString(),
                            observacoes = reader["observacoes"].ToString(),
                            tipoEvento = reader["tipoEvento"].ToString(),




                            tokenRegisto = reader["tokenRegisto"].ToString()

                        };
                        if (reader["idOrador"] != DBNull.Value)
                        {
                            evento.idOrador = (int)reader["idOrador"];
                        }
                        else
                        {
                            evento.idOrador = -1;
                        }
                        if (reader["idInquerito"] != DBNull.Value)
                        {
                            evento.idInquerito = (int)reader["idInquerito"];
                        }
                        else
                        {
                            evento.idInquerito = -1;
                        }
                        if (reader["custo"] != DBNull.Value)
                        {
                            evento.custo = (float)reader["custo"];
                        }
                        else
                        {
                            evento.custo = 0;
                        }
                        eventos.Add(evento);
                    }
                    conn.Close();
                }
            }
            return eventos;
        }


        public List<JObject> GetAllEventosTipoIdNome(string tipoEvento)
        {

            List<JObject> objects = new List<JObject>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM evento where tipoEvento=@tipoEvento", conn))
                {
                    cmd.Parameters.AddWithValue("@tipoEvento", tipoEvento);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        dynamic evento = new JObject();
                        evento.id = (int)reader["idEvento"];
                        evento.nome = "NOME: " + reader["nomeEvento"].ToString() + " DATA: " + reader["dataEvento"].ToString() + " LOCAL: " + reader["localEvento"].ToString();

                        objects.Add(evento);
                    }
                    conn.Close();
                }
            }
            return objects;
        }

         public static List<JObject> GetInscricoesPagamentosEvento(int id)
        {

            List<JObject> objects = new List<JObject>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM inscricoes_pagamentos_evento where idEvento=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        dynamic valor = new JObject();

                        valor.idEvento = reader.GetInt32("idEvento");
                        valor.idParticipante = reader.GetInt32("idParticipante");
                        valor.flagPrimeiroEvento = reader.GetBoolean("flagPrimeiroEvento");
                        valor.flagCompareceu = reader["flagCompareceu"] == DBNull.Value ? false : reader.GetBoolean("flagCompareceu");
                        valor.valor = reader["valor"] == DBNull.Value ? -1 : reader.GetFloat("valor");
                        valor.recibo = reader["recibo"] == DBNull.Value ? -1 : reader.GetInt32("recibo");
                        valor.nome = reader.GetString("nome");
                        valor.email = reader.GetString("email");
                        valor.necessitaPagamento = reader.GetBoolean("necessitaPagamento");
                        

                        objects.Add(valor);
                    }
                    conn.Close();
                }
            }
            return objects;
        }

        public static int EditEvento(int id, Evento evento)
        {
            int nrRows;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("UPDATE `evento` SET idOrador = @idOrador, nomeEvento = @nomeEvento, dataEvento = @dataEvento, localEvento = @localEvento, tipoEvento = @tipoEvento, observacoes = @observacoes WHERE idEvento = @idEvento", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idOrador", id);
                        sqlCommand.Parameters.AddWithValue("@nomeEvento", evento.nomeEvento);
                        sqlCommand.Parameters.AddWithValue("@dataEvento", evento.dataEvento);
                        sqlCommand.Parameters.AddWithValue("@localEvento", evento.localEvento);
                        sqlCommand.Parameters.AddWithValue("@tipoEvento", evento.tipoEvento);
                        sqlCommand.Parameters.AddWithValue("@observacoes", evento.observacoes);
                        sqlCommand.Parameters.AddWithValue("@idEvento", id);


                         nrRows = sqlCommand.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                if(nrRows == 0)
                {
                    return 0;
                }

                return 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao editar Evento: " + e.Message);
                return -1;
            }
        }

    }
}
