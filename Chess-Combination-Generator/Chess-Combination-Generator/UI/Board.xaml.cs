using Common;
using System;
using System.Collections.Generic;
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
    public partial class Board : UserControl
    {
        public Board()
        {
            InitializeComponent();
            this.Loaded += Board_Loaded;
        }

        private void Board_Loaded(object sender, RoutedEventArgs e)
        {
            //LOAD RESOURCES
            this.Resources["White"] = new SolidColorBrush(Colors.White);
            this.Resources["Black"] = new SolidColorBrush(Colors.Red);

        }

        public void SetBoard(FieldType[] board)
        {
            //RESOURCES
            //https://en.wikipedia.org/wiki/Chess_symbols_in_Unicode
            var a = main.FindName("f1");
            var index = -1;
            foreach (var item in BoardInformations.InsideBoard)
            {
                index++;
                if (board[item] != FieldType.Empty)
                {
                    var field = main.FindName("f" + index);
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
                            case FieldType.WhiteRock:
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
                            case FieldType.BlackRock:
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

            }
        }
    }
}
