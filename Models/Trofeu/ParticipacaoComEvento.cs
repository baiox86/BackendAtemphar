using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace API.Models.Trofeu
{
    public class ParticipacaoComEvento
    {
        public int idEvento { get; set; }
        public string nomeEvento { get; set; }
        public string localEvento { get; set; }
        public string dataEvento { get; set; }
        public string tipoEvento { get; set; }
        public string observacoes { get; set; }
        public int idOrador { get; set; }
        public int idPessoa { get; set; }
        public bool flagPrimeiroEvento { get; set; }
        public string nome { get; set; }


        public static List<ParticipacaoComEvento> GetPartipacaoComEvento(int id)
        {

            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select * from participacoes_evento_view join evento ON evento.idEvento = participacoes_evento_view.idEvento WHERE idParticipante = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<ParticipacaoComEvento> participacoes = new List<ParticipacaoComEvento>();

                    while (reader.Read())
                    {
                        participacoes.Add(ParticipacaoComEvento.FromDB(reader));
                    }

                    return participacoes;

                }
            }
        }


        public static ParticipacaoComEvento FromDB(MySqlDataReader reader)
        {
            int idEvento = (int)reader["idEvento"];
            string nomeEvento = reader["nomeEvento"].ToString();
            string localEvento = reader["localEvento"].ToString();
            string dataEvento = reader["dataEvento"].ToString();
            string observacoes = reader["observacoes"].ToString();
            int idOrador = (int)reader["idOrador"];
            string tipoEvento = reader["tipoEvento"].ToString();
            int idPessoa = (int)reader["idParticipante"];
            bool flagPrimeiroEvento = (bool)reader["flagPrimeiroEvento"];
            string nome = reader["nome"].ToString();

            return new ParticipacaoComEvento()
            {
                idEvento = idEvento,
                nomeEvento = nomeEvento,
                localEvento = localEvento,
                dataEvento = dataEvento,
                observacoes = observacoes,
                idOrador = idOrador,
                tipoEvento = tipoEvento,
                idPessoa = idPessoa,
                flagPrimeiroEvento = flagPrimeiroEvento,
                nome = nome
            };
        }
    }
}

