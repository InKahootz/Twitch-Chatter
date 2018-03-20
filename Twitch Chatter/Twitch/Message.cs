using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Twitch_Chatter
{
    public class Message
    {
        private Type type;
        private const string TWITCH_INFO_REGEX = @"^:tmi\.twitch\.tv\s(?<command>\d+)\s(?<user>.*?)\s:(?<usermessage>.*)";
        private const string PRIVMSG_REGEX = @":(?<user>.*?)!\k<user>@\k<user>\.tmi\.twitch\.tv\sPRIVMSG\s#(?<channel>.*?)\s:(?<usermessage>.*)";
        private const string TAG_REGEX = @"([\w-]+?)=(.*?)[;\s]";
        // @"^@badges=(?<badges>.*?);.*?(bits=(?<bits>.*?);)?color=(?<color>.*?);display\-name=(?<displayname>.*?);emotes=(?<emotes>.*?);id=(?<id>.*?);mod=(?<mod>.*?);room\-id=(?<roomid>.*?);sent\-ts=(?<sentts>.*?);subscriber=(?<subscriber>.*?);tmi\-sent\-ts=(?<tmisentts>.*?);turbo=(?<turbo>.*?);user\-id=(?<userid>.*?);user\-type=(?<usertype>.*?)\s:(?<user>.*?)!\k<user>@\k<user>\.tmi\.twitch\.tv\sPRIVMSG\s#(?<channel>.*?)\s:(?<usermessage>.*)$";

        public Message(string rawMessage)
        {
            type = GetType();
            ParseMessage(rawMessage);
            SetBadges();
        }

        public string Badges { get; private set; }

        public string Bits { get; private set; }

        public string Channel { get; set; }

        public string Color { get; set; } = "Black";

        public string DisplayName { get; private set; }

        public string Emotes { get; private set; }

        public string ID { get; private set; }

        public string UserMessage { get; private set; }

        public string Mod { get; private set; }

        public string RoomID { get; private set; }

        public string Subscriber { get; private set; }

        public string Turbo { get; private set; }

        public string User { get; private set; }

        public string UserID { get; private set; }

        public string UserType { get; private set; }

        // public TwitchUser User { get; set; }

        public int Command { get; private set; }

        public bool IsCommand { get; private set; }

        public bool IsPrivateMessage { get; private set; }

        public string RawMessage { get; set; }

        private void ParseMessage(string message)
        {
            RawMessage = message;

            var match = Regex.Match(message, PRIVMSG_REGEX);
            if (match.Success)
            {
                IsPrivateMessage = true;

                Channel = match.Groups["channel"].Value;
                User = match.Groups["user"].Value;
                UserMessage = match.Groups["usermessage"].Value;

                var tags = Regex.Matches(message, TAG_REGEX);
                foreach (Match tag in tags)
                {
                    var propInfo = type.GetProperty(tag.Groups[1].Value.Replace("-",""), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (!string.IsNullOrWhiteSpace(tag.Groups[2].Value))
                    {
                        propInfo?.SetValue(this, tag.Groups[2].Value);
                    }
                }
                return;
            }

            match = Regex.Match(message, TWITCH_INFO_REGEX);
            if (match.Success)
            {
                IsCommand = true;
                Command = int.Parse(match.Groups["command"].Value);
            }

        }

        private void SetBadges()
        {

        }
    }
}
