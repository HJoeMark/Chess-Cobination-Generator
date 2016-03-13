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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Init.BasicPosition();
            board.SetBoard(BoardInformations.CurrentPosition);
            //TEST
            var whiteEvaluate = Evaluator.Evaluate(BoardInformations.CurrentPosition);
            var blackEvaluate = Evaluator.Evaluate(BoardInformations.CurrentPosition, false);

            var whiteK = PossibleSteps.WithKing(BoardInformations.CurrentPosition, BoardInformations.WhiteKingPosition, BoardInformations.BlackKingPosition);
            var bhiteK = PossibleSteps.WithKing(BoardInformations.CurrentPosition, BoardInformations.WhiteKingPosition, BoardInformations.BlackKingPosition, false);
            var whiteR = PossibleSteps.WithRock(BoardInformations.CurrentPosition);
            var blackR = PossibleSteps.WithRock(BoardInformations.CurrentPosition, false);

            var allW = PossibleSteps.AllPiece(BoardInformations.CurrentPosition);
            var allB = PossibleSteps.AllPiece(BoardInformations.CurrentPosition, false);

        }
    }
}
