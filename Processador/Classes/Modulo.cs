using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{
    class Modulo
    {
        public string id { get; set; }

        public int idVeiculo { get; set; }

        public int idEmpresa { get; set; }

        public string banco { get; set; }

        public Modulo( string ID,int IDVEICULO,int IDEMPRESA,string BANCO)
        {
            id = ID;
            idVeiculo = IDVEICULO;
            idEmpresa = IDEMPRESA;
            banco = BANCO;
        }
    }
}
