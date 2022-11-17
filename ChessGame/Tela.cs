using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using tabuleiro.Enums;

namespace ChessGame
{
    internal class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8- i + " ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (tab.PecaPub(i,j) == null)
                        Console.Write("- ");

                    else
                    {
                        ImprimirPeca(tab.PecaPub(i, j));
                        Console.Write(" ");
                    }


                }

                Console.WriteLine();
            }

            Console.Write("  a b c d e f g h");
        }

        public static void ImprimirPeca(Peca peca)
        {
            if (peca.Cor == Cor.Branca)
            {
                Console.Write(peca);
            }

            else
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(peca);
                Console.ForegroundColor = aux;
            }
        }
    }
}
