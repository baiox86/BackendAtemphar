using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pessoas
{
    public class PessoaConheceuPorPessoa : Pessoa
    {
        public  List<PessoaConheceuPorPessoa> GetAll()
        {
            List<PessoaConheceuPorPessoa> pessoas = new List<PessoaConheceuPorPessoa>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM pessoa  IS NOT null", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
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


                        PessoaConheceuPorPessoa pessoa = new PessoaConheceuPorPessoa()
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
                       
                        pessoas.Add(pessoa);
 
                    }
                    conn.Close();
                }
            }
            return pessoas;
        }

    }
}