using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum FieldType
    {
        Frame,
        Empty,
        WhiteKing,
        WhiteQueen,
        WhiteRock,
        WhiteKnight,
        WhiteBishop,
        WhitePawn,
        BlackKing,
        BlackQueen,
        BlackRock,
        BlackKnight,
        BlackBishop,
        BlackPawn
    }

    public static class BoardInformations
    {
        public static FieldType[] CurrentPosition;

        public static byte[] InsideBoard = new byte[] {
            26,27,28,29,30,31,32,33,
            38,39,40,41,42,43,44,45,
            50,51,52,53,54,55,56,57,
            62,63,64,65,66,67,68,69,
            74,75,76,77,78,79,80,81,
            86,87,88,89,90,91,92,93,
            98,99,100,101,102,103,104,105,
            110,111,112,113,114,115,116,117
        };

        public static byte[] Frame = new byte[] {
            0,1,2,3,4,5,6,7,8,9,10,11,
            12,13,14,15,16,17,18,19,20,21,22,23,
            24,25,34,35,
            36,37,46,47,
            48,49,58,59,
            60,61,70,71,
            72,73,82,83,
            84,85,94,95,
            96,97,106,107,
            108,109,118,119,
            120,121,122,123,124,125,126,127,128,129,130,131,
            132,133,134,135,136,137,138,139,140,141,142,143
        };

        public static byte[] WhiteFields = new byte[] {
            26,28,30,32,
            39,41,43,45,
            50,52,54,56,
            63,65,67,69,
            74,76,78,80,
            87,89,91,93,
            98,100,102,104,
            111,113,115,117
        };

        public static FieldType[] WhitePieces = new FieldType[] { FieldType.WhiteKing, FieldType.WhiteQueen, FieldType.WhiteRock, FieldType.WhiteKnight, FieldType.WhiteBishop, FieldType.WhitePawn };
        public static FieldType[] BlackPieces = new FieldType[] { FieldType.BlackKing, FieldType.BlackQueen, FieldType.BlackRock, FieldType.BlackKnight, FieldType.BlackBishop, FieldType.BlackPawn };

        public static FieldType[] SetEmptyBoard()
        {
            var result = new FieldType[144];
            for (byte i = 0; i < BoardInformations.InsideBoard.Length; i++)
                result[BoardInformations.InsideBoard[i]] = FieldType.Empty;
            return result;
        }

        public static string GetFEN(FieldType[] board, bool isWhite = true)
        {
            var result = "";
            var empty = 0;
            var index = 0;
            foreach (var field in InsideBoard)
            {
                index++;
                if (index > 8)
                {
                    index = 1;
                    result += (empty > 0 ? (empty + "") : "") + "/";
                    empty = 0;
                }
                switch (board[field])
                {
                    case FieldType.Empty:
                        empty++;
                        break;
                    case FieldType.WhiteKing:
                        result += (empty > 0 ? (empty + "") : "") + "K";
                        empty = 0;
                        break;
                    case FieldType.WhiteQueen:
                        result += (empty > 0 ? (empty + "") : "") + "Q";
                        empty = 0;
                        break;
                    case FieldType.WhiteRock:
                        result += (empty > 0 ? (empty + "") : "") + "R";
                        empty = 0;
                        break;
                    case FieldType.WhiteKnight:
                        result += (empty > 0 ? (empty + "") : "") + "N";
                        empty = 0;
                        break;
                    case FieldType.WhiteBishop:
                        result += (empty > 0 ? (empty + "") : "") + "B";
                        empty = 0;
                        break;
                    case FieldType.WhitePawn:
                        result += (empty > 0 ? (empty + "") : "") + "P";
                        empty = 0;
                        break;
                    case FieldType.BlackKing:
                        result += (empty > 0 ? (empty + "") : "") + "k";
                        empty = 0;
                        break;
                    case FieldType.BlackQueen:
                        result += (empty > 0 ? (empty + "") : "") + "q";
                        empty = 0;
                        break;
                    case FieldType.BlackRock:
                        result += (empty > 0 ? (empty + "") : "") + "r";
                        empty = 0;
                        break;
                    case FieldType.BlackKnight:
                        result += (empty > 0 ? (empty + "") : "") + "n";
                        empty = 0;
                        break;
                    case FieldType.BlackBishop:
                        result += (empty > 0 ? (empty + "") : "") + "b";
                        empty = 0;
                        break;
                    case FieldType.BlackPawn:
                        result += (empty > 0 ? (empty + "") : "") + "p";
                        empty = 0;
                        break;
                    default:
                        break;
                }
            }
            result += empty > 0 ? (empty + "") : "";
            //I think the last two member are not important
            return result + (isWhite ? " w" : " b") + " 0" + " 0";
        }
    }
}
