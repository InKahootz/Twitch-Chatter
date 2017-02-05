using System.Windows;

namespace Twitch_Chatter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static MainWindow wnd;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            wnd = new MainWindow(e) {Title = "Twitch Chatter"};
            wnd.Show();
        }
    }
}
