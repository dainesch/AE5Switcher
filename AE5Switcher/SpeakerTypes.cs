using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE5Switcher
{
    enum SpeakerType
    {
        [Description("5.1")]
        FiveOne,
        [Description("Stereo")]
        Stereo,
        [Description("Direct")]
        Direct,
        [Description("Normal")]
        Normal
    }
}
