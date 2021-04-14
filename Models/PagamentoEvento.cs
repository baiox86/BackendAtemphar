using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class PagamentoEvento
    {
        public int idPessoa { get; set; }
        public int idEvento { get; set; }
        public float valor { get; set; }
        public int recibo { get; set; }



        public static PagamentoEvento FromDB(MySqlDataReader reader)
        {
            int idPessoa = (int)reader["idPessoa"];
            int idEvento = (int)reader["idEvento"];
            float valor = (float)reader["valor"];
            int recibo = (int)reader["recibo"];

            return new PagamentoEvento()
            {
                idPessoa = idPessoa,
                idEvento = idEvento,
                valor = valor,
                recibo = recibo
            };
        }

    }
}