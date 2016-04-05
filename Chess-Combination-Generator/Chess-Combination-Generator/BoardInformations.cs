using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator
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

        public static int[] WhitePawnBasicState = new int[] { 98, 99, 100, 101, 102, 103, 104, 105 };
        public static int[] BlackPawnBasicState = new int[] { 38, 39, 40, 41, 42, 43, 44, 45 };


        public static int[] RowOneEight = new int[] { 26, 27, 28, 29, 30, 31, 32, 33, 110, 111, 112, 113, 114, 115, 116, 117 };


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

        public static void SetBoard(FieldType[] board, string fen)
        {
            var array = fen.Split('/');
            array[array.Length - 1] = array[array.Length - 1].Split(' ')[0];
            var fields = new FieldType[64];

            var index = 0;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    int n;
                    bool isNumeric = int.TryParse(array[i].Substring(j, 1), out n);
                    if (isNumeric)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            fields[index] = FieldType.Empty;
                            index += 1;
                        }
                    }
                    else
                    {
                        switch (array[i].Substring(j, 1))
                        {
                            case "K":
                                fields[index] = FieldType.WhiteKing;
                                index++;
                                break;
                            case "Q":
                                fields[index] = FieldType.WhiteQueen;
                                index++;
                                break;
                            case "R":
                                fields[index] = FieldType.WhiteRock;
                                index++;
                                break;
                            case "B":
                                fields[index] = FieldType.WhiteBishop;
                                index++;
                                break;
                            case "N":
                                fields[index] = FieldType.WhiteKnight;
                                index++;
                                break;
                            case "P":
                                fields[index] = FieldType.WhitePawn;
                                index++;
                                break;
                            case "k":
                                fields[index] = FieldType.BlackKing;
                                index++;
                                break;
                            case "q":
                                fields[index] = FieldType.BlackQueen;
                                index++;
                                break;
                            case "r":
                                fields[index] = FieldType.BlackRock;
                                index++;
                                break;
                            case "b":
                                fields[index] = FieldType.BlackBishop;
                                index++;
                                break;
                            case "n":
                                fields[index] = FieldType.BlackKnight;
                                index++;
                                break;
                            case "p":
                                fields[index] = FieldType.BlackPawn;
                                index++;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            var num = 0;
            foreach (var field in InsideBoard)
            {
                board[field] = fields[num];
                num++;
            }
        }

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


        //public static void ClearBoard(ref FieldType[] board)
        //{
        //    board = new FieldType[144];
        //    for (byte i = 0; i < BoardInformations.InsideBoard.Length; i++)
        //        board[BoardInformations.InsideBoard[i]] = FieldType.Empty;
        //}

    }
}
