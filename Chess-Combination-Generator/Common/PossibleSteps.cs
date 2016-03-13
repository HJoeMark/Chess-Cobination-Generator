using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class PossibleSteps
    {
        /// <summary>
        /// King possible steps
        /// </summary>
        /// <param name="board">the current board</param>
        /// <param name="isWhite">who is comming</param>
        /// <returns>the fields, where the king can move</returns>
        public static byte[] WithKing(FieldType[] board, byte? wKingPos, byte? bKingPos, bool isWhite = true)
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
                    if (!BoardInformations.Frame.Contains(newPos) && (board[newPos] == FieldType.Empty || (byte)board[newPos] > 8) && !IsOtherKingNear(board, newPos, bKingPos, isWhite))
                        result.Add(newPos);
                }
            }
            else //BLACK
                foreach (var item in KingSteps)
                {
                    newPos = (byte)(bKingPos - item);
                    //FieldType 2-8 white people
                    if (!BoardInformations.Frame.Contains(newPos) && (board[newPos] == FieldType.Empty || ((byte)board[newPos] > 2 && (byte)board[newPos] < 8)) && !IsOtherKingNear(board, wKingPos, newPos, isWhite))
                        result.Add(newPos);
                }

            return result.ToArray();
        }

        public static byte[] WithRock(FieldType[] board, bool isWhite = true)
        {
            List<byte> result = new List<byte>();
            List<byte> rocks = new List<byte>();

            if (isWhite)
                foreach (var field in BoardInformations.InsideBoard)
                {
                    if (board[field] == FieldType.WhiteRock)
                        rocks.Add(field);
                }
            else
                foreach (var field in BoardInformations.InsideBoard)
                {
                    if (board[field] == FieldType.BlackRock)
                        rocks.Add(field);
                }
            if (rocks.Count() > 0)
            {
                int newPos;
                if (isWhite)
                {
                    foreach (var rockPos in rocks)
                    {
                        foreach (var item in RockSteps)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                newPos = (rockPos + item * i);
                                if (newPos >= 0)
                                {
                                    if (board[newPos] == FieldType.Frame)
                                        break;
                                    if (!BoardInformations.Frame.Contains((byte)newPos) && (board[newPos] == FieldType.Empty || (byte)board[newPos] > 8))
                                        result.Add((byte)newPos);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var rockPos in rocks)
                    {
                        foreach (var item in RockSteps)
                        {
                            for (int i = 1; i < 8; i++)
                            {
                                newPos = (rockPos + item * i);
                                if (newPos >= 0)
                                {
                                    if (board[newPos] == FieldType.Frame)
                                        break;
                                    if (!BoardInformations.Frame.Contains((byte)newPos) && (board[newPos] == FieldType.Empty || ((byte)board[newPos] > 2 && (byte)board[newPos] < 8)))
                                        result.Add((byte)newPos);
                                }
                            }
                        }
                    }
                }
            }
            return result.ToArray();
        }

        public static byte[] AllPiece(FieldType[] board, bool isWhite = true)
        {
            var wKingPos = SearchKing(board);
            var bKingPos = SearchKing(board, false);

            if (wKingPos == null || bKingPos == null)
                throw new Exception(String.Format("No {0} king", wKingPos == null ? (bKingPos == null ? "both" : "white") : "black"));

            List<byte> result = new List<byte>();
            result.AddRange(WithKing(board, wKingPos, bKingPos, isWhite));
            result.AddRange(WithRock(board, isWhite));
            return result.ToArray();
        }

        private static bool IsOtherKingNear(FieldType[] board, byte? wKingPos, byte? bKingPos, bool isWhite = true)
        {
            if (isWhite)
                foreach (var item in KingSteps)
                {
                    if (wKingPos + item == bKingPos)
                        return true;
                }
            else
                foreach (var item in KingSteps)
                {
                    if (wKingPos == bKingPos + item)
                        return true;
                }
            return false;
        }

        static byte? SearchKing(FieldType[] board, bool isWhite = true)
        {
            if (isWhite)
            {
                for (byte i = 0; i < BoardInformations.InsideBoard.Count(); i++)
                    if (board[BoardInformations.InsideBoard[i]] == FieldType.WhiteKing)
                        return BoardInformations.InsideBoard[i];
            }
            else
                for (byte i = 0; i < BoardInformations.InsideBoard.Count(); i++)
                    if (board[BoardInformations.InsideBoard[i]] == FieldType.BlackKing)
                        return BoardInformations.InsideBoard[i];
            return null;
        }


        public static int[] KingSteps = new int[] { -13, -12, -11, -1, 1, 11, 12, 13 };
        public static int[] RockSteps = new int[] { -12, -1, 1, 12 };
    }
}
