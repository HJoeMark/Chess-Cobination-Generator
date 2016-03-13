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

namespace Chess_Combination_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        byte[] whiteK;
        byte[] blackK;
        byte[] whiteR;
        byte[] blackR;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Init.BasicPosition();
            board.SetBoard(BoardInformations.CurrentPosition);
            //TEST
            //It's working
            //var whiteEvaluate = Evaluator.Evaluate(BoardInformations.CurrentPosition);
            //var blackEvaluate = Evaluator.Evaluate(BoardInformations.CurrentPosition, false);

            var allW = PossibleSteps.AllPiece(BoardInformations.CurrentPosition);
            var allB = PossibleSteps.AllPiece(BoardInformations.CurrentPosition, false);

            whiteK = PossibleSteps.WithKing(BoardInformations.CurrentPosition, BoardInformations.WhiteKingPosition, BoardInformations.BlackKingPosition)/*.Where(x => !allB.Contains(x)).ToArray()*/;
            blackK = PossibleSteps.WithKing(BoardInformations.CurrentPosition, BoardInformations.WhiteKingPosition, BoardInformations.BlackKingPosition, false)/*.Where(x => !allW.Contains(x)).ToArray()*/;
            whiteR = PossibleSteps.WithRock(BoardInformations.CurrentPosition);
            blackR = PossibleSteps.WithRock(BoardInformations.CurrentPosition, false);


            // board.SetBoard(BoardInformations.CurrentPosition, blackR);


            pSteps_lbox.Items.Add(FType.BlackKing);
            pSteps_lbox.Items.Add(FType.WhiteKing);
            pSteps_lbox.Items.Add(FType.BlackRocks);
            pSteps_lbox.Items.Add(FType.WhiteRocks);
        }


        //Select a piece race
        private void pSteps_lbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((FType)((ListBox)sender).SelectedItem)
            {
                case FType.WhiteKing:
                    board.SetBoard(BoardInformations.CurrentPosition, whiteK);
                    break;
                case FType.BlackKing:
                    board.SetBoard(BoardInformations.CurrentPosition, blackK);
                    break;
                case FType.WhiteRocks:
                    board.SetBoard(BoardInformations.CurrentPosition, whiteR);
                    break;
                case FType.BlackRocks:
                    board.SetBoard(BoardInformations.CurrentPosition, blackR);
                    break;
                default:
                    break;
            }
        }


    }

    enum FType
    {
        WhiteKing,
        BlackKing,
        WhiteRocks,
        BlackRocks
    }
}
