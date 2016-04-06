using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Combination_Generator.Model
{
    public class SettingsModel
    {
        private Language language;

        public Language Language
        {
            get { return language; }
            set { language = value; }
        }

        private string settingsPath = Settings.SETTINGS_PATH;

        public string SettingsPath
        {
            get { return settingsPath; }
            //set { settingsPath = value; }
        }




    }
}
