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
                    FieldType[] newBoard = new FieldType[144];
                    Array.Copy(board, newBoard, 144);
                    if (!BoardInformations.Frame.Contains(newPos) && (board[newPos] == FieldType.Empty || (byte)board[newPos] > 8) && !IsOtherKingNear(board, newPos, bKingPos, isWhite) /*&& !IsCheck(newBoard, newPos)*/)
                        result.Add(newPos);
                }
            }
            else //BLACK
                foreach (var item in KingSteps)
                {
                    newPos = (byte)(bKingPos - item);
                    //FieldType 2-8 white people
                    FieldType[] newBoard = new FieldType[144];
                    Array.Copy(board, newBoard, 144);
                    if (!BoardInformations.Frame.Contains(newPos) && (board[newPos] == FieldType.Empty || ((byte)board[newPos] > 2 && (byte)board[newPos] < 8)) && !IsOtherKingNear(board, wKingPos, newPos, isWhite) /*&& !IsCheck(newBoard, newPos, false)*/)
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
                            for (int i = 1; i < 8; i++)
                            {
                                newPos = (rockPos + item * i);
                                if (newPos >= 0)
                                {
                                    if (board[newPos] == FieldType.Frame)
                                        break;
                                    if (board[newPos] == FieldType.Empty)
                                    {
                                        result.Add((byte)newPos);
                                        continue;
                                    }
                                    if (!BoardInformations.Frame.Contains((byte)newPos) && (byte)board[newPos] > 7)
                                    {
                                        result.Add((byte)newPos);
                                        break;
                                    }
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
                                    if (board[newPos] == FieldType.Empty)
                                    {
                                        result.Add((byte)newPos);
                                        continue;
                                    }
                                    if (!BoardInformations.Frame.Contains((byte)newPos) && ((byte)board[newPos] > 1 && (byte)board[newPos] < 8))
                                    {
                                        result.Add((byte)newPos);
                                        break;
                                    }
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

        //static Dictionary<byte, byte[]> StepsForAllPiece(FieldType[] board)
        //{
        //    Dictionary<byte, byte[]> pSteps = new Dictionary<byte, byte[]>();

        //    foreach (var item in BoardInformations.InsideBoard)
        //    {
        //        if(pSteps.)
        //    }
        //}

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

        //static bool IsCheck(FieldType[] board, byte kingPos, bool isWhite = true)
        //{
        //    //TODO Bishop, Knight, Queeen, Pawn

        //    if (isWhite)
        //    {
        //        if (WithRock(board, false).Contains(kingPos))
        //            return true;
        //    }
        //    else
        //    {
        //        if (WithRock(board).Contains(kingPos))
        //            return true;
        //    }

        //    return false;
        //}


        public static int[] KingSteps = new int[] { -13, -12, -11, -1, 1, 11, 12, 13 };
        public static int[] RockSteps = new int[] { -12, -1, 1, 12 };
    }
}
