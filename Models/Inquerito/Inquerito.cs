using MlkPwgen;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;


namespace API.Models.Inquerito
{
    public class Inquerito
    {
        int id { get; set; }




        public static List<JObject> GetAllInqueritos()
        {

            List<JObject> jsonL = new List<JObject>();
            dynamic json = new JObject();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM inquerito ", conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        json = new JObject();
                        json.idInquerito = (int)reader["idInquerito"];
                        jsonL.Add(json);
                    }
                }
                conn.Close();
            }

            foreach (dynamic inquerito in jsonL)
            {
                JArray LQ = new JArray();
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT idQuestao,questao,tipoVar FROM questoes_inquerito where idInquerito = @id ", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", inquerito["idInquerito"]);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            json = new JObject();
                            json.questao = reader["questao"].ToString();
                            json.tipoVar = reader["tipoVar"].ToString();
                            json.idQuestao = (int)reader["idQuestao"];
                            LQ.Add(json);


                        }
                        inquerito.questoes = LQ;
                    }
                    conn.Close();
                }

            }
            return jsonL;



        }

        public static List<JObject> GetAllInqueritosEvento()
        {

            List<JObject> inqueritos = new List<JObject>();
            dynamic json = new JObject();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM inquerito_eventos_all ", conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        json = new JObject();
                        json.id = reader.GetInt32("id");
                        json.idEvento = reader.GetInt32("idEvento");
                        json.nomeEvento = reader.GetString("nomeEvento");
                        json.dataEvento = reader.GetString("dataEvento");
                        json.link = reader.GetString("link");
                        json.estado = reader.GetInt32("estado");
                        json.nrQuestoes = reader.GetInt32("nrQuestoes");
                        json.nrRespostas = reader.GetInt32("nrRespostas");

                        inqueritos.Add(json);
                    }
                }
                conn.Close();
            }


            return inqueritos;


        }






        public static JObject GetInqueritoFromToken(string token)
        {
            int idInquerito = -1, ativo = -1;
            string nomeEvento = null, localEvento = null, dataEvento = null, tipoEvento = null;
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT e.nomeEvento AS nomeEvento, e.tipoEvento AS tipoEvento, e.localEvento AS localEvento, e.dataEvento AS dataEvento ,i.idInquerito AS idInquerito ,i.ativo AS ativo FROM evento_inquerito i JOIN evento e ON i.idEvento = e.idEvento where link=@token", conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        idInquerito = reader.GetInt32("idInquerito");
                        ativo = reader.GetInt32("ativo");
                        nomeEvento = reader.GetString("nomeEvento");
                        localEvento = reader.GetString("localEvento");
                        dataEvento = reader.GetString("dataEvento");
                        tipoEvento = reader.GetString("tipoEvento");



                    }


                }
                conn.Close();
            }
            return GetInquerito(idInquerito,ativo,nomeEvento, localEvento, dataEvento,tipoEvento);

        }
        public static JObject GetIDInqueritoFromToken(string token)
        {
            dynamic dados = new JObject();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT idInquerito,idEvento FROM evento_inquerito where link=@token", conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    
                    dados.idInquerito = reader.GetInt32("idInquerito");
                    dados.idEvento = reader.GetInt32("idEvento");
                }
                conn.Close();
            }
            return dados;

        }

        public static JObject GetInquerito(int id)
        {
            return Inquerito.GetInquerito(id, 0, null, null, null,null);
        }


        public static JObject GetInquerito(int id, int ativo, string nomeEvento, string localEvento, string dataEvento,string tipoEvento)
        {

            dynamic jsonL = new JObject();
            dynamic json = new JObject();
            jsonL.idInquerito = id;
            jsonL.ativo = ativo;
            jsonL.nomeEvento = nomeEvento;
            jsonL.localEvento = localEvento;
            jsonL.dataEvento = dataEvento;
            jsonL.tipoEvento = tipoEvento;

            JArray LQ = new JArray();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT idQuestao,questao,tipoVar FROM questoes_inquerito where idInquerito = @id ", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        json = new JObject();
                        json.questao = reader["questao"].ToString();
                        json.tipoVar = reader["tipoVar"].ToString();
                        json.idQuestao = (int)reader["idQuestao"];
                        json.intervalo = ValoresIntervalos.GetAllValoresQuestao((int)reader["idQuestao"]);
                        LQ.Add(json);

                    }

                    jsonL.questoes = LQ;

                    conn.Close();
                }

            }
            return jsonL;



        }
        //DEVOLVE TRUE SE EXISTIREM REGISTOS PARA ESSE EVENTO
        public static bool CheckMapEventoInquerito(long idE)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("Select Count(*) as EXISTE from evento_inquerito where idEvento=@idE ", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idE", idE);

                        MySqlDataReader reader = sqlCommand.ExecuteReader();
                        reader.Read();
                        bool check = ((long)reader["EXISTE"] > 0);
                        return check;
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao checkar evento_inquerito: " + e.Message);
                return false;
            }
        }




        public static int deleteMapEventoInquerito(long idE)
        {

            int check = 0;
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand sqlCommand = new MySqlCommand("delete from evento_inquerito where idEvento=@idE;", conn))
                {
                    sqlCommand.Parameters.AddWithValue("@idE", idE);
                    check = sqlCommand.ExecuteNonQuery();
                }
                conn.Close();
            }
            return check;

        }
        public static long MapEventoInquerito(long idE, long idI)
        {
            string link = PasswordGenerator.Generate(length: 50, allowed: Sets.Alphanumerics);
            long idLast = -1;
            try
            {
                //Se existirem registos temos de eliminar primeiro
                if (CheckMapEventoInquerito(idE))
                {
                    int check = deleteMapEventoInquerito(idE);
                    if (check < 1)
                    {
                        return -1;
                    }
                }
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO evento_inquerito VALUES(@idE,@idI,@link,@true)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idE", idE);
                        sqlCommand.Parameters.AddWithValue("@idI", idI);
                        sqlCommand.Parameters.AddWithValue("@link", link);
                        sqlCommand.Parameters.AddWithValue("@true", true);
                        sqlCommand.ExecuteNonQuery();
                        idLast = sqlCommand.LastInsertedId;
                    }


                    conn.Close();
                }
                return idLast;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar evento_inquerito: " + e.Message);
                return idLast;
            }

        }
        public static long MapQuestaoInquerito(long idI, long idQ)
        {
            long idLast = -1;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO inquerito_questao VALUES(@idI,@idQ)", conn))
                    {

                        sqlCommand.Parameters.AddWithValue("@idI", idI);
                        sqlCommand.Parameters.AddWithValue("@idQ", idQ);
                        sqlCommand.ExecuteNonQuery();
                        idLast = sqlCommand.LastInsertedId;
                    }
                    conn.Close();
                }
                return idLast;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao mapear questao inquerito: " + e.Message);
                return idLast;
            }
        }
        public static long InstanciarInquerito()
        {
            long idLast = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO inquerito VALUES(@id)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", DBNull.Value);
                        sqlCommand.ExecuteNonQuery();
                        idLast = sqlCommand.LastInsertedId;
                    }


                    conn.Close();
                }
                return idLast;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar InstanciaInquerito: " + e.Message);
                return idLast;
            }

        }


        public static long CriarInquerito(JObject json)
        {
            try
            {
                List<Questao> questoes = new List<Questao>();
                foreach (var questao in json["questoes"])
                {
                    questoes.Add(Questao.CreateFromJobj(questao));
                }

                List<ValoresIntervalos> intervalos = ValoresIntervalos.GetAllValores();
                long idQuestao = -1;
                long idIntervalo = -1;

                long idInquerito = InstanciarInquerito();
                foreach (Questao questao in questoes)
                {

                    //Cria-se a a questão
                    idQuestao = Questao.CriarQuestao(questao);
                    if (questao.tipoVar.ToLower() == "intervalo")
                    {
                        foreach (var intervalo in questao.intervalo)
                        {
                            //Para cada intervalo fornecido, verificamos se a tabela responsavel ja tem esse valor registado(ex:Satisfeito)
                            ValoresIntervalos encontrado = intervalos.Find(x => x.valor == intervalo.valor);

                            //Se não for encontrado, tem de se criar um novo
                            if (encontrado == null)
                            {
                                idIntervalo = ValoresIntervalos.CriarValor(intervalo.valor);
                            }
                            else
                            {
                                //Ja existe, aproveitamos o ID
                                idIntervalo = encontrado.id;
                            }

                            //Nesta fase sabemos que existe(obrigatoriamente) o valor do intervalo na BD, então damos map na tabela da questão para o valor
                            ValoresIntervalos.MapIntervaloQuestao(idIntervalo, idQuestao);
                        }
                    }
                    //Já so falta dar map da questão ao inquerito
                    Inquerito.MapQuestaoInquerito(idInquerito, idQuestao);
                }
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }

        }


        public static int UpdateEstadoInquerito(int idInquerito, int idEvento)
        {
            try
            {
                int nrRows = 0;
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("UPDATE evento_inquerito SET ativo=1-ativo WHERE idInquerito=@idInquerito AND idEvento=@idEvento", conn))
                    {

                        sqlCommand.Parameters.AddWithValue("@idInquerito", idInquerito);
                        sqlCommand.Parameters.AddWithValue("@idEvento", idEvento);
                        nrRows = sqlCommand.ExecuteNonQuery();

                    }
                    conn.Close();
                }
                return nrRows;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao mapear questao inquerito: " + e.Message);
                return -1;
            }
        }
    }
}