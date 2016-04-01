using Chess_Combination_Generator.Model;
using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Chess_Combination_Generator.UI
{
    /// <summary>
    /// Interaction logic for Generator.xaml
    /// </summary>
    public partial class Generator : UserControl
    {
        GenerationModel generationModel;
        BackgroundWorker bw = null;


        private bool isStart = false;

        public Generator()
        {
            InitializeComponent();
            this.Loaded += Generator_Loaded;
            // TODO: Load for settings.xml
            //TEST
            generationModel = new GenerationModel()
            {
                NumberOfCombination = 4,
                TreeLevel = 1,
                IsWhite = false,
                Black = new PiecesNumber()
                {
                    Queens = 0,
                    Rocks = 2,
                    Bishops = 0,
                    Knights = 0,
                    Pawns = 0
                },
                White = new PiecesNumber()
                {
                    Queens = 0,
                    Rocks = 0,
                    Bishops = 0,
                    Knights = 0,
                    Pawns = 0
                }
            };
            this.DataContext = generationModel;
        }

        private void Generator_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void generate_btn_Click(object sende, RoutedEventArgs e)
        {
            if (bw != null)
            {
                isStart = false;
                bw.CancelAsync();
                bw.Dispose();
                bw = null;
                generate_btn.Content = "Start Generate";

            }
            else
            {
                //Global.ProgressBar.Maximum = generationModel.NumberOfCombination;
                //Global.ProgressBar.Value = 0;

                isStart = true;
                bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                generate_btn.Content = "Stop Generate";
                // define the event handlers
                bw.DoWork += (sender, args) =>
                {
                    List<string> fens = new List<string>();
                    var index = 0;
                    var lastFen = "";
                    if (!Directory.Exists("Fens"))
                        Directory.CreateDirectory("Fens");

                    var path = "Fens/fens" + DateTime.Now.Ticks + ".txt";
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        while (index < generationModel.NumberOfCombination && isStart)
                        {
                            var fen = "";
                            var nb = new FieldType[144];
                            foreach (var item in BoardInformations.InsideBoard)
                                nb[item] = FieldType.Empty;
                            if (Common.Generator.Generate(nb, out fen, false, generationModel.IsWhite, generationModel.TreeLevel,
                        generationModel.Black.Queens, generationModel.Black.Rocks, generationModel.Black.Knights, generationModel.Black.Bishops, generationModel.Black.Pawns,
                        generationModel.White.Queens, generationModel.White.Rocks, generationModel.White.Knights, generationModel.White.Bishops, generationModel.White.Pawns) && fen != lastFen)
                            {
                                sw.WriteLine(fen);
                                lastFen = fen;
                                index++;
                                //System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(() =>
                                //{
                                //    Global.ProgressBar.Value = index;
                                //});

                            }
                        }
                    }
                    if (isStart)
                        MessageBox.Show("Complete", "Generate", MessageBoxButton.OK, MessageBoxImage.Information);
                };
                bw.RunWorkerCompleted += (sender, args) =>
                {
                    if (args.Error != null)  // if an exception occurred during DoWork,
                        MessageBox.Show(args.Error.ToString());  // do your error handling here

                    // Do whatever else you want to do after the work completed.
                    // This happens in the main UI thread.
                    generate_btn.Content = "Start Generate";
                    isStart = false;

                };
                bw.Disposed += (sender, args) =>
                {

                };
                bw.RunWorkerAsync(); // starts the background worker

                // execution continues here in parallel to the background worker
            }
        }



    }
}
