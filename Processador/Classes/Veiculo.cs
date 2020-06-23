using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processador.Classes
{
    class Veiculo
    {
        public string UnitID { get; set; }
        public string idEmpresa { get; set; }
        public Posicao UltimaPosicao { get; set; }
        public List<Posicao> Posicoes { get; set; }

        public Veiculo()
        {
            Posicoes = new List<Posicao>();
        }
    }
}
