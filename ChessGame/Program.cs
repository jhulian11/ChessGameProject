using ChessGame;
using tabuleiro;
using tabuleiro.Enums;
using tabuleiro.Exeptions;
using xadrez;

//try
//{
//    Tabuleiro tab = new Tabuleiro(8, 8);

//    tab.ColocarPeca(new Torre(tab, Cor.Preta), new Posicao(0, 0));
//    tab.ColocarPeca(new Torre(tab, Cor.Preta), new Posicao(1, 3));
//    tab.ColocarPeca(new Rei(tab, Cor.Preta), new Posicao(2, 4));

//    Tela.ImprimirTabuleiro(tab);
//}

//catch (TabuleiroException e)
//{
//    Console.WriteLine(e.Message);
//}

PosicaoXadrez pos = new PosicaoXadrez('c', 7);

Console.WriteLine(pos);
Console.WriteLine(pos.ToPosicao());

Console.ReadLine();