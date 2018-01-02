using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE5Switcher
{
    class Constants
    {
        public static readonly String EXE_NAME = "Creative.SBConnect.exe";
        public static readonly String WINDOW_TITLE = "Sound Blaster Connect";
        public static readonly Point SETTINGS = new Point(50, -75);

        public static readonly Point SPEAKER_FIVE_ONE = new Point(260, 300);
        public static readonly Point SPEAKER_STEREO = new Point(375,300);
        public static readonly Point SPEAKER_DIRECT = new Point(490, 300);

        public static readonly Point HEADPHONES = new Point(600, 300);
        public static readonly Point HEADPHONES_DIRECT = new Point(725, 300);

        private Constants()
        {

        }
    }
}
