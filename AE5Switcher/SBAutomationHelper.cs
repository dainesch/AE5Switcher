using AutoIt;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AE5Switcher
{
    class SBAutomationHelper
    {

        private SBAutomationHelper()
        {

        }


        public static void SetOutput(SpeakerMode mode, SpeakerType type)
        {
            AutoItX.Run(Properties.Settings.Default.SBConnectExe, Properties.Settings.Default.SBConnectExe.Replace(Constants.EXE_NAME,""));
            AutoItX.WinWaitActive(Constants.WINDOW_TITLE);

       
            Rectangle wPos = AutoItX.WinGetPos(Constants.WINDOW_TITLE);

            switch (mode)
            {
                case SpeakerMode.HEADPHONES:

                    switch (type)
                    {
                        case SpeakerType.Direct:
                            DoClicks(wPos, Constants.HEADPHONES_DIRECT);
                            break;
                        case SpeakerType.Normal:
                            DoClicks(wPos, Constants.HEADPHONES);
                            break;
                        default:
                            break;
                    }

                   
                    break;

                case SpeakerMode.SPEAKER:
                    switch (type)
                    {
                        case SpeakerType.FiveOne:
                            DoClicks(wPos, Constants.SPEAKER_FIVE_ONE);
                            break;
                        case SpeakerType.Stereo:
                            DoClicks(wPos, Constants.SPEAKER_STEREO);
                            break;
                        case SpeakerType.Direct:
                            DoClicks(wPos, Constants.SPEAKER_DIRECT);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            

            AutoItX.WinClose(Constants.WINDOW_TITLE);
            if (Properties.Settings.Default.CloseSBConnect)
            {
                AutoItX.ProcessClose(Constants.EXE_NAME);
            }

            
        }

        private static void DoClicks(Rectangle wPos, Point type)
        {
            Point orgPos = AutoItX.MouseGetPos();
            // click settings
            AutoItX.MouseClick("LEFT", wPos.Left + Constants.SETTINGS.X, wPos.Bottom + Constants.SETTINGS.Y, 1, 0);

            AutoItX.MouseMove(orgPos.X, orgPos.Y, 0);
            Thread.Sleep(1000);
            orgPos = AutoItX.MouseGetPos();

            // click mode
            AutoItX.MouseClick("LEFT", wPos.Left + type.X, wPos.Top + type.Y, 1, 0);
            AutoItX.MouseMove(orgPos.X, orgPos.Y, 0);
        }
    }
}
