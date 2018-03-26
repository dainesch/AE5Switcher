using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE5Switcher
{
    class AudioHelper
    {

        public static double GetVolume()
        {
            CoreAudioController controller = new CoreAudioController();
            CoreAudioDevice defaultPlaybackDevice = controller.DefaultPlaybackDevice;
            double val = defaultPlaybackDevice.Volume;
            controller.Dispose();
            System.Console.Out.WriteLine("GetVolume done");
            return val;
        }

        public static void SetVolume(double volume)
        {
            CoreAudioController controller = new CoreAudioController();
            CoreAudioDevice defaultPlaybackDevice = controller.DefaultPlaybackDevice;
            Task<double> t = defaultPlaybackDevice.SetVolumeAsync(volume);
            t.Wait(Constants.SEC_WIN_WAIT);
            
            controller.Dispose();
            System.Console.Out.WriteLine("SetVolume done");
        }

    }
}
