using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
                {
                    foreach (var file in Directory.GetFiles("Fens"))
                        if (System.IO.Path.GetExtension(file) == ".txt")
                            foreach (string line in File.ReadLines(file))
                                fens.Add(line);

                    //merges db
                    if (Directory.GetFiles("Fens").Count() > 1)
                    {
                        //Delete old files
                        foreach (var file in Directory.GetFiles("Fens"))
                            File.Delete(file);

                        //Create new file
                        var fensList = fens.Distinct().ToList();
                        using (StreamWriter file = new System.IO.StreamWriter("Fens/fens" + DateTime.Now.Ticks + ".txt"))
                        {
                            foreach (string line in fensList)
                                file.WriteLine(line);
                        }
                    }
                    fens_lbox.SelectedIndex = 0;
                }
                fens_lbox.ItemsSource = fens.Distinct().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Fens_lbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoardInformations.SetBoard(BoardInformations.CurrentPosition, fens_lbox.SelectedItem.ToString());
            board.SetBoard(BoardInformations.CurrentPosition, null, fens_lbox.SelectedItem.ToString().Contains('w') ? true : false);
            Memory.Root = null;
        }

        private void Fens_lbox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            fens_lbox.ContextMenu.Visibility = Visibility.Visible;
        }

        private void Copy_menuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(fens_lbox.SelectedItem.ToString());
        }

        private void Random_btn_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            var index = rnd.Next(0, fens_lbox.Items.Count - 1);
            fens_lbox.SelectedIndex = index;
            fens_lbox.ScrollIntoView(fens_lbox.SelectedItem);
        }

        private void SaveToImage_btn_Click(object sender, RoutedEventArgs e)
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

            if (!Directory.Exists("Images"))
                Directory.CreateDirectory("Images");

            using (Stream fileStream = new FileStream($@"Images\c_{DateTime.Now.Ticks}.png", FileMode.Create))
            {
                png.Save(fileStream);
            }
            MessageBox.Show("Complete", "Image saving", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenImagesFolder_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists("Images"))
                Process.Start("Images");
            else
                MessageBox.Show("You haven't got any images yet.", "Images not found", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
