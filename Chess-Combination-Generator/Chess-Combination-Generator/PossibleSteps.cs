using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator
{
    //TODO: Steps functions may be combined
    public static class PossibleSteps
    {
        static byte[] WithKing(FieldType[] board, byte? wKingPos, byte? bKingPos, bool isWhite = true)
        {
            List<byte> result = new List<byte>();
            byte newPos;
            if (isWhite) //WHITE
            {
                //UP -13,-12,-11
                //LEFT -1
                //RIGHT +1
                //DOWN +11,+12,+13
                foreach (var item in KingSteps)
                {
                    newPos = (byte)(wKingPos - item);
                    //FieldType 8 = black king and higher are his people
                    //FieldType[] newBoard = new FieldType[144];
                    //Array.Copy(board, newBoard, 144);
                    if (!BoardInformations.Frame.Contains(newPos) && (board[newPos] == FieldType.Empty || (byte)board[newPos] > 7) /*&& !IsOtherKingNear(board, newPos, bKingPos, isWhite) *//*&& !IsCheck(newBoard, newPos)*/)
                        result.Add(newPos);
                }
            }
            else //BLACK
                foreach (var item in KingSteps)
                {
                    newPos = (byte)(bKingPos - item);
                    //FieldType 2-8 white people
                    //FieldType[] newBoard = new FieldType[144];
                    //Array.Copy(board, newBoard, 144);
                    if (!BoardInformations.Frame.Contains(newPos) && (board[newPos] == FieldType.Empty || ((byte)board[newPos] > 1 && (byte)board[newPos] < 8)) /*&& !IsOtherKingNear(board, wKingPos, newPos, isWhite)*/ /*&& !IsCheck(newBoard, newPos, false)*/)
                        result.Add(newPos);
                }

            return result.ToArray();
        }

        #region With Position

        public static byte[] WithKing(FieldType[] board, byte kingPos, bool isWhite = true)
        {
            List<byte> result = new List<byte>();
            byte newPos;
            if (isWhite)
            {
                foreach (var item in KingSteps)
                {
                    newPos = (byte)(kingPos - item);
                    if (!BoardInformations.Frame.Contains(newPos) && (board[newPos] == FieldType.Empty || (byte)board[newPos] > 8) /*&& !IsOtherKingNear(board, newPos, bKingPos, isWhite)*/)
                        result.Add(newPos);
                }
            }
            else
                foreach (var item in KingSteps)
                {
                    newPos = (byte)(kingPos - item);
                    if (!BoardInformations.Frame.Contains(newPos) && (board[newPos] == FieldType.Empty || ((byte)board[newPos] > 2 && (byte)board[newPos] < 8)) /*&& !IsOtherKingNear(board, wKingPos, newPos, isWhite)*/)
                        result.Add(newPos);
                }

            return result.ToArray();
        }

        public static byte[] WithKnight(FieldType[] board, byte knightPos, bool isWhite = true)
        {
            List<byte> result = new List<byte>();
            int newPos;
            if (isWhite)
                foreach (var item in KnightSteps)
                {
                    newPos = (knightPos + item);
                    if (newPos >= 0)
                        if (board[newPos] != FieldType.Frame && (((byte)board[newPos] > 7) || board[newPos] == FieldType.Empty))
                            result.Add((byte)newPos);
                }
            else
                foreach (var item in KnightSteps)
                {
                    newPos = (knightPos + item);
                    if (newPos >= 0)
                        if (board[newPos] != FieldType.Frame && (((byte)board[newPos] > 1 && (byte)board[newPos] < 8) || board[newPos] == FieldType.Empty))
                            result.Add((byte)newPos);
                }

            return result.ToArray();
        }

        public static byte[] WithPiece(FieldType[] board, byte piecePos, int[] steps, bool isWhite = true)
        {
            List<byte> result = new List<byte>();
            int newPos;
            if (isWhite)
                foreach (var item in steps)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        newPos = (piecePos + item * i);
                        if (board[newPos] == FieldType.Frame)
                            break;
                        if (board[newPos] == FieldType.Empty)
                        {
                            result.Add((byte)newPos);
                            continue;
                        }
                        if ((byte)board[newPos] > 7)
                        {
                            result.Add((byte)newPos);
                            break;
                        }
                        else
                            break;
                    }
                }
            else
                foreach (var item in steps)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        newPos = (piecePos + item * i);
                        if (board[newPos] == FieldType.Frame)
                            break;
                        if (board[newPos] == FieldType.Empty)
                        {
                            result.Add((byte)newPos);
                            continue;
                        }
                        if (((byte)board[newPos] > 1 && (byte)board[newPos] < 8))
                        {
                            result.Add((byte)newPos);
                            break;
                        }
                        else
                            break;
                    }
                }
            return result.ToArray();

        }
        #endregion

        static byte[] Steps(FieldType[] board, byte figPos, bool isWhite = true)
        {
            byte[] result = null;
            switch (board[figPos])
            {
                case FieldType.WhiteKing:
                    result = WithKing(board, figPos);
                    break;
                case FieldType.WhiteQueen:
                    result = WithPiece(board, figPos, QueenSteps);
                    break;
                case FieldType.WhiteRock:
                    result = WithPiece(board, figPos, RockSteps);
                    break;
                case FieldType.WhiteKnight:
                    result = WithKnight(board, figPos);
                    break;
                case FieldType.WhiteBishop:
                    result = WithPiece(board, figPos, BishopSteps);
                    break;
                case FieldType.WhitePawn:
                    break;
                case FieldType.BlackKing:
                    result = WithKing(board, figPos, false);
                    break;
                case FieldType.BlackQueen:
                    result = WithPiece(board, figPos, QueenSteps, false);
                    break;
                case FieldType.BlackRock:
                    result = WithPiece(board, figPos, RockSteps, false);
                    break;
                case FieldType.BlackKnight:
                    result = WithKnight(board, figPos, false);
                    break;
                case FieldType.BlackBishop:
                    result = WithPiece(board, figPos, BishopSteps, false);
                    break;
                case FieldType.BlackPawn:
                    break;
                default:
                    break;
            }
            return result;
        }

        public static byte[] AllPiece(FieldType[] board, bool isWhite = true)
        {
            //TODO pawn
            var wKingPos = WhereIsTheKing(board);
            var bKingPos = WhereIsTheKing(board, false);
            //Smash the king
            if (bKingPos > 144 || wKingPos > 144)
                return new List<byte>().ToArray();
            List<byte> result = new List<byte>();
            result.AddRange(WithKing(board, wKingPos, bKingPos, isWhite));

            if (isWhite)
            {
                foreach (var field in BoardInformations.InsideBoard)
                {
                    if (board[field] == FieldType.WhiteRock)
                        result.AddRange(WithPiece(board, field, RockSteps, isWhite));
                    if (board[field] == FieldType.WhiteBishop)
                        result.AddRange(WithPiece(board, field, BishopSteps, isWhite));
                    if (board[field] == FieldType.WhiteQueen)
                        result.AddRange(WithPiece(board, field, QueenSteps, isWhite));
                    if (board[field] == FieldType.WhiteKnight)
                        result.AddRange(WithKnight(board, field, isWhite));
                }
            }
            else
            {
                foreach (var field in BoardInformations.InsideBoard)
                {
                    if (board[field] == FieldType.BlackRock)
                        result.AddRange(WithPiece(board, field, RockSteps, isWhite));
                    if (board[field] == FieldType.BlackBishop)
                        result.AddRange(WithPiece(board, field, BishopSteps, isWhite));
                    if (board[field] == FieldType.BlackQueen)
                        result.AddRange(WithPiece(board, field, QueenSteps, isWhite));
                    if (board[field] == FieldType.BlackKnight)
                        result.AddRange(WithKnight(board, field, isWhite));
                }
            }

            return result.ToArray();
        }

        public static Dictionary<byte, PiecesAndSteps> StepsForAllPiece(FieldType[] board, bool isWhite = true)
        {
            Dictionary<byte, PiecesAndSteps> pSteps = new Dictionary<byte, PiecesAndSteps>();
            Dictionary<byte, PiecesAndSteps> result = new Dictionary<byte, PiecesAndSteps>();
            if (isWhite)
                foreach (var field in BoardInformations.InsideBoard)
                {
                    if (BoardInformations.WhitePieces.Contains(board[field]))
                    {
                        pSteps.Add(field, new PiecesAndSteps(board[field], Steps(board, field)));
                    }
                }
            else
                foreach (var field in BoardInformations.InsideBoard)
                {
                    if (BoardInformations.BlackPieces.Contains(board[field]))
                    {
                        pSteps.Add(field, new PiecesAndSteps(board[field], Steps(board, field, false)));
                    }
                }

            List<byte> newSteps;
            byte kingPos;
            foreach (var piece in pSteps)
            {
                newSteps = new List<byte>();
                foreach (var step in piece.Value.Steps)
                {
                    var newBoard = new FieldType[144];
                    Array.Copy(board, newBoard, 144);
                    newBoard[piece.Key] = FieldType.Empty;
                    newBoard[step] = piece.Value.FieldType;
                    kingPos = WhereIsTheKing(newBoard, isWhite);
                    if (!AllPiece(newBoard, !isWhite).Contains(kingPos))
                        newSteps.Add(step);
                }
                result.Add(piece.Key, new PiecesAndSteps(board[piece.Key], newSteps.ToArray()));
            }
            return result;
        }

        public static bool IsOtherKingNear(FieldType[] board, byte king1, byte king2)
        {
            foreach (var item in KingSteps)
            {
                if (king1 + item == king2)
                    return true;
            }
            return false;
        }

        public static int[] KingSteps = new int[] { -13, -12, -11, -1, 1, 11, 12, 13 };
        public static int[] RockSteps = new int[] { -12, -1, 1, 12 };
        public static int[] KnightSteps = new int[] { -25, -23, -14, -10, 10, 14, 23, 25 };
        public static int[] BishopSteps = new int[] { -13, -11, 11, 13 };
        public static int[] QueenSteps = new int[] { -13, -12, -11, -1, 1, 12, 11, 13 };

        public static byte WhereIsTheKing(FieldType[] board, bool isWhite = true)
        {
            return (byte)Array.IndexOf(board, isWhite ? FieldType.WhiteKing : FieldType.BlackKing);
        }
    }

    public class PiecesAndSteps
    {
        public PiecesAndSteps()
        {

        }
        public PiecesAndSteps(FieldType _fieldType, byte[] _steps)
        {
            FieldType = _fieldType;
            Steps = _steps;
        }
        public FieldType FieldType { get; set; }
        public byte[] Steps { get; set; }
    }

}
