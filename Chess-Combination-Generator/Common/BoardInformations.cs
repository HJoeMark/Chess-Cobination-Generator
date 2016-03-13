﻿using System;
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

        public static byte WhiteKingPosition;
        public static byte BlackKingPosition;

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

    }
}
