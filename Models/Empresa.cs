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
    public class Empresa
    {
        public int idEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        public string moradaEmpresa { get; set; }
        public string telefoneEmpresa { get; set; }
        public string emailEmpresa { get; set; }
        public string nifEmpresa { get; set; }
        public string observacoes { get; set; }

        public static List<Empresa> GetAll()
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from empresa", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Empresa> empresas = new List<Empresa>();


                    while (reader.Read())
                    {
                        empresas.Add(Empresa.FromDB(reader));
                    }

                    return empresas;
                }
            }
        }

        public static Empresa GetDados(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from empresa WHERE idEmpresa = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return null;
                    }

                    return Empresa.FromDB(reader);
                }
            }
        }

        public static JObject GetDadosEmpresario(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT e.idEmpresario, p.nome, p.email, p.telefone FROM empresario e JOIN pessoa p ON p.idPessoa = e.idEmpresario WHERE e.idEmpresa = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return null;
                    }

                    dynamic json = new JObject();

                    json.idEmpresario = (int) reader["idEmpresario"];
                    json.nome = reader["nome"].ToString() ;
                    json.email = reader["email"].ToString();
                    json.telefone = reader["telefone"].ToString() ;


                    return json;
                }
            }
        }


        public static List<Pessoa> GetPessoasNaoEmEmpresa(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT p.* FROM pessoa p WHERE NOT EXISTS (SELECT 1 FROM empresario e WHERE p.idPessoa = e.idEmpresario AND idEmpresa = @id)", conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);

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

        public static bool CreateEmpresa(Empresa empresa)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO empresa (nomeEmpresa, moradaEmpresa, telefoneEmpresa, emailEmpresa, nifEmpresa, observacoes) VALUES (@nomeEmpresa, @moradaEmpresa, @telefoneEmpresa, @emailEmpresa, @nifEmpresa , @observacoes)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@nomeEmpresa", empresa.nomeEmpresa);
                        sqlCommand.Parameters.AddWithValue("@moradaEmpresa", empresa.moradaEmpresa);
                        sqlCommand.Parameters.AddWithValue("@telefoneEmpresa", empresa.telefoneEmpresa);
                        sqlCommand.Parameters.AddWithValue("@emailEmpresa", empresa.emailEmpresa);
                        sqlCommand.Parameters.AddWithValue("@nifEmpresa", empresa.nifEmpresa);
                        sqlCommand.Parameters.AddWithValue("@observacoes", empresa.observacoes);

                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Empresa: " + e.Message);
                return false;
            }
        }

        public static Empresa FromDB(MySqlDataReader reader)
        {
            int idEmpresa =(int) reader["idEmpresa"];
            string nomeEmpresa = reader["nomeEmpresa"].ToString();
            string moradaEmpresa = reader["moradaEmpresa"].ToString();
            string telefoneEmpresa = reader["telefoneEmpresa"].ToString();
            string emailEmpresa = reader["emailEmpresa"].ToString();
            string nifEmpresa = reader["nifEmpresa"].ToString();
            string observacoes = reader["observacoes"].ToString();


            return new Empresa
            {
                idEmpresa = idEmpresa,
                nomeEmpresa = nomeEmpresa,
                moradaEmpresa = moradaEmpresa,
                telefoneEmpresa = telefoneEmpresa,
                emailEmpresa = emailEmpresa,
                nifEmpresa = nifEmpresa,
                observacoes = observacoes
            };
            
        }
    }
}