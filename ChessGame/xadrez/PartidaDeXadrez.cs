﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using tabuleiro;
using tabuleiro.Enums;
using tabuleiro.Exeptions;

namespace xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set;}
        public bool Terminada { get; private set;}
        public int Turno { get; private set;}
        public Cor JogadorAtual { get; private set;}
        private HashSet<Peca> pecas;
        private HashSet<Peca> pecasCapturadas;
        public bool Xeque { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada =false;
            Xeque = false;
            pecas = new HashSet<Peca>();
            pecasCapturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p,destino);
            if (pecaCapturada != null)
            {
                pecasCapturadas.Add(pecaCapturada);
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada,destino);
                pecasCapturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);
        }
        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem,destino);

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem,destino,pecaCapturada);
                throw new TabuleiroException("Voce não pode se colocar em xeque!");
            }

            if (EstaEmXeque(Adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if (TesteXequemate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.PecaPub(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida");
            }

            if (JogadorAtual != Tab.PecaPub(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não e sua!");

            }

            if (!Tab.PecaPub(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não ha movimentos possiveis para a peça escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.PecaPub(origem).PodeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino Invalida!");

            }
        }
        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (var peca in pecas)
            {
                if (peca.Cor == cor)
                {
                    aux.Add(peca);
                }
            }

            aux.ExceptWith(PecasCapturadas(cor));

            return aux;
        }
        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (var pecaCapturada in pecasCapturadas)
            {
                if (pecaCapturada.Cor == cor)
                {
                    aux.Add(pecaCapturada);
                }
            }
            return aux;
        }

        private Cor Adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }

            return Cor.Branca;
        }

        private Peca Rei(Cor cor)
        {
            foreach (var peca in PecasEmJogo(cor))
            {
                if (peca is Rei)
                {
                    return peca;
                }
            }

            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca Rei = this.Rei(cor);

            if (Rei == null)
                throw new TabuleiroException("Não tem rei da cor "+ cor + " no tabuleiro!");

            foreach (var peca in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = peca.MovimentosPossiveis();
                if (mat[Rei.Posicao.Linha, Rei.Posicao.Coluna])
                {
                    return true;
                }
            }

            return false;
        }

        public bool TesteXequemate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }

            foreach (var peca in PecasEmJogo(cor))
            {
                bool[,] mat = peca.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i,j])
                        {
                            var origem = peca.Posicao;
                            var destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca,new PosicaoXadrez(coluna,linha).ToPosicao());
            pecas.Add(peca);
        }
        private void ColocarPecas()
        {
            ColocarNovaPeca('c', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('c', 2, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('d', 2, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('e', 2, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Rei(Tab, Cor.Branca));

            ColocarNovaPeca('c', 7, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('d', 7, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('e', 7, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Rei(Tab, Cor.Preta));
        }
    }
}
