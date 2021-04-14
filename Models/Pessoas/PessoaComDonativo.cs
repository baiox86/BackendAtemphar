using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Pessoas
{
    public class PessoaComDonativo : Pessoa
    {
        public int totalDonativos { get; set; }
        public int totalDonativosAno { get; set; }
    }
}