using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chess_Combination_Generator
{
    public static class Settings
    {
        //Colors
        public static SolidColorBrush WhiteField = new SolidColorBrush(Colors.White);
        public static SolidColorBrush BlackField = new SolidColorBrush(Colors.Gray);


        //File path

        public static string SettingsPath = "Settings.xml";

    }
}
