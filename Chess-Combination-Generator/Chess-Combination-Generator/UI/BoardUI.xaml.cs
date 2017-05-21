using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess_Combination_Generator.UI
{

    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class BoardUI : UserControl
    {
        public bool IsWhite;
        bool isClickedOnce;
        byte prePos;
        FieldType[] currentPosition;

        private SolidColorBrush whiteField = new SolidColorBrush(Colors.White);
        private SolidColorBrush blackField = new SolidColorBrush(Colors.Gray);


        public BoardUI()
        {
            InitializeComponent();
        }

        public void SetBoard(FieldType[] board, byte[] possibleSteps = null, bool _isWhite = true)
        {
            ClearBoard();
            IsWhite = _isWhite;
            isClickedOnce = false;
            currentPosition = board;

            //RESOURCES
            //https://en.wikipedia.org/wiki/Chess_symbols_in_Unicode
            var index = _isWhite ? -1 : 64;
            foreach (var item in BoardInformations.InsideBoard)
            {
                index += _isWhite ? 1 : -1;
                var field = fields.FindName("f" + index);
                if (board[item] != FieldType.Empty)
                {
                    if (field != null)
                    {
                        switch (board[item])
                        {
                            case FieldType.WhiteKing:
                                ((Label)field).Content = "♔";
                                break;
                            case FieldType.WhiteQueen:
                                ((Label)field).Content = "♕";
                                break;
                            case FieldType.WhiteRook:
                                ((Label)field).Content = "♖";
                                break;
                            case FieldType.WhiteKnight:
                                ((Label)field).Content = "♘";
                                break;
                            case FieldType.WhiteBishop:
                                ((Label)field).Content = "♗";
                                break;
                            case FieldType.WhitePawn:
                                ((Label)field).Content = "♙";
                                break;
                            case FieldType.BlackKing:
                                ((Label)field).Content = "♚";
                                break;
                            case FieldType.BlackQueen:
                                ((Label)field).Content = "♛";
                                break;
                            case FieldType.BlackRook:
                                ((Label)field).Content = "♜";
                                break;
                            case FieldType.BlackKnight:
                                ((Label)field).Content = "♞";
                                break;
                            case FieldType.BlackBishop:
                                ((Label)field).Content = "♝";
                                break;
                            case FieldType.BlackPawn:
                                ((Label)field).Content = "♟";
                                break;
                            default:
                                break;
                        }
                    }
                }
                //#if (DEBUG == true)
                //                else
                //                {
                //                    ((Label)field).Content = BoardInformations.InsideBoard[isWhite ? byte.Parse(((Label)field).Name.Substring(1)) : 63- byte.Parse(((Label)field).Name.Substring(1))];
                //                }
                //#endif
                if (possibleSteps != null && possibleSteps.Contains(item))
                {
                    field = fields.FindName("f" + index);
                    ((Label)field).Background = new SolidColorBrush(Colors.LightGreen);
                }
            }

            if (_isWhite)
            {
                Label label;
                int cIndex = 0;
                foreach (var column in BoardInformations.Columns)
                {
                    cIndex++;
                    label = (Label)columns_grid.FindName("c" + cIndex);
                    label.Content = column;
                }
                for (int i = 1; i < 9; i++)
                {
                    label = (Label)rows_grid.FindName("r" + (9 - i));
                    label.Content = i;
                }

            }
            else
            {
                Label label;
                int cIndex = 9;
                foreach (var column in BoardInformations.Columns)
                {
                    cIndex--;
                    label = (Label)columns_grid.FindName("c" + cIndex);
                    label.Content = column;
                }
                for (int i = 1; i < 9; i++)
                {
                    label = (Label)rows_grid.FindName("r" + (i));
                    label.Content = i;
                }
            }
        }

        public FieldType[] GetBoard()
        {
            return currentPosition;
        }

        private void ClearBoard(bool widthPieces = true)
        {
            var index = -1;
            foreach (var item in BoardInformations.InsideBoard)
            {
                index++;
                var field = fields.FindName("f" + index);
                if (widthPieces)
                    ((Label)field).Content = "";
                if (BoardInformations.WhiteFields.Contains(item))
                    ((Label)field).Background = whiteField;

                else
                    ((Label)field).Background = blackField;
            }
        }

        private void FieldClick(object sender, MouseButtonEventArgs e)
        {
            var currentLabel = ((Label)sender);
            var fieldNumber = int.Parse(currentLabel.Name.Substring(1));
            if (!IsWhite)
                fieldNumber = 63 - fieldNumber;
            var position = BoardInformations.InsideBoard.ElementAt(fieldNumber);
            BoardNode selectedNode;

            if (!isClickedOnce)
            {
                if (IsWhite)
                {
                    if (!BoardInformations.WhitePieces.Contains(currentPosition[position]))
                        return;
                }
                else if (!BoardInformations.BlackPieces.Contains(currentPosition[position]))
                    return;

                var pieceType = currentPosition[position];
                if (pieceType != FieldType.Empty)
                {
                    switch (pieceType)
                    {
                        case FieldType.WhiteKing:
                        case FieldType.BlackKing:
                            KingColoring(pieceType, position);
                            break;
                        default:
                            Coloring(pieceType, position);
                            break;
                    }
                    currentLabel.Background = new SolidColorBrush(Colors.Green);
                }
                else
                    currentLabel.Background = new SolidColorBrush(Colors.Red);
                isClickedOnce = true;
                prePos = position;
            }
            else
            {
                if (((SolidColorBrush)currentLabel.Background).Color == Brushes.Yellow.Color)
                {
                    currentPosition = TemporatyBoard(currentPosition, prePos, position);
                    SetBoard(currentPosition, null, IsWhite);
                    if (AI.IsStalemate(currentPosition, !IsWhite) && AI.IsCheck(currentPosition, !IsWhite))
                        MessageBox.Show("Congrats!! It is checkmate :)");
                    else
                    {
                        //Opponent move    

                        Stopwatch sw = new Stopwatch();

                        sw.Start();
                        AI.ListOfPossibleSteps = new Dictionary<BoardNode, int>();
                        AI.StartDepth = 3;


                        if (Memory.Root == null)
                        {
                            Memory.Root = new BoardNode();
                            Memory.Root.Board = currentPosition;
                        }
                        else
                        {
                            //Delete memory                          
                            selectedNode = Memory.Root.Nodes.FirstOrDefault(x => x.Board.SequenceEqual(currentPosition));
                            if (selectedNode != null)
                            {
                                var index = Memory.Root.Nodes.ToList<BoardNode>().IndexOf(selectedNode);
                                Memory.Root = Memory.Root.Nodes.ElementAt(index);
                            }
                            else
                                throw new Exception("Can't delete the memory");
                        }


                        AI.AlphaBeta2(Memory.Root, 3, int.MinValue, int.MaxValue, !IsWhite);
                        sw.Stop();
                        //MessageBox.Show($"{sw.Elapsed.TotalMinutes} minutes. {Memory.Root.Nodes.Count}");

                        var step = IsWhite ? AI.ListOfPossibleSteps.First(y => y.Value == AI.ListOfPossibleSteps.Min(x => x.Value)).Key : AI.ListOfPossibleSteps.First(y => y.Value == AI.ListOfPossibleSteps.Max(x => x.Value)).Key;

                        currentPosition = step.Board;
                        SetBoard(currentPosition, null, IsWhite);

                        //Delete Memory
                        selectedNode = Memory.Root.Nodes.FirstOrDefault(x => x.Board.SequenceEqual(step.Board) && x.Nodes == step.Nodes);
                        if (selectedNode != null)
                        {
                            var index = Memory.Root.Nodes.ToList<BoardNode>().IndexOf(selectedNode);
                            Memory.Root = Memory.Root.Nodes.ElementAt(index);
                        }
                        else
                            throw new Exception("Can't delete the memory");
                    }
                }
                else
                    ClearBoard(false);

                isClickedOnce = false;
            }
        }

        void KingColoring(FieldType type, byte currentPiecePos)
        {
            bool _isWhite = BoardInformations.WhitePieces.Contains(type);
            ClearBoard(false);
            var steps = PossibleSteps.WithKing(currentPosition, currentPiecePos, _isWhite);
            if (steps != null)
            {
                //Filter
                var otherKingPos = PossibleSteps.WhereIsTheKing(currentPosition, !_isWhite);
                var coloredField = steps.Select(x => Array.IndexOf(BoardInformations.InsideBoard.ToArray(), (byte)x));
                var opponentPosibbleSteps = PossibleSteps.AllPiece(currentPosition, !_isWhite);
                var b = coloredField.Where(x => !AI.IsCheck(TemporatyBoard(currentPosition, currentPiecePos, BoardInformations.InsideBoard.ElementAt((byte)x)), _isWhite));
                if (b.Count() > 0)
                    foreach (Label child in fields.Children)
                        if (b.Contains(this.IsWhite ? int.Parse(child.Name.Substring(1)) : 63 - int.Parse(child.Name.Substring(1))))
                            child.Background = new SolidColorBrush(Colors.Yellow);
            }
        }

        FieldType[] TemporatyBoard(FieldType[] currentBoard, byte currentFigPos, byte newFigPos)
        {
            var copyBoard = new FieldType[144];
            Array.Copy(currentBoard, copyBoard, 144);
            //TODO: always queen
            copyBoard[newFigPos] =
                copyBoard[currentFigPos] == FieldType.BlackPawn && BoardInformations.WhitePawnBasicState.Contains(currentFigPos) ? FieldType.BlackQueen :
                copyBoard[currentFigPos] == FieldType.WhitePawn && BoardInformations.BlackPawnBasicState.Contains(currentFigPos) ? FieldType.WhiteQueen : copyBoard[currentFigPos];
            copyBoard[currentFigPos] = FieldType.Empty;
            return copyBoard;
        }

        void Coloring(FieldType type, byte currentPiecePos)
        {
            var _isWhite = BoardInformations.WhitePieces.Contains(type);
            ClearBoard(false);
            var steps =
                type == FieldType.BlackRook || type == FieldType.WhiteRook || type == FieldType.BlackBishop || type == FieldType.WhiteBishop || type == FieldType.WhiteQueen || type == FieldType.BlackQueen
                ?
                PossibleSteps.WithPiece(currentPosition, currentPiecePos, WhichPieceSteps(type), _isWhite)
                :
                type == FieldType.BlackKnight || type == FieldType.WhiteKnight
                ?
                PossibleSteps.WithKnight(currentPosition, currentPiecePos, _isWhite)
                :
                type == FieldType.BlackPawn || type == FieldType.WhitePawn
                ?
                PossibleSteps.WithPawn(currentPosition, currentPiecePos, _isWhite)
                :
                null;
            if (steps != null)
            {
                var coloredField = steps.Select(x => BoardInformations.InsideBoard.ToList().IndexOf(x));
                if (coloredField.Count() > 0)
                    foreach (Label child in fields.Children)
                        if (coloredField.Contains(IsWhite ? int.Parse(child.Name.Substring(1)) : 63 - int.Parse(child.Name.Substring(1)))) //global isWhite, becouse it is the board orientation
                        {
                            var tempBoard = TemporatyBoard(currentPosition, currentPiecePos, BoardInformations.InsideBoard.ElementAt(IsWhite ? byte.Parse(child.Name.Substring(1)) : 63 - byte.Parse(child.Name.Substring(1))));
                            if (!AI.IsCheck(tempBoard, _isWhite))
                                child.Background = new SolidColorBrush(Colors.Yellow);
                        }
            }
        }

        HashSet<int> WhichPieceSteps(FieldType type)
        {
            switch (type)
            {
                case FieldType.BlackRook:
                case FieldType.WhiteRook:
                    return PossibleSteps.RookSteps;
                case FieldType.BlackBishop:
                case FieldType.WhiteBishop:
                    return PossibleSteps.BishopSteps;
                case FieldType.WhiteQueen:
                case FieldType.BlackQueen:
                    return PossibleSteps.QueenSteps;
                default:
                    throw new Exception();
            }
        }

        private void FieldMouseMove(object sender, MouseEventArgs e)
        {
            if (currentPosition == null)
                return;

            var currentLabel = ((Label)sender);
            var fieldNumber = int.Parse(currentLabel.Name.Substring(1));
            if (!IsWhite)
                fieldNumber = 63 - fieldNumber;
            var position = BoardInformations.InsideBoard.ElementAt(fieldNumber);
            if (IsWhite)
            {
                if (!BoardInformations.WhitePieces.Contains(currentPosition[position]))
                    return;
                else
                {
                    currentLabel.Cursor = Cursors.Hand;
                    currentLabel.BorderBrush = new SolidColorBrush(Colors.Blue);
                    currentLabel.BorderThickness = new Thickness(2);
                }
            }
            else
            {
                if (!BoardInformations.BlackPieces.Contains(currentPosition[position]))
                    return;
                else
                {
                    currentLabel.Cursor = Cursors.Hand;
                    currentLabel.BorderBrush = new SolidColorBrush(Colors.Blue);
                    currentLabel.BorderThickness = new Thickness(2);
                }
            }

        }

        private void FieldMouseLeave(object sender, MouseEventArgs e)
        {
            var currentLabel = ((Label)sender);
            currentLabel.Cursor = null;
            currentLabel.BorderBrush = null;
            currentLabel.BorderThickness = new Thickness(0);
        }
    }
}
