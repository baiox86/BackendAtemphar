using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Trofeu
{
    public class Trofeu
    {
        public int id { get; set; }
        public TipoTrofeu tipoTrofeu{ get;set; }
        public string descricao { get; set; }
        public int ano { get; set; }
        public int valor { get; set; }
    }
}