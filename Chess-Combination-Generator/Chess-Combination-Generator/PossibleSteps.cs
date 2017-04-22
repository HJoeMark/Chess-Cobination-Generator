using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator
{ 
    public static class PossibleSteps
    {      
        #region With Position

        public static byte[] WithKing(FieldType[] board, byte kingPos, bool isWhite = true)
        {
            //TODO 0-0 0-0-0
            List<byte> result = new List<byte>();
            byte newPos;
            if (isWhite)
            {
                foreach (var item in KingSteps)
                {
                    newPos = (byte)(kingPos - item);
                    if ((board[newPos] == FieldType.Empty || BoardInformations.BlackPieces.Contains(board[newPos])))
                        result.Add(newPos);
                }
            }
            else
                foreach (var item in KingSteps)
                {
                    newPos = (byte)(kingPos - item);
                    if ((board[newPos] == FieldType.Empty || BoardInformations.WhitePieces.Contains(board[newPos])))
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
                        if (/*board[newPos] != FieldType.Frame &&*/ (/*((byte)board[newPos] > 7)*/BoardInformations.BlackPieces.Contains(board[newPos]) || board[newPos] == FieldType.Empty))
                            result.Add((byte)newPos);
                }
            else
                foreach (var item in KnightSteps)
                {
                    newPos = (knightPos + item);
                    if (newPos >= 0)
                        if (/*board[newPos] != FieldType.Frame && */(/*((byte)board[newPos] > 1 && (byte)board[newPos] < 8)*/BoardInformations.WhitePieces.Contains(board[newPos]) || board[newPos] == FieldType.Empty))
                            result.Add((byte)newPos);
                }

            return result.ToArray();
        }

        public static byte[] WithPiece(FieldType[] board, byte piecePos, HashSet<int> steps, bool isWhite = true)
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
                        if (BoardInformations.BlackPieces.Contains(board[newPos]))
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
                        if (BoardInformations.WhitePieces.Contains(board[newPos]))
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

        public static byte[] WithPawn(FieldType[] board, byte pawnPos, bool isWhite = true)
        {
            //TODO n'passant
            List<byte> result = new List<byte>();
            byte newPos;
            if (isWhite)
            {
                newPos = (byte)(pawnPos - 12);
                if (board[newPos] == FieldType.Empty)
                {
                    result.Add(newPos);
                    newPos -= 12;
                    if (BoardInformations.WhitePawnBasicState.Contains(pawnPos) && board[newPos] == FieldType.Empty)
                        result.Add(newPos);
                }
                newPos = (byte)(pawnPos - 13);
                if (BoardInformations.BlackPieces.Contains(board[newPos]))
                    result.Add(newPos);
                newPos = (byte)(pawnPos - 11);
                if (BoardInformations.BlackPieces.Contains(board[newPos]))
                    result.Add(newPos);
            }
            else
            {
                newPos = (byte)(pawnPos + 12);
                if (board[newPos] == FieldType.Empty)
                {
                    result.Add(newPos);
                    newPos += 12;
                    if (BoardInformations.BlackPawnBasicState.Contains(pawnPos) && board[newPos] == FieldType.Empty)
                        result.Add(newPos);
                }
                newPos = (byte)(pawnPos + 13);
                if (BoardInformations.WhitePieces.Contains(board[newPos]))
                    result.Add(newPos);
                newPos = (byte)(pawnPos + 11);
                if (BoardInformations.WhitePieces.Contains(board[newPos]))
                    result.Add(newPos);
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
                case FieldType.WhiteRook:
                    result = WithPiece(board, figPos, RookSteps);
                    break;
                case FieldType.WhiteKnight:
                    result = WithKnight(board, figPos);
                    break;
                case FieldType.WhiteBishop:
                    result = WithPiece(board, figPos, BishopSteps);
                    break;
                case FieldType.WhitePawn:
                    result = WithPawn(board, figPos);
                    break;
                case FieldType.BlackKing:
                    result = WithKing(board, figPos, false);
                    break;
                case FieldType.BlackQueen:
                    result = WithPiece(board, figPos, QueenSteps, false);
                    break;
                case FieldType.BlackRook:
                    result = WithPiece(board, figPos, RookSteps, false);
                    break;
                case FieldType.BlackKnight:
                    result = WithKnight(board, figPos, false);
                    break;
                case FieldType.BlackBishop:
                    result = WithPiece(board, figPos, BishopSteps, false);
                    break;
                case FieldType.BlackPawn:
                    result = WithPawn(board, figPos, false);
                    break;
                default:
                    break;
            }
            return result;
        }

        public static byte[] AllPiece(FieldType[] board, bool isWhite = true)
        {
            var wKingPos = WhereIsTheKing(board);
            var bKingPos = WhereIsTheKing(board, false);
            //Smash the king
            if (bKingPos > 144 || wKingPos > 144)
                return new List<byte>().ToArray();
            List<byte> result = new List<byte>();
            //result.AddRange(WithKing(board, wKingPos, bKingPos, isWhite));

            if (isWhite)
            {
                foreach (var field in BoardInformations.InsideBoard)
                {
                    if (board[field] == FieldType.WhiteRook)
                        result.AddRange(WithPiece(board, field, RookSteps, isWhite));
                    if (board[field] == FieldType.WhiteBishop)
                        result.AddRange(WithPiece(board, field, BishopSteps, isWhite));
                    if (board[field] == FieldType.WhiteQueen)
                        result.AddRange(WithPiece(board, field, QueenSteps, isWhite));
                    if (board[field] == FieldType.WhiteKnight)
                        result.AddRange(WithKnight(board, field, isWhite));
                    if (board[field] == FieldType.WhitePawn)
                        result.AddRange(WithPawn(board, field, isWhite));
                    if (board[field] == FieldType.WhiteKing)
                        result.AddRange(WithKing(board, field, isWhite));
                }
            }
            else
            {
                foreach (var field in BoardInformations.InsideBoard)
                {
                    if (board[field] == FieldType.BlackRook)
                        result.AddRange(WithPiece(board, field, RookSteps, isWhite));
                    if (board[field] == FieldType.BlackBishop)
                        result.AddRange(WithPiece(board, field, BishopSteps, isWhite));
                    if (board[field] == FieldType.BlackQueen)
                        result.AddRange(WithPiece(board, field, QueenSteps, isWhite));
                    if (board[field] == FieldType.BlackKnight)
                        result.AddRange(WithKnight(board, field, isWhite));
                    if (board[field] == FieldType.BlackPawn)
                        result.AddRange(WithPawn(board, field, isWhite));
                    if (board[field] == FieldType.BlackKing)
                        result.AddRange(WithKing(board, field, isWhite));
                }
            }

            return result.ToArray();
        }

        public static Dictionary<FieldAndType, PiecesAndSteps> StepsForAllPiece(FieldType[] board, bool isWhite = true)
        {
            Dictionary<FieldAndType, PiecesAndSteps> pSteps = new Dictionary<FieldAndType, PiecesAndSteps>();
            Dictionary<FieldAndType, PiecesAndSteps> result = new Dictionary<FieldAndType, PiecesAndSteps>();
            if (isWhite)
            {
                foreach (var field in BoardInformations.InsideBoard)
                    if (BoardInformations.WhitePieces.Contains(board[field]))
                        if (board[field] != FieldType.WhitePawn || !BoardInformations.BlackPawnBasicState.Contains(field))
                            pSteps.Add(new FieldAndType(field, board[field]), new PiecesAndSteps(board[field], Steps(board, field)));
                        else
                            for (int i = 3; i < 7; i++)
                                pSteps.Add(new FieldAndType(field, (FieldType)i), new PiecesAndSteps((FieldType)i, Steps(board, field)));
            }
            else
                foreach (var field in BoardInformations.InsideBoard)
                    if (BoardInformations.BlackPieces.Contains(board[field]))
                        if (board[field] != FieldType.BlackPawn || !BoardInformations.WhitePawnBasicState.Contains(field))
                            pSteps.Add(new FieldAndType(field, board[field]), new PiecesAndSteps(board[field], Steps(board, field)));
                        else
                            for (int i = 9; i < 13; i++)
                                pSteps.Add(new FieldAndType(field, (FieldType)i), new PiecesAndSteps((FieldType)i, Steps(board, field)));

            List<byte> newSteps;
            byte kingPos;
            foreach (var piece in pSteps)
            {
                newSteps = new List<byte>();
                foreach (var step in piece.Value.Steps)
                {
                    var newBoard = new FieldType[144];
                    Array.Copy(board, newBoard, 144);
                    newBoard[piece.Key.Field] = FieldType.Empty;
                    newBoard[step] = piece.Value.FieldType;
                    kingPos = WhereIsTheKing(newBoard, isWhite);
                    if (!AllPiece(newBoard, !isWhite).Contains(kingPos))
                        newSteps.Add(step);
                }
                //   if (newSteps.Count() > 0)
                result.Add(piece.Key, new PiecesAndSteps(board[piece.Key.Field], newSteps.ToArray()));
            }
            return result;
        }

        public static bool IsOtherKingNear(byte king1, byte king2)
        {
            foreach (var step in KingSteps)
                if (king1 + step == king2)
                    return true;
            return false;
        }

        public static HashSet<int> KingSteps = new HashSet<int> { -13, -12, -11, -1, 1, 11, 12, 13 };
        public static HashSet<int> RookSteps = new HashSet<int> { -12, -1, 1, 12 };
        public static HashSet<int> KnightSteps = new HashSet<int> { -25, -23, -14, -10, 10, 14, 23, 25 };
        public static HashSet<int> BishopSteps = new HashSet<int> { -13, -11, 11, 13 };
        public static HashSet<int> QueenSteps = new HashSet<int> { -13, -12, -11, -1, 1, 12, 11, 13 };

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


    public class FieldAndType
    {
        public FieldAndType()
        {

        }
        public FieldAndType(byte _field, FieldType _type)
        {
            Field = _field;
            Type = _type;
        }
        public byte Field { get; set; }
        public FieldType Type { get; set; }
    }
}
