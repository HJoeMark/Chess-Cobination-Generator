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
    /// Interaction logic for Viewer.xaml
    /// </summary>
    public partial class Viewer : UserControl
    {
        public Viewer()
        {
            InitializeComponent();
            this.Loaded += Viewer_Loaded;
        }

        private void Viewer_Loaded(object sender, RoutedEventArgs e)
        {
            fens_lbox.Items.Clear();
            if (Directory.Exists("Fens"))
                foreach (var file in Directory.GetFiles("Fens"))
                    if (System.IO.Path.GetExtension(file) == ".txt")
                        foreach (string line in File.ReadLines(file))
                            fens_lbox.Items.Add(line);
        }
    }
}
