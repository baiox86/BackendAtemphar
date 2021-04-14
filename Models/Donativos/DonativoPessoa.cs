using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Donativos
{
    public class DonativoPessoa : Donativo
    {
        public int idPessoa { get; set; }

        public static List<DonativoPessoa> GetDonativosPessoa(int idPessoa)
        {

            List<DonativoPessoa> donativos = new List<DonativoPessoa>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM donativopessoa where idPessoa = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPessoa);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DonativoPessoa donativo = new DonativoPessoa
                        {
                            idDonativo = (int)reader["idDonativo"],
                            quantidadeDonativo = (decimal)reader["quantidadeDonativo"],
                            observacoes = (string)reader["observacoes"],
                            idPessoa = (int)reader["idPessoa"],
                            dataDonativo = (string)reader["dataDonativo"].ToString(),

                        };

                        donativos.Add(donativo);
                    }
                    conn.Close();
                }
                return donativos;
            }
        }
        public static decimal GetTotalDonativosPessoa(int idPessoa)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT sum(quantidadeDonativo) as quantidadeDonativo FROM donativopessoa where idPessoa = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPessoa);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();


                    if (reader["quantidadeDonativo"]==null)
                    {
                        return 0;
                    }

                    return (decimal)reader["quantidadeDonativo"];
                }
                
            }
        }
        public static bool CreateDonativo(DonativoPessoa donativo)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO donativopessoa VALUES(@id,@idPessoa,@dataDonativo,@quantidadeDonativo,@obs)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@idPessoa", donativo.idPessoa);
                        sqlCommand.Parameters.AddWithValue("@dataDonativo", donativo.dataDonativo);
                        sqlCommand.Parameters.AddWithValue("@quantidadeDonativo", donativo.quantidadeDonativo);
                        sqlCommand.Parameters.AddWithValue("@obs", donativo.observacoes);
                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.Write("F: " + e.Message);
                return false;
            }

        }

    }
}
