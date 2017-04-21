using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator
{
    public static class Memory
    {
        public static BoardNode Root;
    }

    public class BoardAndPossibleStep
    {
        public FieldType[] Board { get; set; }
        public HashSet<StepAndValue> Steps { get; set; }
    }


    public class BoardNode
    {
        public FieldType[] Board { get; set; }
        //  public StepAndValue Step { get; set; }
        public HashSet<BoardNode> Nodes { get; set; }
    }
}
