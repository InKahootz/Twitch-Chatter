using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Twitch_Chatter
{
    internal static class OptionsParser
    {
        private static string _channelRegex = @"\-channel\s?(\w+)";
        private static string _tokenRegex = @"\-token\s?(\w+)";
        private static string _usernameRegex = @"\-user\s?(\w+)";

        public static string Channel { get; private set; }

        public static string UserName { get; private set; }

        public static string Token { get; private set; }

        public static void ParseCommandLineArguments(string args)
        {
            
            if (Regex.IsMatch(args, _channelRegex))
            {
                Channel = Regex.Match(args, _channelRegex).Groups[1].Value;
            }

            if (Regex.IsMatch(args, _tokenRegex))
            {
                Token = Regex.Match(args, _tokenRegex).Groups[1].Value;
            }

            if (Regex.IsMatch(args, _usernameRegex))
            {
                UserName = Regex.Match(args, _usernameRegex).Groups[1].Value;
            }
        }

    }
}
