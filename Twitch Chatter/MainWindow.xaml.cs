using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Twitch_Chatter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Regex regex = new Regex(@":([\w])*!");
        static IrcClient ircClient;
        static string chatChannel;
        static MainWindow wnd;

        public static ObservableCollection<string> Messages { get; set; }
        public ObservableCollection<string> Users { get; set; }

        internal List<IrcClient> Clients { get; set; }

        public string CommandBoxText
        {
            get
            {
                return commandBoxText;
            }
            set
            {
                if (commandBoxText != value)
                {
                    commandBoxText = value;
                    OnPropertyChanged("CommandBoxText");
                }
            }
        }
        private string commandBoxText;
        public MainWindow(StartupEventArgs e)
        {
            Options.ParseOptions(e.Args);

            Messages = new ObservableCollection<string>();
            Users = new ObservableCollection<string>();
            Clients = new List<IrcClient>();

            InitializeComponent();
            DataContext = this;

            ircClient = new IrcClient(new IrcOptions(e.Args));

            RunClient();
        }

        private void IRC_SendCommand(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ircClient.SendChatMesage(commandBoxText);
                CommandBoxText = String.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void RunClient()
        {
            while (true)
            {
                var message = await ircClient.ReadMessageAsync();
                if (!string.IsNullOrEmpty(message) && message != " ")
                {
                    string[] completeMsg = message.Split(' ');
                    string[] identityInfo = completeMsg[1].Split(' ');
                    if (completeMsg[2] == "PRIVMSG")
                    {
                        var match = new Regex(@"name=(.*?);.*PRIVMSG.*:(.*)").Match(message);
                        string userName = match.Groups[1].Value;
                        string text = match.Groups[2].Value;
                        Messages.Add($"{userName}: {text}");
                        MessagesScrollViewer.ScrollToBottom();
                        continue;
                    }

                    if (completeMsg[1] == "001")
                    {
                        ircClient.Join();
                    }
                    //After server sends 001 command, we can set mode to bot and join a channel
                    if (completeMsg[1] == "004")
                    {
                        ircClient.RequestMembership();
                        ircClient.RequestTags();

                    }
                    if (completeMsg[1] == "376")
                    {
                        // Request IRCv3 membership for capabilities such as usernames list
                        //ircClient.JoinRoom(chatChannel);
                    }

                    // After joining channel, recieve username list until 366 ident
                    //if (completeMsg[1] == "353")
                    //{
                    //    string usersString = "";
                    //    do
                    //    {
                    //        usersString += $" {completeMsg[2]}";
                    //        message = ircClient.ReadMessage();
                    //        completeMsg = message.Split(' ');
                    //    } while (completeMsg[1] != "366");

                    //    await wnd.Dispatcher.BeginInvoke(
                    //        new Action(() =>
                    //        {
                    //            foreach (string s in usersString.Split(' '))
                    //            {
                    //                wnd.Users.Add(s);
                    //            }
                    //        })
                    //    );
                    //}

                    // JTV add or remove operator +o/-o
                    if (identityInfo[0] == "jtv")
                    {

                    }

                    //Console.WriteLine(message);
                }
            }
        }

    }
}
