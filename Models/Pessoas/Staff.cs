using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pessoas
{
    public class Staff : Pessoa
    {
        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }
        public int totalApoiosOferecidoAno { get; set; }
        public int totalApoios { get; set; }
        public int totalPontuacaoAno { get; set;}
        public int totalPontuacao { get; set; }



        public List<Staff> GetAllStaff()
        {
            List<Staff> staffList = new List<Staff>();
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM staff_view", conn))
                {

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Staff staff = new Staff
                        {
                            id = (int)reader["idStaff"],
                            telefone = (string)reader["telefone"],
                            nome = (string)reader["nome"],
                            email = (string)reader["email"],
               
                        };

                        staffList.Add(staff);
                    }
                    conn.Close();
                }
            }
            return staffList;
        }


    }
}