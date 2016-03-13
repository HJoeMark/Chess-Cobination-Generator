using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class PossibleSteps
    {
        public static byte[] WithKing(FieldType[] board, bool isWhite = true)
        {
            var wKingPos = SearchKing(board);
            var bKingPos = SearchKing(board, false);

            if (wKingPos == null || bKingPos == null)
                throw new Exception(String.Format("No {0} king", wKingPos == null ? (bKingPos == null ? "both" : "white") : "black"));

            byte[] result = new byte[8];
            if (isWhite) //WHITE
            {
                byte newPos = (byte)(wKingPos - 13);
                //UP
                //-13
                if (!BoardInformations.Frame.Contains(newPos) && board[newPos] == FieldType.Empty)
                { }
                //-12
                //-11

                //LEFT
                //-1
                //RIGHT
                //+1

                //DOWN
                //+11
                //+12
                //+13
            }
            else //BLACK
            {

            }
            return result;
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

    }
}
