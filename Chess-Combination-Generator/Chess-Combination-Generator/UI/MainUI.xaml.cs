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
            generationProcess_pbar.ValueChanged += GenerationProcess_pbar_ValueChanged;
        }

        private void GenerationProcess_pbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            generazionProcess_tblock.Text = generationProcess_pbar.Maximum + "/" + generationProcess_pbar.Value;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BoardInformations.BasicPosition();
            generatorUI.pbar = generationProcess_pbar;

            //TODO: need an init mechanism
            Memory.Root = null;
        }
    }
}
