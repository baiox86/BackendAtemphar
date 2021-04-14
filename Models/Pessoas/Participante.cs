using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pessoas
{
    public class Participante : Pessoa
    {

        public static List<Participante> GetAllParticipantes()
        {
            List<Participante> participantes = new List<Participante>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from participante_view", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        participantes.Add(Participante.FromDB(reader));
                    }
                    conn.Close();
                }
            }
            return participantes;
        }
        public static Participante FromDB(MySqlDataReader reader)
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


            Participante participante = new Participante()
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
                participante.atravesQuem = (int)reader["atravesQuem"];
            }
            return participante;
        }


    }
}