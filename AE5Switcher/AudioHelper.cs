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

        private static volatile AudioHelper instance = null;
        private static object syncRoot = new Object();

        private CoreAudioController controller;

        private AudioHelper() {
            controller = new CoreAudioController();
        }

        public double GetVolume()
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            return defaultPlaybackDevice.Volume;
        }

        public void SetVolume(double volume)
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = volume;
        }

        public static AudioHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AudioHelper();
                    }
                }

                return instance;
            }
        }



    }
}
