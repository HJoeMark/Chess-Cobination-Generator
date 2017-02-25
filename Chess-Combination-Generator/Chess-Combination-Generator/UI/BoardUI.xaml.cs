using System;
using System.Collections.Generic;
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
        bool isWhite;
        bool isClickedOnce;
        byte prePos;

        public BoardUI()
        {
            InitializeComponent();
            this.Loaded += Board_Loaded;
        }

        private void Board_Loaded(object sender, RoutedEventArgs e)
        {
            //LOAD RESOURCES
            this.Resources["White"] = Settings.WhiteField;
            this.Resources["Black"] = Settings.BlackField;
            isWhite = true;
        }

        public void SetBoard(FieldType[] board, byte[] possibleSteps = null, bool _isWhite = true)
        {
            ClearBoard();
            this.isWhite = _isWhite;
            this.isClickedOnce = false;
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
                    ((Label)field).Background = Settings.WhiteField;

                else
                    ((Label)field).Background = Settings.BlackField;
            }
        }

        private void FieldClick(object sender, MouseButtonEventArgs e)
        {
            var currentLabel = ((Label)sender);
            var fieldNumber = int.Parse(currentLabel.Name.Substring(1));
            if (!isWhite)
                fieldNumber = 63 - fieldNumber;
            var position = BoardInformations.InsideBoard[fieldNumber];


            if (!isClickedOnce)
            {
                if (isWhite)
                {
                    if (!BoardInformations.WhitePieces.Contains(BoardInformations.CurrentPosition[position]))
                        return;
                }
                else if (!BoardInformations.BlackPieces.Contains(BoardInformations.CurrentPosition[position]))
                    return;

                var pieceType = BoardInformations.CurrentPosition[position];
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
                    BoardInformations.CurrentPosition = TemporatyBoard(BoardInformations.CurrentPosition, prePos, position);
                    SetBoard(BoardInformations.CurrentPosition, null, isWhite);
                    if (AI.IsStalemate(BoardInformations.CurrentPosition, !isWhite) && AI.IsCheck(BoardInformations.CurrentPosition, !isWhite))
                        MessageBox.Show("Congrats!! It is checkmate :)");
                    else
                    {
                        //Opponent move

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
            var steps = PossibleSteps.WithKing(BoardInformations.CurrentPosition, currentPiecePos, _isWhite);
            if (steps != null)
            {
                //Filter
                var otherKingPos = PossibleSteps.WhereIsTheKing(BoardInformations.CurrentPosition, !_isWhite);
                var coloredField = steps.Select(x => Array.IndexOf(BoardInformations.InsideBoard, (byte)x));
                var opponentPosibbleSteps = PossibleSteps.AllPiece(BoardInformations.CurrentPosition, !_isWhite);
                var b = coloredField.Where(x => !AI.IsCheck(TemporatyBoard(BoardInformations.CurrentPosition, currentPiecePos, BoardInformations.InsideBoard[(byte)x]), _isWhite));
                if (b.Count() > 0)
                    foreach (Label child in fields.Children)
                        if (b.Contains(this.isWhite ? int.Parse(child.Name.Substring(1)) : 63 - int.Parse(child.Name.Substring(1))))
                            child.Background = new SolidColorBrush(Colors.Yellow);
            }
        }

        FieldType[] TemporatyBoard(FieldType[] currentBoard, byte currentFigPos, byte newFigPos)
        {
            var copyBoard = new FieldType[144];
            Array.Copy(currentBoard, copyBoard, 144);
            copyBoard[newFigPos] = copyBoard[currentFigPos];
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
                PossibleSteps.WithPiece(BoardInformations.CurrentPosition, currentPiecePos, WhichPieceSteps(type), _isWhite)
                :
                type == FieldType.BlackKnight || type == FieldType.WhiteKnight
                ?
                PossibleSteps.WithKnight(BoardInformations.CurrentPosition, currentPiecePos, _isWhite)
                :
                type == FieldType.BlackPawn || type == FieldType.WhitePawn
                ?
                PossibleSteps.WithPawn(BoardInformations.CurrentPosition, currentPiecePos, _isWhite)
                :
                null;
            if (steps != null)
            {
                var coloredField = steps.Select(x => BoardInformations.InsideBoard.ToList().IndexOf(x));
                if (coloredField.Count() > 0)
                    foreach (Label child in fields.Children)
                        if (coloredField.Contains(isWhite ? int.Parse(child.Name.Substring(1)) : 63 - int.Parse(child.Name.Substring(1)))) //global isWhite, becouse it is the board orientation
                        {
                            var tempBoard = TemporatyBoard(BoardInformations.CurrentPosition, currentPiecePos, BoardInformations.InsideBoard[isWhite ? byte.Parse(child.Name.Substring(1)) : 63 - byte.Parse(child.Name.Substring(1))]);
                            if (!AI.IsCheck(tempBoard, _isWhite))
                                child.Background = new SolidColorBrush(Colors.Yellow);
                        }
            }
        }

        int[] WhichPieceSteps(FieldType type)
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
            var currentLabel = ((Label)sender);
            var fieldNumber = int.Parse(currentLabel.Name.Substring(1));
            if (!isWhite)
                fieldNumber = 63 - fieldNumber;
            var position = BoardInformations.InsideBoard[fieldNumber];
            if (isWhite)
            {
                if (!BoardInformations.WhitePieces.Contains(BoardInformations.CurrentPosition[position]))
                    return;
                else
                {
                    currentLabel.Cursor = Cursors.Hand;
                    currentLabel.BorderBrush = new SolidColorBrush(Colors.LightBlue);
                    currentLabel.BorderThickness = new Thickness(2);
                }
            }
            else
            {
                if (!BoardInformations.BlackPieces.Contains(BoardInformations.CurrentPosition[position]))
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
