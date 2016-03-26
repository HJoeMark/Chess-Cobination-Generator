using Chess_Combination_Generator.Model;
using Common;
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
    /// Interaction logic for Generator.xaml
    /// </summary>
    public partial class Generator : UserControl
    {
        GenerationModel generationModel;

        public Generator()
        {
            InitializeComponent();
            this.Loaded += Generator_Loaded;
        }

        private void Generator_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void generate_btn_Click(object sender, RoutedEventArgs e)
        {
            List<string> fens = new List<string>();
            var index = 0;
            var lastFen = "";
            using (StreamWriter sw = new StreamWriter("fens" + DateTime.Now.Ticks + ".txt"))
            {
                while (index < generationModel.NumberOfCombination)
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
                    }
                }
            }
            MessageBox.Show("Complete");
        }
    }
}
