using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Evento
{
    public class Palestra : EventoComOrador
    {

        public int idSimposio { get; set; }
        public int idPalestra { get; set; }
        public string nomeOrador { get; set; }


        public static List<Palestra> GetAllPalestrasSimposio(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("Select * from palestras_simposio_view WHERE idSimposio = @id order by dataEvento DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    List<Palestra> palestras = new List<Palestra>();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        palestras.Add(Palestra.FromDB(reader));
                    }


                    return palestras;

                }
            }

        }
        public static bool CreatePalestraSimposio(Palestra palestra)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO palestrasimposio VALUES(@id1,@id2)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@id2", palestra.idSimposio);
                        sqlCommand.Parameters.AddWithValue("@id1", palestra.idPalestra);
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



        public static Palestra FromDB(MySqlDataReader reader)
        {
            int idSimposio = (int)reader["idSimposio"];
            int idPalestra = (int)reader["idPalestra"];
            string nomeOrador = reader["nomeOrador"].ToString();
            string nomeEvento = reader["nomeEvento"].ToString();
            string dataEvento = reader["dataEvento"].ToString();
            string localEvento = reader["localEvento"].ToString();
            string observacoes = reader["observacoes"].ToString();

            return new Palestra()
            {
                idSimposio = idSimposio,
                nomeEvento = nomeEvento,
                localEvento = localEvento,
                dataEvento = dataEvento,
                observacoes = observacoes,
                nomeOrador = nomeOrador,
                idPalestra = idPalestra
            };

        }
    }
}