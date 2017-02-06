using System.Collections.Generic;
using System.Windows.Media;

namespace Twitch_Chatter
{
    class TwitchUser
    {
        public bool Admin { get; set; }

        public bool Broadcaster { get; set; }

        public string DisplayName { get; set; }

        public bool GlobalMod { get; set; }

        public string Name { get; set; }

        public Color NameColor { get; set; }

        public Queue<string> Messages { get; set; }

        public bool Moderator { get; set; }

        public bool Prime { get; set; }

        public bool Staff { get; set; }

        public bool Subscriber { get; set; }

        public bool Turbo { get; set; }
    }
}
