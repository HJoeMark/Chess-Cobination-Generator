﻿using System;
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
        //bool firstClick = false;
        //Label firstLabel;

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
        }

        public void SetBoard(FieldType[] board, byte[] possibleSteps = null, bool isWhite = true)
        {
            ClearBoard();
            //RESOURCES
            //https://en.wikipedia.org/wiki/Chess_symbols_in_Unicode
            var index = isWhite ? -1 : 64;
            foreach (var item in BoardInformations.InsideBoard)
            {
                index += isWhite ? 1 : -1;
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
                // TEST
                //else
                //    ((Label)field).Content = item;

                if (possibleSteps != null && possibleSteps.Contains(item))
                {
                    field = fields.FindName("f" + index);
                    ((Label)field).Background = new SolidColorBrush(Colors.LightGreen);
                }
            }

            if (isWhite)
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

        private void ClearBoard()
        {
            var index = -1;
            foreach (var item in BoardInformations.InsideBoard)
            {
                index++;
                var field = fields.FindName("f" + index);
                ((Label)field).Content = "";
                if (BoardInformations.WhiteFields.Contains(item))
                    ((Label)field).Background = Settings.WhiteField;

                else
                    ((Label)field).Background = Settings.BlackField;

            }
        }

        private void FieldClick(object sender, MouseButtonEventArgs e)
        {
            //var currentLabel = ((Label)sender);
            //if (!firstClick)
            //{
            //    firstClick = true;
            //    firstLabel = currentLabel;
            //    firstLabel.Background = new SolidColorBrush(Colors.LightBlue);
            //}
            //else
            //{
            //    firstClick = false;
            //    currentLabel.Content = firstLabel.Content;
            //    firstLabel.Content = "";
            //    BoardInformations.CurrentPosition[BoardInformations.InsideBoard[byte.Parse(currentLabel.Name.Substring(1))]] = BoardInformations.CurrentPosition[BoardInformations.InsideBoard[byte.Parse(firstLabel.Name.Substring(1))]];
            //    BoardInformations.CurrentPosition[BoardInformations.InsideBoard[byte.Parse(firstLabel.Name.Substring(1))]] = FieldType.Empty;

            //    //Step();
            //}
        }

        public void Step()
        {
            //StepAndValue SAV = new StepAndValue(0, 0, FieldType.Frame, 0, new List<StepAndValue>());
            //StepAndValue SAVAB = new StepAndValue(0, 0, FieldType.Frame, 0, new List<StepAndValue>());
            //AI.AlphaBeta(BoardInformations.CurrentPosition, 5, int.MinValue, int.MaxValue, false, SAVAB);
            //var best = SAVAB.Children.First(y => y.EvaluatedValue == SAVAB.Children.Min(x => x.EvaluatedValue));
            //var newBoard = new FieldType[144];
            //Array.Copy(BoardInformations.CurrentPosition, newBoard, 144);
            //newBoard[best.From] = FieldType.Empty;
            //newBoard[best.Where] = best.What;
            //BoardInformations.CurrentPosition = newBoard;
            //SetBoard(BoardInformations.CurrentPosition);
        }
    }
}
