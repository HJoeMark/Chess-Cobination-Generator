using Chess_Combination_Generator.Model;
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
        //Board color
        public static SolidColorBrush WhiteField = new SolidColorBrush(Colors.White);
        public static SolidColorBrush BlackField = new SolidColorBrush(Colors.Gray);


        //Last setting



        //File path

        public static string SETTINGS_PATH = "Settings.xml";

    }

    public class SavedSettings
    {
        public Language Language { get; set; }
        public GenerationModel Generation { get; set; }

    }

    public enum Language
    {
        HUN,
        ENG
    }
}
