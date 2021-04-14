using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.WebPages;

namespace API.Models.Pessoas
{
    public class Pessoa
    {
        [JsonProperty(PropertyName = "idPessoa")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "telefone")]
        public string telefone { get; set; }

        [JsonProperty(PropertyName = "nome")]
        public string nome { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string email { get; set; }

        [JsonProperty(PropertyName = "zona")]
        public string zona { get; set; }

        [JsonProperty(PropertyName = "dataNasc")]
        public string dataNasc { get; set; }

        [JsonProperty(PropertyName = "genero")]
        public string genero { get; set; }

        [JsonProperty(PropertyName = "nif")]
        public string nif { get; set; }

        [JsonProperty(PropertyName = "comoConheceu")]
        public string comoConheceu { get; set; }


       [JsonProperty(PropertyName = "atravesQuem")]
        public int atravesQuem { get; set; }

        [JsonProperty(PropertyName = "atravesQuemNome")]
        public string atravesQuemNome { get; set; }


        public List<Pessoa> GetAllPessoas()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM pessoa", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        pessoas.Add(Pessoa.FromDB(reader));    
                    }
                    conn.Close();
                }
            }
            return pessoas;
        }

        public List<JObject> GetAllPessoasIdNome()
        {
            List<JObject> objects = new List<JObject>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
               
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT idPessoa,nome,email, telefone FROM pessoa", conn))
                {
                  
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        dynamic pessoa = new JObject();
                        pessoa.id  = (int)reader["idPessoa"];
                        pessoa.nome = reader["nome"].ToString() + " - " + reader["email"].ToString() + (reader["telefone"] != null ? " - " + reader["telefone"].ToString() : "");
                        objects.Add(pessoa);
                    }
                    conn.Close();
                }
            }
            return objects;
        }
        public static Pessoa GetPessoa(int id)
        {
            Pessoa pessoa;
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select a.*, b.nome AS atravesQuemNome from pessoa a LEFT JOIN pessoa b ON a.atravesQuem = b.idPessoa WHERE a.idPessoa = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id",id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return null;
                    }
                    pessoa = Pessoa.FromDB(reader);
                    pessoa.atravesQuemNome = reader["atravesQuemNome"] != DBNull.Value ? reader.GetString("atravesQuemNome") : null;

                }

                conn.Close();
            }

            return pessoa;
        }


        public long CreatePessoa(Pessoa pessoa)
        {
            try
            {
                long id = -1;
                int nrRows = 0;
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB)) {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO pessoa VALUES(@id,@nome,@email,@telefone,@zona,@dataNasc,@genero,@nif,@comoConheceu,@atravesQuem)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@telefone",pessoa.telefone);
                        sqlCommand.Parameters.AddWithValue("@nome",pessoa.nome );
                        sqlCommand.Parameters.AddWithValue("@zona",pessoa.zona);
                        sqlCommand.Parameters.AddWithValue("@email",pessoa.email );
                        sqlCommand.Parameters.AddWithValue("@nif",pessoa.nif );
                        sqlCommand.Parameters.AddWithValue("@genero",pessoa.genero );
                        sqlCommand.Parameters.AddWithValue("@dataNasc",pessoa.dataNasc);
                        sqlCommand.Parameters.AddWithValue("@comoConheceu",pessoa.comoConheceu);
                        if (pessoa.atravesQuem!=0)
                        {
                            sqlCommand.Parameters.AddWithValue("@atravesQuem", pessoa.atravesQuem);
                        }
                        else{
                            sqlCommand.Parameters.AddWithValue("@atravesQuem", DBNull.Value);
                        }
                      
                        nrRows = sqlCommand.ExecuteNonQuery();
                        id = sqlCommand.LastInsertedId;
                    }
                    conn.Close();
                }
                if (nrRows <= 0)
                {
                    return -1;
                }

                return id; ;
            }
            catch (Exception e)
            {
               Debug.WriteLine("Erro ao criar pessoa: "+e.Message);
                if (e.Message.StartsWith("Duplicate"))
                {
                    return -2;
                }

                return -1;
            }

        }

        public long CreatePessoaInscricao(Pessoa pessoa, string token)
        {
            try
            {
                long id = -1;
                int nrRows = 0, idPessoa = -1;

                /*VERIFICA SE O TOKEN ENVIADO É VÁLIDO*/
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    /*PROCURAR SE PESSOA ATRAVES QUEM*/
                    using (MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM evento WHERE tokenRegisto=@token", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@token", token);
                        MySqlDataReader reader = sqlCommand.ExecuteReader();

                        if (!reader.Read())
                        {
                            return -1;
                        }
                      

                    }
                    conn.Close();
                }
                if (pessoa.comoConheceu.Equals("Pessoa"))
                {
                    using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                    {
                        conn.Open();
                        /*PROCURAR SE PESSOA ATRAVES QUEM*/
                        using (MySqlCommand sqlCommand = new MySqlCommand("SELECT idPessoa FROM pessoa WHERE nome=@nome", conn))
                        {
                            sqlCommand.Parameters.AddWithValue("@nome", pessoa.atravesQuemNome);
                            MySqlDataReader reader = sqlCommand.ExecuteReader();

                            if (!reader.Read())
                            {
                                idPessoa = -1;
                            }
                            else
                            {
                                idPessoa = reader.GetInt32("idPessoa");
                            }

                        }
                        conn.Close();
                    }

                    if (idPessoa == -1)
                    {
                        using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                        {
                            conn.Open();
                            /*SE NAO ENCONTROU NINGUEM COM O NOME VAI PROCURAR ALGUEM QUE CONTENHA O NOME*/

                            using (MySqlCommand sqlCommand = new MySqlCommand("SELECT idPessoa FROM pessoa WHERE nome LIKE @nome", conn))
                            {
                                sqlCommand.Parameters.AddWithValue("@nome", "%" + pessoa.atravesQuemNome +"%");
                                MySqlDataReader reader = sqlCommand.ExecuteReader();

                                if (reader.Read())
                                {
                                    idPessoa = reader.GetInt32("idPessoa");
                                }

                            }


                            conn.Close();
                        }
                    }
                }


                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {

                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO pessoa(nome,email,telefone,zona,dataNasc,genero,nif,comoConheceu,atravesQuem) VALUES(@nome,@email,@telefone,@zona,@dataNasc,@genero,@nif,@comoConheceu,@atravesQuem)", conn))
                    {
                        conn.Open();

                        sqlCommand.Parameters.AddWithValue("@telefone", pessoa.telefone);
                        sqlCommand.Parameters.AddWithValue("@nome", pessoa.nome);
                        sqlCommand.Parameters.AddWithValue("@zona", pessoa.zona);
                        sqlCommand.Parameters.AddWithValue("@email", pessoa.email);
                        sqlCommand.Parameters.AddWithValue("@nif", pessoa.nif);
                        sqlCommand.Parameters.AddWithValue("@genero", pessoa.genero);
                        sqlCommand.Parameters.AddWithValue("@dataNasc", pessoa.dataNasc);
                        sqlCommand.Parameters.AddWithValue("@comoConheceu", pessoa.comoConheceu);
                        if (idPessoa == -1)
                        {
                            sqlCommand.Parameters.AddWithValue("@atravesQuem", DBNull.Value);
                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue("@atravesQuem", idPessoa);
                        }

                        nrRows = sqlCommand.ExecuteNonQuery();
                        id = sqlCommand.LastInsertedId;
                    }
                    conn.Close();
                }
                if (nrRows <= 0)
                {
                    return -1;
                }

                return id;
                 
               
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar pessoa inscrição: " + e.Message);
                if (e.Message.StartsWith("Duplicate"))
                {
                    return -2;
                }

                return -1;
            }

        }


        public static bool EditPessoa(Pessoa pessoa,int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("Update pessoa set nome=@nome,email=@email,telefone=@telefone,zona=@zona,dataNasc=@dataNasc,genero=@genero,nif=@nif,comoConheceu=@comoConheceu,atravesQuem=@atravesQuem where idPessoa= @id", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", id);
                        sqlCommand.Parameters.AddWithValue("@telefone", pessoa.telefone);
                        sqlCommand.Parameters.AddWithValue("@nome", pessoa.nome);
                        sqlCommand.Parameters.AddWithValue("@zona", pessoa.zona);
                        sqlCommand.Parameters.AddWithValue("@email", pessoa.email);
                        sqlCommand.Parameters.AddWithValue("@nif", pessoa.nif);
                        sqlCommand.Parameters.AddWithValue("@genero", pessoa.genero);
                        sqlCommand.Parameters.AddWithValue("@dataNasc", pessoa.dataNasc);
                        sqlCommand.Parameters.AddWithValue("@comoConheceu", pessoa.comoConheceu);
                        sqlCommand.Parameters.AddWithValue("@atravesQuem", pessoa.atravesQuem); //Devido a hereditariedade atual, nesta classe passa se sempre NULL, fazemos construtor com este campo em pessoaConheceuPorPessoa
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
        public bool DeletePessoa(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM pessoa WHERE idPessoa = @id ",conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", id);
                        sqlCommand.ExecuteNonQuery();
                    }
                    conn.Close();
                    return true;
                }
            }catch(Exception e)
            {
                Debug.WriteLine("F: "+e.Message);
                return false;
            }
        }

       
        /*DEVOLVE JSON OBJECT COM A INFO ACERCA DO PRIMEIRO EVENTO DA PESSOA*/
        public static JObject GetPrimeiroEvento(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select * from primeiro_evento_view WHERE idPessoa = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if(!reader.Read())
                    {
                        return null;
                    }

                    int idPessoa =(int) reader["idPessoa"];
                    int idEvento = (int)reader["idEvento"];
                    string dataEvento = reader["dataEvento"].ToString();
                    bool flagPrimeiroEvento = (bool)reader["flagPrimeiroEvento"];

                    dynamic json = new JObject();

                    json.idPessoa = idPessoa;
                    json.idEvento = idEvento;
                    json.dataEvento = dataEvento;
                    json.flagPrimeiroEvento = flagPrimeiroEvento;


                    return json;
                }
            }
        }


        /*OBTEM PESSOAS QUE OUTRA TROUXE*/
        public static int GetPessoaQueTrouxe(int id)
        {

            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT count(*) AS 'count' FROM pessoa WHERE atravesQuem = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read() || reader["count"] == null)
                    {
                        return 0;
                    }

                    string numeroPessoas = reader["count"].ToString();

                    return int.Parse(numeroPessoas);
                }
            }
        }
        /*Obtem Dados de todos os NÃO oradores*/
        public List<Pessoa> GetAllNaoOradores()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM pessoa_nao_orador_view ", conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Pessoa pessoa = new Pessoa
                        {
                            id = (int)reader["idPessoa"],
                            nome = (string)reader["nome"]
                        };
                        pessoas.Add(pessoa);
                    }
                    conn.Close();
                }
            }
            return pessoas;

        }

        public static List<Pessoa> GetAllPessoasNotStaff()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select * from pessoa p WHERE p.idPessoa NOT IN (SELECT s.idStaff from staff s)", conn))
                {
  
                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Pessoa> pessoas = new List<Pessoa>();


                    while(reader.Read())
                    {
                        pessoas.Add(Pessoa.FromDB(reader));
                    }

                    return pessoas;
                }
            }
        }
      

        public static Pessoa FromDB(MySqlDataReader reader)
        {
            int id = (int)reader["idPessoa"];
            string telefone = reader["telefone"].ToString();
            string nome = reader["nome"].ToString();
            string email = reader["email"].ToString();
            string dataNasc = reader["dataNasc"] == DBNull.Value ? null : reader["dataNasc"].ToString();
            string zona = reader["zona"].ToString();
            string genero = reader["genero"].ToString();
            string nif = reader["nif"] == DBNull.Value ? null : reader["nif"].ToString();
            string comoConheceu = reader["comoConheceu"] == DBNull.Value ? null : reader["comoConheceu"].ToString();
     

            Pessoa pessoa = new Pessoa()
            {
                id = id,
                telefone = telefone,
                nome = nome,
                email = email,
                zona = zona,
                dataNasc = dataNasc,
                genero = genero,
                nif = nif,
                comoConheceu = comoConheceu

            };
            if (reader["atravesQuem"] != DBNull.Value)
            {
                pessoa.atravesQuem = (int)reader["atravesQuem"];
            }
            return pessoa;
        }
       
      
    }
}