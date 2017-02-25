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

        //Something wrong with this
        public static int AlphaBeta(FieldType[] boardNode, int depth, int alpha, int beta, bool maximizinPlayer, StepAndValue sAv)
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
                    sAv.Children.Add(new StepAndValue());
                    Array.Copy(boardNode, newBoard, 144);
                    newBoard[step.From] = FieldType.Empty;
                    newBoard[step.Where] = step.What;
                    sAv.Children[sAv.Children.Count() - 1].What = step.What;
                    sAv.Children[sAv.Children.Count() - 1].Where = step.Where;
                    sAv.Children[sAv.Children.Count() - 1].From = step.From;
                    sAv.Children[sAv.Children.Count() - 1].SetParentDepth(depth - 1);
                    var ab = AlphaBeta(newBoard, depth - 1, alpha, beta, false, sAv.Children[sAv.Children.Count() - 1]);
                    v = Math.Max(v, ab);
                    alpha = Math.Max(alpha, v);
                    sAv.Children[sAv.Children.Count() - 1].EvaluatedValue = ab;
                    if (beta <= alpha)
                        break;
                }
                // sAv.EvaluatedValue = sAv.Children.Count() > 0 ? sAv.Children.First(x => x.EvaluatedValue == sAv.Children.Max(y => y.EvaluatedValue)).EvaluatedValue : sAv.EvaluatedValue;
                return v;
            }
            else
            {
                var v = int.MaxValue;
                foreach (var step in AllNode(boardNode, maximizinPlayer))
                {
                    var newBoard = new FieldType[144];
                    sAv.Children.Add(new StepAndValue());
                    Array.Copy(boardNode, newBoard, 144);
                    newBoard[step.From] = FieldType.Empty;
                    sAv.Children[sAv.Children.Count() - 1].What = newBoard[step.Where] = step.What;
                    sAv.Children[sAv.Children.Count() - 1].Where = step.Where;
                    sAv.Children[sAv.Children.Count() - 1].From = step.From;
                    sAv.Children[sAv.Children.Count() - 1].SetParentDepth(depth - 1);
                    var ab = AlphaBeta(newBoard, depth - 1, alpha, beta, true, sAv.Children[sAv.Children.Count() - 1]);
                    v = Math.Min(v, ab);
                    beta = Math.Min(beta, v);
                    sAv.Children[sAv.Children.Count() - 1].EvaluatedValue = ab;
                    if (beta <= alpha)
                        break;
                }
                // sAv.EvaluatedValue = sAv.Children.Count() > 0 ? sAv.Children.First(x => x.EvaluatedValue == sAv.Children.Max(y => y.EvaluatedValue)).EvaluatedValue : sAv.EvaluatedValue;
                return v;
            }
        }

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
        
        static List<StepAndValue> AllNode(FieldType[] board, bool isWhite = true)
        {
            var result = new List<StepAndValue>();
            foreach (var piece in PossibleSteps.StepsForAllPiece(board, isWhite))
                foreach (var step in piece.Value.Steps)
                    result.Add(new StepAndValue(piece.Key.Field, step, piece.Key.Type, 0, null));
            return result;
        }
    }

    public class StepAndValue
    {
        public StepAndValue()
        {
            Children = new List<StepAndValue>();
        }

        public StepAndValue(byte _from, byte _where, FieldType _what, int _evaluatedValue, List<StepAndValue> _children/*, int _depth = 0*/)
        {
            EvaluatedValue = _evaluatedValue;
            Children = _children;
            //Depth = _depth;
            From = _from;
            Where = _where;
            What = _what;
        }
        public int Depth;
        public int EvaluatedValue;
        public List<StepAndValue> Children;
        public byte From;
        public byte Where;
        public FieldType What;

        public StepAndValue Parent;

        public void SetParentDepth(int depth)
        {
            if (Parent == null)
                this.Depth = depth;
            else
            {
                if (this.Parent.Depth > depth)
                    this.Parent.SetParentDepth(depth);
            }
        }
    }

}
