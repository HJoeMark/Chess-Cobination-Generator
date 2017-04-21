using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator
{
    public static class AI
    {
        public static bool IsCheck(FieldType[] board, bool isWhite = true)
        {
            return PossibleSteps.StepsForAllPiece(board, !isWhite).SelectMany(x => x.Value.Steps).Contains(PossibleSteps.WhereIsTheKing(board, isWhite));
        }

        public static bool IsStalemate(FieldType[] board, bool isWhite = true)
        {
            return PossibleSteps.StepsForAllPiece(board, isWhite).SelectMany(x => x.Value.Steps).Count() == 0;
        }

        public static bool IsStalemate2(BoardNode boardNode, bool isWhite = true)
        {
            return boardNode.Nodes != null ? boardNode.Nodes.SelectMany(x => x.Board).Count() == 0 : PossibleSteps.StepsForAllPiece(boardNode.Board, isWhite).SelectMany(x => x.Value.Steps).Count() == 0;
        }


        //https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning
        //01 function alphabeta(node, depth, α, β, maximizingPlayer)
        //02      if depth = 0 or node is a terminal node
        //03          return the heuristic value of node
        //04      if maximizingPlayer
        //05          v := -∞
        //06          for each child of node
        //07              v := max(v, alphabeta(child, depth - 1, α, β, FALSE))
        //08              α := max(α, v)
        //09              if β ≤ α
        //10                  break (* β cut-off*)
        //11          return v
        //12      else
        //13          v := ∞
        //14          for each child of node
        //15              v := min(v, alphabeta(child, depth - 1, α, β, TRUE))
        //16              β := min(β, v)
        //17              if β ≤ α
        //18                  break (* α cut-off*)
        //19          return v



        public static int AlphaBeta(FieldType[] boardNode, int depth, int alpha, int beta, bool maximizinPlayer)
        {
            if (IsStalemate(boardNode, maximizinPlayer))
            {
                if (IsCheck(boardNode, maximizinPlayer))
                    return !maximizinPlayer ? int.MaxValue : int.MinValue;
                else
                    return Evaluator.Evaluate(boardNode, maximizinPlayer);
            }

            if (depth == 0)
                return Evaluator.Evaluate(boardNode, maximizinPlayer);

            if (maximizinPlayer)
            {
                var v = int.MinValue;
                foreach (var step in AllNode(boardNode, maximizinPlayer))
                {
                    var newBoard = new FieldType[144];
                    Array.Copy(boardNode, newBoard, 144);
                    newBoard[step.From] = FieldType.Empty;
                    newBoard[step.Where] = step.What;
                    var ab = AlphaBeta(newBoard, depth - 1, alpha, beta, false);
                    v = Math.Max(v, ab);
                    alpha = Math.Max(alpha, v);
                    if (beta <= alpha)
                        break;
                }
                return v;
            }
            else
            {
                var v = int.MaxValue;
                foreach (var step in AllNode(boardNode, maximizinPlayer))
                {
                    var newBoard = new FieldType[144];
                    Array.Copy(boardNode, newBoard, 144);
                    newBoard[step.From] = FieldType.Empty;
                    newBoard[step.Where] = step.What;
                    var ab = AlphaBeta(newBoard, depth - 1, alpha, beta, true);
                    v = Math.Min(v, ab);
                    beta = Math.Min(beta, v);
                    if (beta <= alpha)
                        break;
                }
                return v;
            }
        }



        public static Dictionary<BoardNode, int> ListOfPossibleSteps;
        public static int StartDepth;
        public static int AlphaBeta2(BoardNode boardNode, int depth, int alpha, int beta, bool maximizinPlayer)
        {
            //02      if depth = 0 or node is a terminal node
            //03          return the heuristic value of node
            if (IsStalemate2(boardNode, maximizinPlayer))
            {
                if (IsCheck(boardNode.Board, maximizinPlayer))
                    return !maximizinPlayer ? int.MaxValue : int.MinValue;
                else
                    return Evaluator.Evaluate(boardNode.Board, maximizinPlayer);
            }

            if (depth == 0)
                return Evaluator.Evaluate(boardNode.Board, maximizinPlayer);
            //04      if maximizingPlayer
            if (maximizinPlayer)
            {
                //05          v := -∞
                //06          for each child of node
                //07              v := max(v, alphabeta(child, depth - 1, α, β, FALSE))
                //08              α := max(α, v)
                //09              if β ≤ α
                //10                  break (* β cut-off*)
                //11          return v
                var v = int.MinValue;


                if (boardNode.Nodes == null)
                {
                    boardNode.Nodes = new HashSet<BoardNode>();
                    foreach (var board in AllBoardNode(boardNode.Board, maximizinPlayer))
                        boardNode.Nodes.Add(new BoardNode() { Board = board });
                }

                foreach (var step in boardNode.Nodes)
                {
                    var ab = AlphaBeta2(step, depth - 1, alpha, beta, false);
                    v = Math.Max(v, ab);
                    alpha = Math.Max(alpha, v);

                    if (StartDepth == depth)
                    {
                        ListOfPossibleSteps.Add(step, ab);
                    }


                    if (beta <= alpha)
                        break;
                }
                return v;
            }
            //12      else
            else
            {

                //13          v := ∞
                //14          for each child of node
                //15              v := min(v, alphabeta(child, depth - 1, α, β, TRUE))
                //16              β := min(β, v)
                //17              if β ≤ α
                //18                  break (* α cut-off*)
                //19          return v


                var v = int.MaxValue;

                if (boardNode.Nodes == null)
                {
                    boardNode.Nodes = new HashSet<BoardNode>();
                    foreach (var board in AllBoardNode(boardNode.Board, maximizinPlayer))
                        boardNode.Nodes.Add(new BoardNode() { Board = board });
                }

                foreach (var step in boardNode.Nodes)
                {
                    var ab = AlphaBeta2(step, depth - 1, alpha, beta, true);
                    v = Math.Min(v, ab);
                    beta = Math.Min(beta, v);

                    if (StartDepth == depth)
                    {
                        ListOfPossibleSteps.Add(step, ab);
                    }

                    if (beta <= alpha)
                        break;
                }
                return v;
            }
        }


        static HashSet<StepAndValue> AllNode(FieldType[] board, bool isWhite = true)
        {
            var result = new HashSet<StepAndValue>();
            foreach (var piece in PossibleSteps.StepsForAllPiece(board, isWhite))
                foreach (var step in piece.Value.Steps)
                    result.Add(new StepAndValue(piece.Key.Field, step, piece.Key.Type));
            return result;
        }

        public static HashSet<FieldType[]> AllBoardNode(FieldType[] board, bool isWhite = true)
        {
            var result = new HashSet<FieldType[]>();
            foreach (var piece in PossibleSteps.StepsForAllPiece(board, isWhite))
                foreach (var step in piece.Value.Steps)
                {
                    FieldType[] newBoard = new FieldType[144];
                    Array.Copy(board, newBoard, 144);
                    newBoard[piece.Key.Field] = FieldType.Empty;
                    newBoard[step] = piece.Key.Type;
                    result.Add(newBoard);
                }
            return result;
        }



        //static bool CompareBoards(FieldType[] board1, FieldType[] board2)
        //{
        //    foreach (var index in BoardInformations.InsideBoard)
        //    {
        //        if (board1[index] != board2[index])
        //            return false;
        //    }
        //    return true;
        //}
    }

    public class StepAndValue
    {
        public StepAndValue()
        {
        }

        public StepAndValue(byte _from, byte _where, FieldType _what)
        {
            From = _from;
            Where = _where;
            What = _what;
        }

        public byte From;
        public byte Where;
        public FieldType What;


    }

}
