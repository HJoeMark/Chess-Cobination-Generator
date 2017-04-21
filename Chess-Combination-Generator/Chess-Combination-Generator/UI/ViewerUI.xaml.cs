using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ViewerUI : UserControl
    {
        public ViewerUI()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            this.Loaded += Viewer_Loaded;
            Memory.Root = null;
        }

        private void Viewer_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> fens = new List<string>();
                if (Directory.Exists("Fens"))
                    foreach (var file in Directory.GetFiles("Fens"))
                        if (System.IO.Path.GetExtension(file) == ".txt")
                            foreach (string line in File.ReadLines(file))
                                fens.Add(line);

                fens_lbox.ItemsSource = fens.Distinct().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fens_lbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoardInformations.SetBoard(BoardInformations.CurrentPosition, fens_lbox.SelectedItem.ToString());
            board.SetBoard(BoardInformations.CurrentPosition, null, fens_lbox.SelectedItem.ToString().Contains('w') ? true : false);
            Memory.Root = null;
        }

        private void fens_lbox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            fens_lbox.ContextMenu.Visibility = Visibility.Visible;
        }

        private void copy_menuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(fens_lbox.SelectedItem.ToString());
        }

        private void random_btn_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            var index = rnd.Next(0, fens_lbox.Items.Count - 1);
            fens_lbox.SelectedIndex = index;
            fens_lbox.ScrollIntoView(fens_lbox.SelectedItem);
        }

        private void saveToImage_btn_Click(object sender, RoutedEventArgs e)
        {
            UserControl control = this.board;
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)control.ActualWidth * 2, (int)control.ActualHeight * 2, 192, 192, PixelFormats.Pbgra32);
            Rect bounds = VisualTreeHelper.GetDescendantBounds(control);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(control);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);

            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));

            using (Stream fileStream = new FileStream("c_" + DateTime.Now.Ticks + ".png", FileMode.Create))
            {
                png.Save(fileStream);
            }
            MessageBox.Show("Complete", "Image saving", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
