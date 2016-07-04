using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator.Model
{
    public class GenerationModel
    {
        private bool isWhite;

        public bool IsWhite
        {
            get { return isWhite; }
            set { isWhite = value; }
        }

        private int numberOfCombination;

        public int NumberOfCombination
        {
            get { return numberOfCombination; }
            set { numberOfCombination = value; }
        }

        private int treeLevel;

        public int TreeLevel
        {
            get { return treeLevel; }
            set { treeLevel = value; }
        }

        private PiecesNumber white;

        public PiecesNumber White
        {
            get { return white; }
            set { white = value; }
        }

        private PiecesNumber black;

        public PiecesNumber Black
        {
            get { return black; }
            set { black = value; }
        }
    }

    public class PiecesNumber
    {
        private int queens;
        public int Queens
        {
            get { return queens; }
            set { queens = value; }
        }
        private int rooks;
        public int Rooks
        {
            get { return rooks; }
            set { rooks = value; }
        }
        private int knights;
        public int Knights
        {
            get { return knights; }
            set { knights = value; }
        }
        private int bishops;
        public int Bishops
        {
            get { return bishops; }
            set { bishops = value; }
        }
        private int pawns;
        public int Pawns
        {
            get { return pawns; }
            set { pawns = value; }
        }
    }
}
