using AduSkin.Controls.Metro;
using System.Windows;
using TerminalMACS.Infrastructure.UI;

namespace TerminalMACS.Views
{
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();

            this.Closed += delegate { Application.Current.Shutdown(); };
            Theme.ColorChange += delegate
            {
                // Do not bind colors through XAML, unable to get notifications
                BorderBrush = Theme.CurrentColor.OpaqueSolidColorBrush;
            };
        }


        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            string language = (sender as MetroMenuItem).Tag.ToString();
            LanguageHelper.SetLanguage(language);
        }

        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }
    }
}
