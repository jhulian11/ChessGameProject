using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using tabuleiro.Enums;

namespace xadrez
{
    internal class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor) : base(tab, cor)
        {

        }
        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.PecaPub(pos);
            return p == null || p.Cor != this.Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            //acima
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);

            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.PecaPub(pos) != null && Tab.PecaPub(pos).Cor != Cor)
                {
                    break;
                }

                pos.Linha--;
            }

            //abaixo
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);

            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.PecaPub(pos) != null && Tab.PecaPub(pos).Cor != Cor)
                {
                    break;
                }

                pos.Linha++;
            }

            //direita
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);

            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.PecaPub(pos) != null && Tab.PecaPub(pos).Cor != Cor)
                {
                    break;
                }

                pos.Coluna++;
            }
            //direita
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);

            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.PecaPub(pos) != null && Tab.PecaPub(pos).Cor != Cor)
                {
                    break;
                }

                pos.Coluna--;
            }
           
            return mat;
        }
        public override string ToString()
        {
            return "T";
        }
    }
}
