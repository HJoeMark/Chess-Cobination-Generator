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
    /// Interaction logic for PlayWithAIUI.xaml
    /// </summary>
    public partial class PlayWithAIUI : UserControl
    {
        public PlayWithAIUI()
        {
            InitializeComponent();
            board.SetBoard(BoardInformations.DefaultBoard);
        }

        void ChangeSide()
        {
            board.SetBoard(board.GetBoard(), null, !board.IsWhite);
        }

        private void changeSide_btn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSide();
        }
    }
}
