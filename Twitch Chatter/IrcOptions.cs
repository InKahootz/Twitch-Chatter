using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Chatter
{
    internal class IrcOptions
    {
        public string Channel { get; set; }


        public IrcOptions(string[] args)
        {
            //OptionsParser.ParseCommandLineArguments(args);
            Channel = OptionsParser.Channel;
        }
    }
}
