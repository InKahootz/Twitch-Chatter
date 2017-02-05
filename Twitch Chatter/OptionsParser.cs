using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Chatter
{
    internal static class OptionsParser
    {
        public static string Channel { get; private set; }

        public static string UserName { get; private set; }

        public static string Token { get; private set; }

        public static void ParseCommandLineArguments(string[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-channel":
                        i++;
                        Channel = args[i];
                        break;
                    case "-user":
                        i++;
                        UserName = args[i];
                        break;
                    case "-token":
                        i++;
                        Token = args[i];
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
