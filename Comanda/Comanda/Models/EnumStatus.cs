using System;
using System.Collections.Generic;
using System.Text;

namespace Comanda.Models
{
    class EnumStatus
    {
        public enum StatusMesa
        {
            Disponivel = 0, Ocupado = 1, Ausente = 2
        }

        public enum StatusVenda
        {
            Disponivel = 0, Ocupado = 1, ReceberPagamento = 2, Finalizado = 3
        }

        public enum StatusCaixa
        {
            Aberto = 0,
            Fechado = 1
        }
    }
}
