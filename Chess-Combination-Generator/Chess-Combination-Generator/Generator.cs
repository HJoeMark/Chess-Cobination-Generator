using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator
{
    public static class Generator
    {
        static List<byte> Pieces;
        static List<byte> lastPices;

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

        static void Rooks(FieldType[] board, int number, bool isWhite = true)
        {
            if (number == 0)
                return;
            var isComplete = false;
            var numberOfRook = 0;
            Random rnd = new Random();
            var index = rnd.Next(0, 63);
            while (!isComplete)
            {
                if (!Pieces.Contains(BoardInformations.InsideBoard[index]))
                {
                    if (isWhite)
                        board[BoardInformations.InsideBoard[index]] = FieldType.WhiteRook;
                    else
                        board[BoardInformations.InsideBoard[index]] = FieldType.BlackRook;
                    Pieces.Add(BoardInformations.InsideBoard[index]);
                    numberOfRook++;
                    if (numberOfRook == number)
                        isComplete = true;
                }
                else
                    index = rnd.Next(0, 63);
            }
        }

        static void Knights(FieldType[] board, int number, bool isWhite = true)
        {
            if (number == 0)
                return;
            var isComplete = false;
            var numberOfKnight = 0;
            Random rnd = new Random();
            var index = rnd.Next(0, 63);
            while (!isComplete)
            {
                if (!Pieces.Contains(BoardInformations.InsideBoard[index]))
                {
                    if (isWhite)
                        board[BoardInformations.InsideBoard[index]] = FieldType.WhiteKnight;
                    else
                        board[BoardInformations.InsideBoard[index]] = FieldType.BlackKnight;
                    Pieces.Add(BoardInformations.InsideBoard[index]);
                    numberOfKnight++;
                    if (numberOfKnight == number)
                        isComplete = true;
                }
                else
                    index = rnd.Next(0, 63);
            }
        }

        static void Bishops(FieldType[] board, int number, bool isWhite = true)
        {
            if (number == 0)
                return;
            var isComplete = false;
            var numberOfKnight = 0;
            Random rnd = new Random();
            var index = rnd.Next(0, 63);
            while (!isComplete)
            {
                if (!Pieces.Contains(BoardInformations.InsideBoard[index]))
                {
                    if (isWhite)
                        board[BoardInformations.InsideBoard[index]] = FieldType.WhiteBishop;
                    else
                        board[BoardInformations.InsideBoard[index]] = FieldType.BlackBishop;
                    Pieces.Add(BoardInformations.InsideBoard[index]);
                    numberOfKnight++;
                    if (numberOfKnight == number)
                        isComplete = true;
                }
                else
                    index = rnd.Next(0, 63);
            }
        }

        static void Queen(FieldType[] board, int number, bool isWhite = true)
        {
            if (number == 0)
                return;
            var isComplete = false;
            var numberOfKnight = 0;
            Random rnd = new Random();
            var index = rnd.Next(0, 63);
            while (!isComplete)
            {
                if (!Pieces.Contains(BoardInformations.InsideBoard[index]))
                {
                    if (isWhite)
                        board[BoardInformations.InsideBoard[index]] = FieldType.WhiteQueen;
                    else
                        board[BoardInformations.InsideBoard[index]] = FieldType.BlackQueen;
                    Pieces.Add(BoardInformations.InsideBoard[index]);
                    numberOfKnight++;
                    if (numberOfKnight == number)
                        isComplete = true;
                }
                else
                    index = rnd.Next(0, 63);
            }
        }

        static void Pawn(FieldType[] board, int number, bool isWhite = true)
        {
            if (number == 0)
                return;
            var isComplete = false;
            var numberOfKnight = 0;
            Random rnd = new Random();
            var index = rnd.Next(0, 63);
            while (!isComplete)
            {
                if (!BoardInformations.RowOneEight.Contains(BoardInformations.InsideBoard[index]) && !Pieces.Contains(BoardInformations.InsideBoard[index]))
                {
                    if (isWhite)
                        board[BoardInformations.InsideBoard[index]] = FieldType.WhitePawn;
                    else
                        board[BoardInformations.InsideBoard[index]] = FieldType.BlackPawn;
                    Pieces.Add(BoardInformations.InsideBoard[index]);
                    numberOfKnight++;
                    if (numberOfKnight == number)
                        isComplete = true;
                }
                else
                    index = rnd.Next(0, 63);
            }
        }

        public static ReturnValues Generate(bool checkIsOk = false, bool isWhite = true, int level = 5,
            int bQueens = 0, int bRooks = 2, int bKnights = 2, int bBishops = 0, int bPawns = 0,
            int wQueens = 0, int wRooks = 2, int wKnights = 2, int wBishops = 0, int wPawns = 0)
        {
            FieldType[] board = null;

            while (Pieces == null)
            {
                board = BoardInformations.EmptyBoard;
                Pieces = new List<byte>();
                Kings(board);

                //White  
                Rooks(board, wRooks);
                Knights(board, wKnights);
                Bishops(board, wBishops);
                Queen(board, wQueens);
                Pawn(board, wPawns);
                //Black
                Rooks(board, bRooks, false);
                Knights(board, bKnights, false);
                Bishops(board, bBishops, false);
                Queen(board, bQueens, false);
                Pawn(board, bPawns, false);

                if (lastPices != null && Pieces.SequenceEqual(lastPices))
                    Pieces = null;
            }
            lastPices = new List<byte>();
            Pieces.ForEach((item) =>
            {
                lastPices.Add(item);
            });
            Pieces = null;

            StepAndValue SAV = new StepAndValue(0, 0, FieldType.Frame, 0, new List<StepAndValue>());
            var value = AI.AlphaBeta(board, level, int.MinValue, int.MaxValue, isWhite, SAV);
            var searchedValue = isWhite ? int.MaxValue : int.MinValue;

            if ((!checkIsOk && (AI.IsCheck(board) || AI.IsCheck(board, false)) || value != searchedValue))//SAV.Children.First(y => y.EvaluatedValue == SAV.Children.Min(x => x.EvaluatedValue)).EvaluatedValue != int.MinValue))
                return new ReturnValues() { Fen = "", IsCorrect = false };
            else
                return new ReturnValues() { Fen = BoardInformations.GetFEN(board, isWhite), IsCorrect = true };
        }
    }

    public class ReturnValues
    {
        private string fen;

        public string Fen
        {
            get { return fen; }
            set { fen = value; }
        }
        private bool isCorrect;

        public bool IsCorrect
        {
            get { return isCorrect; }
            set { isCorrect = value; }
        }
    }
}
