using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Generator
    {
        static List<byte> Pieces;

        static void Kings(FieldType[] board)
        {
            Random rnd = new Random();
            var index = rnd.Next(0, 63);
            board[BoardInformations.InsideBoard[index]] = FieldType.WhiteKing;

            var isComplete = false;
            var index2 = rnd.Next(0, 63);
            while (!isComplete)
            {
                if (index2 != index && !PossibleSteps.IsOtherKingNear(board, BoardInformations.InsideBoard[index], BoardInformations.InsideBoard[index2]))
                    isComplete = true;
                else
                    index2 = rnd.Next(0, 63);
            }
            board[BoardInformations.InsideBoard[index2]] = FieldType.BlackKing;
            Pieces.Add(BoardInformations.InsideBoard[index]);
            Pieces.Add(BoardInformations.InsideBoard[index2]);
        }

        static void Rocks(FieldType[] board, byte number, bool isWhite = true)
        {
            var isComplete = false;
            var numberOfRock = 0;
            Random rnd = new Random();
            var index = rnd.Next(0, 63);
            while (!isComplete)
            {
                if (!Pieces.Contains(BoardInformations.InsideBoard[index]))
                {
                    if (isWhite)
                        board[BoardInformations.InsideBoard[index]] = FieldType.WhiteRock;
                    else
                        board[BoardInformations.InsideBoard[index]] = FieldType.BlackRock;
                    Pieces.Add(BoardInformations.InsideBoard[index]);
                    numberOfRock++;
                    if (numberOfRock == number)
                        isComplete = true;
                }
                else
                    index = rnd.Next(0, 63);
            }
        }


        public static void Generate(FieldType[] board, bool checkIsOk = false, bool isWhite = true)
        {
            Pieces = new List<byte>();
            Kings(board);
            Rocks(board, 2, false);
            //AI.AlphaBeta(board, 1, int.MinValue, int.MaxValue, isWhite, new StepAndValue(0, 0, FieldType.Frame, 0, new List<StepAndValue>()));

            StepAndValue SAV = new StepAndValue(0, 0, FieldType.Frame, 0, new List<StepAndValue>());

            AI.AlphaBeta(BoardInformations.CurrentPosition, 5, int.MinValue, int.MaxValue, false, SAV);

            if ((!checkIsOk && (AI.IsCheck(board) || AI.IsCheck(board, false)) || SAV.Children.First(y => y.EvaluatedValue == SAV.Children.Min(x => x.EvaluatedValue)).EvaluatedValue != int.MinValue))
            {
                for (byte i = 0; i < Pieces.Count(); i++)
                    board[Pieces[i]] = FieldType.Empty;
                Generate(board);
            }
        }
    }
}
