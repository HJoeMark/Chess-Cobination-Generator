using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class AI
    {
        public static bool IsCheck(FieldType[] board, bool isWhite = true)
        {
            return PossibleSteps.StepsForAllPiece(board, !isWhite).SelectMany(x => x.Value.Steps).Contains(PossibleSteps.WhereIsTheKing(board, isWhite));
        }

        public static bool IsCheckMate(FieldType[] board, bool isWhite = true)
        {
            return IsCheck(board, isWhite) && PossibleSteps.StepsForAllPiece(board, isWhite).SelectMany(x => x.Value.Steps).Count() == 0;
        }

        public static bool IsStalemate(FieldType[] board, bool isWhite = true)
        {
            return PossibleSteps.StepsForAllPiece(board, isWhite).SelectMany(x => x.Value.Steps).Count() == 0;
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

        public static int AlphaBeta(FieldType[] boardNode, int depth, int alpha, int beta, bool maximizinPlayer, StepAndValue sAv)
        {
            if (IsCheckMate(boardNode, !maximizinPlayer))
                return maximizinPlayer ? int.MaxValue : int.MinValue;

            if (depth == 0 || IsStalemate(boardNode, !maximizinPlayer))
                return Evaluator.Evaluate(boardNode, maximizinPlayer);
           
            if (maximizinPlayer)
            {
                var v = int.MinValue;
                foreach (var piece in PossibleSteps.StepsForAllPiece(boardNode, maximizinPlayer))
                {
                    var newBoard = new FieldType[144];
                    foreach (var step in piece.Value.Steps)
                    {
                        sAv.Children.Add(new StepAndValue());
                        Array.Copy(boardNode, newBoard, 144);
                        newBoard[piece.Key] = FieldType.Empty;
                        newBoard[step] = piece.Value.FieldType;
                        Array.Copy(newBoard, sAv.Children[sAv.Children.Count() - 1].Step = new FieldType[144], 144);
                        var ab = AlphaBeta(newBoard, depth - 1, alpha, beta, false, sAv.Children[sAv.Children.Count() - 1]);
                        v = Math.Max(v, ab);
                        alpha = Math.Max(alpha, v);
                        sAv.Children[sAv.Children.Count() - 1].EvaluatedValue = alpha;
                        if (beta <= alpha)
                            break;
                    }
                }
                SAV = sAv;
                return v;
            }
            else
            {
                var v = int.MaxValue;
                foreach (var piece in PossibleSteps.StepsForAllPiece(boardNode, maximizinPlayer))
                {
                    var newBoard = new FieldType[144];
                    foreach (var step in piece.Value.Steps)
                    {
                        sAv.Children.Add(new StepAndValue());
                        Array.Copy(boardNode, newBoard, 144);
                        newBoard[piece.Key] = FieldType.Empty;
                        newBoard[step] = piece.Value.FieldType;
                        Array.Copy(newBoard, sAv.Children[sAv.Children.Count() - 1].Step = new FieldType[144], 144);
                        var ab = AlphaBeta(newBoard, depth - 1, alpha, beta, true, sAv.Children[sAv.Children.Count() - 1]);
                        v = Math.Min(v, ab);
                        beta = Math.Min(beta, v);
                        sAv.Children[sAv.Children.Count() - 1].EvaluatedValue = beta;
                        if (beta <= alpha)
                            break;
                    }
                }
                SAV = sAv;
                return v;
            }
        }

        //public static bool IsTerminalNode(FieldType[] board, bool isWhite = true)
        //{
        //    return IsCheckMate(board, isWhite);
        //}


        public static StepAndValue SAV;
    }

    public class StepAndValue
    {
        public StepAndValue()
        {
            Children = new List<StepAndValue>();
        }

        public StepAndValue(FieldType[] _step, int _evaluatedValue, List<StepAndValue> _children)
        {
            Step = _step;
            EvaluatedValue = _evaluatedValue;
            Children = _children;
        }
        public FieldType[] Step;
        public int EvaluatedValue;
        public List<StepAndValue> Children;
    }
}
