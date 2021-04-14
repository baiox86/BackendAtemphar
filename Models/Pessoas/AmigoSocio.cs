using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Pessoas
{
    public class AmigoSocio : PessoaComDonativo
    {
        public int idAmigoSocio { get; set; }
        public string morada { get; set; }
        public string codPostal { get; set; }
        public string dataExpiracao { get; set; }


        public AmigoSocio GetAmigoSocio(int id)
        {
            AmigoSocio amigosocio;
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM amigosocio idPessoa= @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    amigosocio = new AmigoSocio
                    {
                        id = (int)reader["idAmigoSocio"],
                        morada = (string)reader["morada"],
                        codPostal = (string)reader["codPostal"],
                        dataExpiracao = (string)reader["dataExpiracao"].ToString()

                    };
                    conn.Close();
                }
            }


            return amigosocio;
        }
        public List<AmigoSocio> GetAmigosSocio()
        {
            List<AmigoSocio> amigos = new List<AmigoSocio>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM amigosocio", conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        AmigoSocio amigosocio = new AmigoSocio
                        {
                            id = (int)reader["idAmigoSocio"],
                            morada = (string)reader["morada"],
                            codPostal = (string)reader["codPostal"],
                            dataExpiracao = (string)reader["dataExpiracao"].ToString()

                        };
                        amigos.Add(amigosocio);
                    }
                    conn.Close();
                }
            }


            return amigos;
        }

        public static bool CreateAmigoSocio(AmigoSocio amigoSocio)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO `amigosocio` (idAmigoSocio, morada, codPostal, dataExpiracao) VALUES (@idAmigoSocio, @morada, @codPostal, @dataExpiracao)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idAmigoSocio", amigoSocio.idAmigoSocio);
                        sqlCommand.Parameters.AddWithValue("@morada", amigoSocio.morada);
                        sqlCommand.Parameters.AddWithValue("@codPostal", amigoSocio.codPostal);
                        sqlCommand.Parameters.AddWithValue("@dataExpiracao", amigoSocio.dataExpiracao);

                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Amigo Sócio: " + e.Message);
                return false;
            }
        }

    }
}