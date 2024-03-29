﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro.Exeptions;

namespace tabuleiro
{
    internal class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;
            pecas = new Peca[linhas, colunas];
        }

        public Peca PecaPub(int linha, int coluna)
        {
            return pecas[linha,coluna];
        } public Peca PecaPub(Posicao pos)
        {
            return pecas[pos.Linha,pos.Coluna];
        }

        public bool ExistePeca(Posicao pos)
        {
            ValidarPosicao(pos);
            return PecaPub(pos) != null;
        }
        public void ColocarPeca(Peca p, Posicao pos)
        {
            if (ExistePeca(pos))
            {
                throw new TabuleiroException("Ja existe uma peça nessa posição!");
            }
            pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }

        public Peca RetirarPeca(Posicao pos)
        {
            if (PecaPub(pos) == null)
            {
                return null;
            }

            Peca aux = PecaPub(pos);
            aux.Posicao = null;
            pecas[pos.Linha, pos.Coluna] = null;
            return aux;
        }

        public bool PosicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Linha >=Linhas || (pos.Coluna < 0 || pos.Coluna >= Colunas))
            {
                return false;
            }
            return true;
        }

        public void ValidarPosicao(Posicao pos)
        {
            if (!PosicaoValida(pos))
            {
                throw new TabuleiroException("Posição invalida!");
            }
        }
    }
}
