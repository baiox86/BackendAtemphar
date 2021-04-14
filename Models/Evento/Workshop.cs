using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Evento
{
    public class Workshop : EventoComOrador
    {
        public float custo { get; set; }
        public int idWorkshop { get; set; }


        public float GetCustoWorkshop(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM workshop_view where idWorkshop = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    float custo = (float)reader["custo"];
                    
                    conn.Close();
                }
            }


            return custo;
        }

        public static bool CreateWorkshop(Workshop workshop)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO `workshop` (idWorkshop, custo) VALUES (@idWorkshop, @custo)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idWorkshop", workshop.idWorkshop);
                        sqlCommand.Parameters.AddWithValue("@custo", workshop.custo);

                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Workshop: " + e.Message);
                return false;
            }
        }
    }
}