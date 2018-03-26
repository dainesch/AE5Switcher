using AutoIt;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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

        public static bool SetOutput(SpeakerMode mode, SpeakerType type)
        {
            Task<Boolean> t = Task.Factory.StartNew<Boolean>(() =>
            {
                return SetOutputInt(mode, type);
            });
            return t.Wait(TimeSpan.FromSeconds(Constants.SEC_WIN_WAIT * 2));
        }



        private static bool SetOutputInt(SpeakerMode mode, SpeakerType type)
        {
            AutoItX.Run(Properties.Settings.Default.SBConnectExe, Properties.Settings.Default.SBConnectExe.Replace(Constants.EXE_NAME, ""));
            if (AutoItX.WinWaitActive(Constants.WINDOW_TITLE, "", Constants.SEC_WIN_WAIT) == 0)
            {
                return false;
            }

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

            // wait a bit
            Thread.Sleep(500);

            return true;
        }

        private static void DoClicks(Rectangle wPos, Point type)
        {
            float scale = GetDisplayScaleFactor(AutoItX.WinGetHandle(Constants.WINDOW_TITLE));  // adapt to dpi

            Point orgPos = AutoItX.MouseGetPos();
            // click settings
            AutoItX.WinActivate(Constants.WINDOW_TITLE);
            AutoItX.MouseClick("LEFT", wPos.Left + (int)(Constants.SETTINGS.X * scale), wPos.Bottom + (int)(Constants.SETTINGS.Y * scale), 1, 0);
            AutoItX.MouseMove(orgPos.X, orgPos.Y, 0);

            Thread.Sleep(1000);


            // click mode
            orgPos = AutoItX.MouseGetPos();
            AutoItX.WinActivate(Constants.WINDOW_TITLE);
            AutoItX.MouseClick("LEFT", wPos.Left + (int)(type.X * scale), wPos.Top + (int)(type.Y * scale), 1, 0);
            AutoItX.MouseMove(orgPos.X, orgPos.Y, 0);
        }

        [DllImport("user32.dll")]
        static extern int GetDpiForWindow(IntPtr hWnd);

        private static float GetDisplayScaleFactor(IntPtr windowHandle)
        {
            try
            {
                return GetDpiForWindow(windowHandle) / 96f;
            }
            catch
            {
                return 1;
            }
        }
    }
}
