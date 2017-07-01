using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Chatter
{
    internal static class Options
    {
        public static string Channel { get; private set; }

        public static string UserName { get; private set; }

        public static string Token { get; private set; }

        public static void ParseIni()
        {
            if (File.Exists("settings.ini"))
            {
                ParseOptions(File.ReadAllLines("settings.ini"));
            }
        }

        public static void ParseOptions(string[] args)
        {
            OptionsParser.ParseCommandLineArguments(string.Join(" ", args));
            UserName = OptionsParser.UserName;
            Token = OptionsParser.Token;
            Channel = OptionsParser.Channel;
        }
    }
}
