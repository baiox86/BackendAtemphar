using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Donativo
    {
        public int idDonativo { get; set; }
        public string dataDonativo { get; set; }
        public string observacoes { get; set; }

   
        public Decimal quantidadeDonativo { get; set; }


        
      
    }
}