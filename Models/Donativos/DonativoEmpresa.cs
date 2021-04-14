using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Donativos
{
    public class DonativoEmpresa : Donativo
    {
        public int idEmpresario { get; set; }


        public static List<DonativoEmpresa> GetDonativosEmpresa(int id)
        {

            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("select de.* from donativoempresa de join empresario e on e.idEmpresario = de.idEmpresario where e.idEmpresa = @id", conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<DonativoEmpresa> donativos = new List<DonativoEmpresa>();


                    while (reader.Read())
                    {
                        donativos.Add(DonativoEmpresa.FromDB(reader));
                    }

                    return donativos;
                }
            }
        }
        public static bool CreateDonativoEmpresa(DonativoEmpresa donativoEmpresa)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
                {
                    conn.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO `donativoempresa` (idEmpresario, dataDonativo, quantidadeDonativo, observacoes) VALUES (@idEmpresario, @dataDonativo,@quantidadeDonativo,@observacoes)", conn))
                    {
                        sqlCommand.Parameters.AddWithValue("@idEmpresario", donativoEmpresa.idEmpresario);
                        sqlCommand.Parameters.AddWithValue("@dataDonativo", donativoEmpresa.dataDonativo);
                        sqlCommand.Parameters.AddWithValue("@quantidadeDonativo", donativoEmpresa.quantidadeDonativo);
                        sqlCommand.Parameters.AddWithValue("@observacoes", donativoEmpresa.observacoes);
              
                        sqlCommand.ExecuteNonQuery();
                    }


                    conn.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro ao criar Donativo de Empresa: " + e.Message);
                return false;
            }
        }

        public static DonativoEmpresa FromDB(MySqlDataReader reader)
        {
            int idDonativo = (int) reader["idDonativo"];
            int idEmpresario = (int)reader["idEmpresario"];
            string dataDonativo = reader["dataDonativo"].ToString();
            decimal quantidadeDonativo = (decimal) reader["quantidadeDonativo"];
            string observacoes = reader["observacoes"].ToString();

            return new DonativoEmpresa
            {
                idDonativo = idDonativo,
                idEmpresario = idEmpresario,
                dataDonativo = dataDonativo,
                quantidadeDonativo = quantidadeDonativo,
                observacoes = observacoes
            };
        }
    }
}