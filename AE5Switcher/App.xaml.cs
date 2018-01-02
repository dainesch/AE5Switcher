using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AE5Switcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        private readonly NotifyIcon notifyIcon;
        private MainWindow mainWindow;
        private SpeakerMode currentMode;

        public App()
        {
            notifyIcon = new NotifyIcon()
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = AE5Switcher.Properties.Resources.TraySpeaker,
                Text = "AE5Switcher",
                Visible = true
            };

            notifyIcon.ContextMenuStrip.Items.Add("Activate Headphones").Click += (s,e) => {
                currentMode = SpeakerMode.SPEAKER;
                ChangeMode();
            };
            notifyIcon.ContextMenuStrip.Items.Add("Activate Speakers").Click += (s, e) => {
                currentMode = SpeakerMode.HEADPHONES;
                ChangeMode();
            };
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add("Settings").Click += OpenSettings;
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += ExitApp;

            notifyIcon.DoubleClick += OnDoubleClick;

            notifyIcon.Visible = true;

            GuessCurrentMode();
        
        }

        private void GuessCurrentMode()
        {
            double vol = AudioHelper.Instance.GetVolume();
            double head = Math.Abs(vol - AE5Switcher.Properties.Settings.Default.VolumeHeadphones);
            double speaker = Math.Abs(vol - AE5Switcher.Properties.Settings.Default.VolumeSpeaker);

            if (speaker < head)
            {
                currentMode = SpeakerMode.SPEAKER;
                notifyIcon.Icon = AE5Switcher.Properties.Resources.TraySpeaker;
            } else
            {
                currentMode = SpeakerMode.HEADPHONES;
                notifyIcon.Icon = AE5Switcher.Properties.Resources.TrayHead;
            }
        }

        private void ChangeMode()
        {

            if (!File.Exists(AE5Switcher.Properties.Settings.Default.SBConnectExe) || !AE5Switcher.Properties.Settings.Default.SBConnectExe.EndsWith(Constants.EXE_NAME))
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(
                    "Could not find the SoundBlaster Connect executable, please check your settings!", "Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return;
            }


            switch (currentMode)
            {
                case SpeakerMode.HEADPHONES:    // switch to speaker
                    currentMode = SpeakerMode.SPEAKER;
                    AudioHelper.Instance.SetVolume(0);
                    SBAutomationHelper.SetOutput(currentMode, (SpeakerType)Enum.Parse(typeof(SpeakerType), AE5Switcher.Properties.Settings.Default.ModeSpeaker));
                    AudioHelper.Instance.SetVolume(AE5Switcher.Properties.Settings.Default.VolumeSpeaker);
                    notifyIcon.Icon = AE5Switcher.Properties.Resources.TraySpeaker;

                    break;
                case SpeakerMode.SPEAKER:       // switch to headphones
                    currentMode = SpeakerMode.HEADPHONES;
                    AudioHelper.Instance.SetVolume(0);
                    SBAutomationHelper.SetOutput(currentMode, (SpeakerType)Enum.Parse(typeof(SpeakerType), AE5Switcher.Properties.Settings.Default.ModeHeadphones));
                    AudioHelper.Instance.SetVolume(AE5Switcher.Properties.Settings.Default.VolumeHeadphones);
                    notifyIcon.Icon = AE5Switcher.Properties.Resources.TrayHead;
                    break;
                default:
                    break;
            }
        }

        private void OpenSettings(object sender, EventArgs e)
        {
            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
                mainWindow.Closed += MainWindow_Closed;
            }
            mainWindow.Show();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            mainWindow = null;
        }


        private void ExitApp(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            if (mainWindow!=null && mainWindow.IsVisible)
            {
                mainWindow.Close();
            }
            System.Windows.Application.Current.Shutdown();
        }



        private void OnDoubleClick(object sender, EventArgs e)
        {
            ChangeMode();
        }


    }
}
