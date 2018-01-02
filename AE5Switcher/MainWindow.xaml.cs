using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AE5Switcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SpeakerBox.ItemsSource = new List<SpeakerType>() { SpeakerType.FiveOne, SpeakerType.Stereo, SpeakerType.Direct};
            HeadphoneBox.ItemsSource = new List<SpeakerType>() { SpeakerType.Normal, SpeakerType.Direct };

            LoadData();
        }

        public void LoadData()
        {
            SpeakerSlider.Value = Properties.Settings.Default.VolumeSpeaker;
            HeadphoneSlider.Value = Properties.Settings.Default.VolumeHeadphones;
            SBConnectBox.Text = Properties.Settings.Default.SBConnectExe;
            SBCloseBox.IsChecked = Properties.Settings.Default.CloseSBConnect;
            StartBox.IsChecked = Properties.Settings.Default.StartWithWindows;

            SpeakerBox.SelectedItem = Enum.Parse(typeof(SpeakerType), Properties.Settings.Default.ModeSpeaker);
            HeadphoneBox.SelectedItem = Enum.Parse(typeof(SpeakerType), Properties.Settings.Default.ModeHeadphones);

        }

        private void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("AE5Switcher", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
            }
        }

        private void RemoveApplicationFromStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("AE5Switcher", false);
            }
        }


        private void Save_CLick(object sender, RoutedEventArgs e)
        {
            if (StartBox.IsChecked.HasValue && (bool)StartBox.IsChecked)
            {
                AddApplicationToStartup();
            } else
            {
                RemoveApplicationFromStartup();
            }

            Properties.Settings.Default.VolumeHeadphones = (int)HeadphoneSlider.Value;
            Properties.Settings.Default.VolumeSpeaker = (int)SpeakerSlider.Value;
            Properties.Settings.Default.SBConnectExe = SBConnectBox.Text;
            Properties.Settings.Default.CloseSBConnect = (bool)SBCloseBox.IsChecked;
            Properties.Settings.Default.StartWithWindows = (bool)StartBox.IsChecked;

            Properties.Settings.Default.ModeSpeaker = SpeakerBox.SelectedItem.ToString();
            Properties.Settings.Default.ModeHeadphones = HeadphoneBox.SelectedItem.ToString();


            Properties.Settings.Default.Save();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Speaker_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SpeakerLabel.Content = ((int)SpeakerSlider.Value) + " %";

        }

        private void Headphone_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            HeadPhoneLabel.Content = ((int)HeadphoneSlider.Value) + " %";
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Title = "Location of " + Constants.EXE_NAME,
                InitialDirectory = SBConnectBox.Text.Replace(Constants.EXE_NAME, "")
            };
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    SBConnectBox.Text = fileDialog.FileName;
                    break;
            }
        }

    }
}
