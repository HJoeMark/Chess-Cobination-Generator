using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator
{
    public static class Init
    {
        public static void BasicPosition()
        {
            BoardInformations.CurrentPosition = new FieldType[144];
            for (byte i = 0; i < BoardInformations.InsideBoard.Length; i++)
            {
                BoardInformations.CurrentPosition[BoardInformations.InsideBoard[i]] = FieldType.Empty;
            }

            //BoardInformations.WhiteKingPosition = 26;
            //BoardInformations.BlackKingPosition = 50;

            //BoardInformations.CurrentPosition[50] = FieldType.BlackKing;
            //BoardInformations.CurrentPosition[26] = FieldType.WhiteKing;
            //BoardInformations.CurrentPosition[45] = FieldType.BlackRock;
            //BoardInformations.CurrentPosition[33] = FieldType.BlackRock;
            //BoardInformations.CurrentPosition[27] = FieldType.WhiteRock;
        }


        public static void ClearBoard(ref FieldType[] board)
        {
            board = new FieldType[144];
            for (byte i = 0; i < BoardInformations.InsideBoard.Length; i++)
                board[BoardInformations.InsideBoard[i]] = FieldType.Empty;
        }
    }
}
