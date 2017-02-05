using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Chatter
{
    internal static class Options
    {
        public static string UserName { get; private set; }

        public static string Token { get; private set; }

        public static void ParseOptions(string[] args)
        {
            OptionsParser.ParseCommandLineArguments(args);
            UserName = OptionsParser.UserName;
            Token = OptionsParser.Token;
        }
    }
}
