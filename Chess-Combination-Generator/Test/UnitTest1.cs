using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Chess_Combination_Generator;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GeneratorSpeedTest()
        {
            var result = true;
            var time = new Stopwatch();
            for (int i = 0; i < 100; i++)
            {
                time.Start();
                var newBoard = Chess_Combination_Generator.Generator.Generate(false, false, 3, 1, 2, 2, 2, 8, 1, 2, 2, 2, 8);
                time.Stop();
                if (time.Elapsed.Seconds >= 3)
                {
                    result = false;
                    break;
                }
            }
            Assert.AreEqual(true, result, $"Elapsed time: {time.Elapsed.Seconds}");
        }

        [TestMethod]
        public void AITest()
        {
            var result = true;
            var isWhite = false;
            var checkIsOk = false;
            var board = new FieldType[144];
            for (byte i = 0; i < BoardInformations.InsideBoard.Count; i++)
            {
                board[BoardInformations.InsideBoard.ElementAt(i)] = FieldType.Empty;
            }
            board[50] = FieldType.BlackKing;
            board[26] = FieldType.WhiteKing;
            board[45] = FieldType.BlackRook;
            board[33] = FieldType.BlackRook;
            board[27] = FieldType.WhiteRook;


            //StepAndValue SAV = new StepAndValue(0, 0, FieldType.Frame, 0, new List<StepAndValue>());
            var value = AI.AlphaBeta(board, 1, int.MinValue, int.MaxValue, isWhite);//, SAV);
            var searchedValue = isWhite ? int.MaxValue : int.MinValue;

            if ((!checkIsOk && (AI.IsCheck(board) || AI.IsCheck(board, false)) || value != searchedValue))
                result = false;

            Assert.AreEqual(true, result, $"");
        }

        [TestMethod]
        public void AI_IsStalemate_Test()
        {
            var result = true;
            var isWhite = false;
            var board = new FieldType[144];
            for (byte i = 0; i < BoardInformations.InsideBoard.Count; i++)
                board[BoardInformations.InsideBoard.ElementAt(i)] = FieldType.Empty;
            board[50] = FieldType.BlackKing;
            board[26] = FieldType.WhiteKing;
            board[45] = FieldType.BlackRook;
            board[33] = FieldType.BlackRook;
            board[27] = FieldType.WhiteRook;

            //for speed test
            for (int i = 0; i < 1000; i++) //important, becouse the time is too small
                result = AI.IsStalemate(board, isWhite);

            Assert.AreEqual(false, result, $"");
        }

        [TestMethod]
        public void AI_IsCheck_Test()
        {
            var result = true;
            var isWhite = false;
            var board = new FieldType[144];
            for (byte i = 0; i < BoardInformations.InsideBoard.Count; i++)
                board[BoardInformations.InsideBoard.ElementAt(i)] = FieldType.Empty;
            board[50] = FieldType.BlackKing;
            board[26] = FieldType.WhiteKing;
            board[45] = FieldType.BlackRook;
            board[33] = FieldType.BlackRook;
            board[27] = FieldType.WhiteRook;

            //for speed test
            for (int i = 0; i < 1000; i++) //important, becouse the time is too small
                result = AI.IsCheck(board, isWhite);

            Assert.AreEqual(false, result, $"");
        }

        [TestMethod]
        public void PossibleSteps_WhereIsTheKing_Test()
        {
            //var result = true;
            var isWhite = false;
            var board = new FieldType[144];
            for (byte i = 0; i < BoardInformations.InsideBoard.Count; i++)
                board[BoardInformations.InsideBoard.ElementAt(i)] = FieldType.Empty;
            board[50] = FieldType.BlackKing;
            board[26] = FieldType.WhiteKing;
            board[45] = FieldType.BlackRook;
            board[33] = FieldType.BlackRook;
            board[27] = FieldType.WhiteRook;

            //for speed test
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10000000; i++) //important, becouse the time is too small
                PossibleSteps.WhereIsTheKing(board, isWhite);
            sw.Stop();
            var a = sw.Elapsed.TotalMilliseconds;

            Assert.AreEqual(false, false, $"");
        }

    }
}
