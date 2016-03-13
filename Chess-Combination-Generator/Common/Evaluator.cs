using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
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

        static int PiecePoint(FieldType[] board, bool isWhite = true)
        {
            var result = 0;
            if (isWhite)
            {
                foreach (var field in BoardInformations.InsideBoard)
                {
                    switch (board[field])
                    {
                        case FieldType.WhiteQueen:
                            result += 900;
                            break;
                        case FieldType.WhiteRock:
                            result += 500;
                            break;
                        case FieldType.WhiteKnight:
                            result += 300;
                            break;
                        case FieldType.WhiteBishop:
                            result += 300;
                            break;
                        case FieldType.WhitePawn:
                            result += 100;
                            break;
                        case FieldType.BlackQueen:
                            result -= 900;
                            break;
                        case FieldType.BlackRock:
                            result -= 500;
                            break;
                        case FieldType.BlackKnight:
                            result -= 300;
                            break;
                        case FieldType.BlackBishop:
                            result -= 300;
                            break;
                        case FieldType.BlackPawn:
                            result -= 100;
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
                    switch (board[field])
                    {
                        case FieldType.WhiteQueen:
                            result -= 900;
                            break;
                        case FieldType.WhiteRock:
                            result -= 500;
                            break;
                        case FieldType.WhiteKnight:
                            result -= 300;
                            break;
                        case FieldType.WhiteBishop:
                            result -= 300;
                            break;
                        case FieldType.WhitePawn:
                            result -= 100;
                            break;
                        case FieldType.BlackQueen:
                            result += 900;
                            break;
                        case FieldType.BlackRock:
                            result += 500;
                            break;
                        case FieldType.BlackKnight:
                            result += 300;
                            break;
                        case FieldType.BlackBishop:
                            result += 300;
                            break;
                        case FieldType.BlackPawn:
                            result += 100;
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }
    }
}
