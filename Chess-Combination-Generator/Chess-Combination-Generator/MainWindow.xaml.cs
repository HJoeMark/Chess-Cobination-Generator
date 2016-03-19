using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using static Common.AI;

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
        }

        //Select a piece race
        private void pSteps_lbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var whiteSteps = PossibleSteps.StepsForAllPiece(BoardInformations.CurrentPosition);
            var blackSteps = PossibleSteps.StepsForAllPiece(BoardInformations.CurrentPosition, false);

            blackR = blackSteps.Where(x => x.Value.FieldType == FieldType.BlackRock).Select(y => y.Value.Steps).SelectMany(x => x).ToArray();
            whiteR = whiteSteps.Where(x => x.Value.FieldType == FieldType.WhiteRock).Select(y => y.Value.Steps).SelectMany(x => x).ToArray();
            blackK = blackSteps.Where(x => x.Value.FieldType == FieldType.BlackKing).Select(y => y.Value.Steps).SelectMany(x => x).ToArray();
            whiteK = whiteSteps.Where(x => x.Value.FieldType == FieldType.WhiteKing).Select(y => y.Value.Steps).SelectMany(x => x).ToArray();

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

        private void Test_btn_Click(object sender, RoutedEventArgs e)
        {
            //StepAndValue SAV = new StepAndValue(0, 0, FieldType.Frame, 0, new List<StepAndValue>());
            //StepAndValue SAVAB = new StepAndValue(0, 0, FieldType.Frame, 0, new List<StepAndValue>());
            ////AI.AlphaBeta(BoardInformations.CurrentPosition, 5, int.MinValue, int.MaxValue, false, SAV);
            ////var val = AI.AB(BoardInformations.CurrentPosition, 5, int.MinValue, int.MaxValue, false, SAV);
            //AI.TreeTest(BoardInformations.CurrentPosition, 5, false, SAV);
            ////  var best = SAVAB.Children.First(y => y.EvaluatedValue == SAVAB.Children.Min(x => x.EvaluatedValue));
            //var best = SAV.Children.First(y => y.EvaluatedValue == SAV.Children.Min(x => x.EvaluatedValue));
            //var newBoard = new FieldType[144];
            //Array.Copy(BoardInformations.CurrentPosition, newBoard, 144);
            //newBoard[best.From] = FieldType.Empty;
            //newBoard[best.Where] = best.What;
            //BoardInformations.CurrentPosition = newBoard;
            //board.SetBoard(BoardInformations.CurrentPosition);
        }

        private void Generate_btn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 101; i++)
            {
                Init.BasicPosition();
                Generator.Generate(BoardInformations.CurrentPosition, false, false);
                board.SetBoard(BoardInformations.CurrentPosition);
                board.UpdateLayout();
                SaveToPng(container_grid, String.Format("C/combination{0}.png", i));
            }
        }

        void SaveToBmp(FrameworkElement visual, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            if (!Directory.Exists("C"))
                Directory.CreateDirectory("C");
            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }

        private void Photo_btn_Click(object sender, RoutedEventArgs e)
        {
            SaveToPng(container_grid, "board" + DateTime.Now.Ticks + ".png");

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
