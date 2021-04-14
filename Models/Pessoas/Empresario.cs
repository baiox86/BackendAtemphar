using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pessoas
{
    public class Empresario : PessoaComDonativo
    {
        public int idEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        public static List<Empresario> GetAll()
        {

            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from empresario_view", conn))
                {
     
                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<Empresario> empresarios = new List<Empresario>();

                    while(reader.Read())
                    {
                        empresarios.Add(Empresario.FromDB(reader));
                    }

                    return empresarios;
                }
            }
        }

        public static Empresario GetDados(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from empresario_view WHERE idPessoa = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return null;
                    }

                    return Empresario.FromDB(reader);
                }
            }
        }


        public static Empresario FromDB(MySqlDataReader reader)
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
            string nomeEmpresa = reader["nomeEmpresa"].ToString();

            Empresario empresario =  new Empresario()
            {
                id = id,
                telefone = telefone,
                nome = nome,
                email = email,
                zona = zona,
                genero = genero,
                nif = nif,
                comoConheceu = comoConheceu,
                nomeEmpresa = nomeEmpresa
            };

            if (reader["atravesQuem"] != DBNull.Value)
            {
                empresario.atravesQuem = (int)reader["atravesQuem"];
            }
            return empresario;
        }
        public static bool CreateEmpresario(Empresario empresario)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO empresario VALUES(@id,@idEmpresa)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", empresario.id);
                        sqlCommand.Parameters.AddWithValue("@idEmpresa",empresario.idEmpresa);
                        sqlCommand.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}