using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator
{
    public static class Evaluator
    {
        public static int Evaluate(FieldType[] board, bool isWhite = true)
        {
            var result = 0;
            result += PiecePoint(board, isWhite);
            //PossibleStepsPoint
            result += PossibleSteps.AllPiece(board, isWhite).Count() - PossibleSteps.AllPiece(board, !isWhite).Count();
            //TODO I thing it is not the best procedure for this
            return result;
        }

        //P = 100
        //N = 320
        //B = 330
        //R = 500
        //Q = 900

        static int PiecePoint(FieldType[] board, bool isWhite = true, bool endGame = true)
        {
            var result = 0;
            var index = -1;
            if (isWhite)
            {
                foreach (var field in BoardInformations.InsideBoard)
                {
                    index++;
                    switch (board[field])
                    {
                        case FieldType.WhiteQueen:
                            result += QueenPoint[index];
                            result += 900;
                            break;
                        case FieldType.WhiteRock:
                            result += RockPoint[index];
                            result += 500;
                            break;
                        case FieldType.WhiteKnight:
                            result += KnightPoint[index];
                            result += 320;
                            break;
                        case FieldType.WhiteBishop:
                            result += BishopPoint[index];
                            result += 330;
                            break;
                        case FieldType.WhitePawn:
                            result += PawnPoint[index];
                            result += 100;
                            break;
                        case FieldType.WhiteKing:
                            result += endGame ? KingPointEnd[index] : KingPointMiddle[index];
                            break;
                        case FieldType.BlackQueen:
                            result -= QueenPoint[63 - index];
                            result -= 900;
                            break;
                        case FieldType.BlackRock:
                            result -= RockPoint[63 - index];
                            result -= 500;
                            break;
                        case FieldType.BlackKnight:
                            result -= KnightPoint[63 - index];
                            result -= 320;
                            break;
                        case FieldType.BlackBishop:
                            result -= BishopPoint[63 - index];
                            result -= 330;
                            break;
                        case FieldType.BlackPawn:
                            result -= PawnPoint[63 - index];
                            result -= 100;
                            break;
                        case FieldType.BlackKing:
                            result -= endGame ? KingPointEnd[63 - index] : KingPointMiddle[63 - index];
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                foreach (var field in BoardInformations.InsideBoard)
                {
                    index++;
                    switch (board[field])
                    {
                        case FieldType.WhiteQueen:
                            result -= QueenPoint[index];
                            result -= 900;
                            break;
                        case FieldType.WhiteRock:
                            result -= RockPoint[index];
                            result -= 500;
                            break;
                        case FieldType.WhiteKnight:
                            result -= KnightPoint[index];
                            result -= 320;
                            break;
                        case FieldType.WhiteBishop:
                            result -= BishopPoint[index];
                            result -= 330;
                            break;
                        case FieldType.WhitePawn:
                            result -= PawnPoint[index];
                            result -= 100;
                            break;
                        case FieldType.WhiteKing:
                            result -= endGame ? KingPointEnd[index] : KingPointMiddle[index];
                            break;
                        case FieldType.BlackQueen:
                            result += QueenPoint[63 - index];
                            result += 900;
                            break;
                        case FieldType.BlackRock:
                            result += RockPoint[63 - index];
                            result += 500;
                            break;
                        case FieldType.BlackKnight:
                            result += KnightPoint[63 - index];
                            result += 320;
                            break;
                        case FieldType.BlackBishop:
                            result += BishopPoint[63 - index];
                            result += 330;
                            break;
                        case FieldType.BlackPawn:
                            result += PawnPoint[63 - index];
                            result += 100;
                            break;
                        case FieldType.BlackKing:
                            result += endGame ? KingPointEnd[63 - index] : KingPointMiddle[63 - index];
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }

        #region PosPoints


        static int[] RockPoint = new int[] {
         0,0,0,0,0,0,0,0,
         5,10,10,10,10,10,10,5,
        -5,0,0,0,0,0,0,-5,
        -5,0,0,0,0,0,0,-5,
        -5,0,0,0,0,0,0,-5,
        -5,0,0,0,0,0,0,-5,
        -5,0,0,0,0,0,0,-5,
         0,0,0,5,5,0,0,0
        };

        static int[] PawnPoint = new int[] {
             0,  0,  0,  0,  0,  0,  0,  0,
50, 50, 50, 50, 50, 50, 50, 50,
10, 10, 20, 30, 30, 20, 10, 10,
 5,  5, 10, 25, 25, 10,  5,  5,
 0,  0,  0, 20, 20,  0,  0,  0,
 5, -5,-10,  0,  0,-10, -5,  5,
 5, 10, 10,-20,-20, 10, 10,  5,
 0,  0,  0,  0,  0,  0,  0,  0
        };
        static int[] KnightPoint = new int[] {
            -50,-40,-30,-30,-30,-30,-40,-50,
-40,-20,  0,  0,  0,  0,-20,-40,
-30,  0, 10, 15, 15, 10,  0,-30,
-30,  5, 15, 20, 20, 15,  5,-30,
-30,  0, 15, 20, 20, 15,  0,-30,
-30,  5, 10, 15, 15, 10,  5,-30,
-40,-20,  0,  5,  5,  0,-20,-40,
-50,-40,-30,-30,-30,-30,-40,-50
        };
        static int[] BishopPoint = new int[] {
            -20,-10,-10,-10,-10,-10,-10,-20,
-10,  0,  0,  0,  0,  0,  0,-10,
-10,  0,  5, 10, 10,  5,  0,-10,
-10,  5,  5, 10, 10,  5,  5,-10,
-10,  0, 10, 10, 10, 10,  0,-10,
-10, 10, 10, 10, 10, 10, 10,-10,
-10,  5,  0,  0,  0,  0,  5,-10,
-20,-10,-10,-10,-10,-10,-10,-20
        };
        static int[] QueenPoint = new int[] {
            -20,-10,-10, -5, -5,-10,-10,-20,
-10,  0,  0,  0,  0,  0,  0,-10,
-10,  0,  5,  5,  5,  5,  0,-10,
 -5,  0,  5,  5,  5,  5,  0, -5,
  0,  0,  5,  5,  5,  5,  0, -5,
-10,  5,  5,  5,  5,  5,  0,-10,
-10,  0,  5,  0,  0,  0,  0,-10,
-20,-10,-10, -5, -5,-10,-10,-20
        };
        static int[] KingPointMiddle = new int[] {
            -30,-40,-40,-50,-50,-40,-40,-30,
-30,-40,-40,-50,-50,-40,-40,-30,
-30,-40,-40,-50,-50,-40,-40,-30,
-30,-40,-40,-50,-50,-40,-40,-30,
-20,-30,-30,-40,-40,-30,-30,-20,
-10,-20,-20,-20,-20,-20,-20,-10,
 20, 20,  0,  0,  0,  0, 20, 20,
 20, 30, 10,  0,  0, 10, 30, 20
        };
        static int[] KingPointEnd = new int[] {
        -50,-40,-30,-20,-20,-30,-40,-50,
-30,-20,-10,  0,  0,-10,-20,-30,
-30,-10, 20, 30, 30, 20,-10,-30,
-30,-10, 30, 40, 40, 30,-10,-30,
-30,-10, 30, 40, 40, 30,-10,-30,
-30,-10, 20, 30, 30, 20,-10,-30,
-30,-30,  0,  0,  0,  0,-30,-30,
-50,-30,-30,-30,-30,-30,-30,-50
        };
        #endregion

    }
}
