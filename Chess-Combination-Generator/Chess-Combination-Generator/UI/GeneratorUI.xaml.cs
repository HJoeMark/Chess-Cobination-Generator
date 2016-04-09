using Chess_Combination_Generator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Chess_Combination_Generator.UI
{
    /// <summary>
    /// Interaction logic for Generator.xaml
    /// </summary>
    public partial class GeneratorUI : UserControl
    {
        GenerationModel generationModel;
        BackgroundWorker bw = null;
        private bool isStart = false;

        List<string> fenList;



        public ProgressBar pbar
        {
            get { return (ProgressBar)GetValue(pbarProperty); }
            set { SetValue(pbarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for pbar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty pbarProperty =
            DependencyProperty.Register("pbar", typeof(ProgressBar), typeof(GeneratorUI), null);


        public GeneratorUI()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
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
                generate_btn_lab.Content = "Start Generate";
                Save();
            }
            else
            {
                fenList = new List<string>();
                pbar.Maximum = generationModel.NumberOfCombination;
                pbar.Value = 0;
                isStart = true;
                bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                generate_btn_lab.Content = "Stop Generate";
                // define the event handlers
                bw.DoWork += (sender, args) =>
                {
                    fenList = new List<string>();
                    var index = 0;
                    var lastFen = "";
                    if (!Directory.Exists("Fens"))
                        Directory.CreateDirectory("Fens");


                    while (index < generationModel.NumberOfCombination && isStart)
                    {
                        var newBoard = Chess_Combination_Generator.Generator.Generate(false, generationModel.IsWhite, generationModel.TreeLevel,
                         generationModel.Black.Queens, generationModel.Black.Rocks, generationModel.Black.Knights, generationModel.Black.Bishops, generationModel.Black.Pawns,
                         generationModel.White.Queens, generationModel.White.Rocks, generationModel.White.Knights, generationModel.White.Bishops, generationModel.White.Pawns);
                        if (newBoard.IsCorrect)
                        {
                            fenList.Add(newBoard.Fen);
                            lastFen = newBoard.Fen;
                            index++;

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                pbar.Value += 1;
                            }));
                        }
                    }

                    if (isStart)
                    {
                        Save();
                        MessageBox.Show("Complete", "Generate", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                };
                bw.RunWorkerCompleted += (sender, args) =>
                {
                    if (args.Error != null)  // if an exception occurred during DoWork,
                        MessageBox.Show(args.Error.ToString());  // do your error handling here

                    // Do whatever else you want to do after the work completed.
                    // This happens in the main UI thread.
                    generate_btn_lab.Content = "Start Generate";
                    isStart = false;

                };
                bw.Disposed += (sender, args) =>
                {

                };
                bw.RunWorkerAsync(); // starts the background worker

                // execution continues here in parallel to the background worker
            }
        }

        void Save()
        {
            var path = "Fens/fens" + DateTime.Now.Ticks + ".txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var fen in fenList)
                    sw.WriteLine(fen);
            }
            if (bw != null)
            {
                bw.Dispose();
                bw = null;
            }
        }
    }
}
