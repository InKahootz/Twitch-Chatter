using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Twitch_Chatter
{
    internal sealed class IrcClient
    {

        private TcpClient _tcpClient;
        private StreamReader _inputStream;
        private StreamWriter _outputStream;

        private string _channel;
        private string _userName;
        private string _token;

        public string Channel => _channel;
        public string UserName => _userName;

        public IrcClient()
        {
            _tcpClient = new TcpClient("irc.chat.twitch.tv", 6667);
            _inputStream = new StreamReader(_tcpClient.GetStream());
            _outputStream = new StreamWriter(_tcpClient.GetStream()) { AutoFlush = true };

            this._channel = Options.Channel;
            this._token = Options.Token;
            this._userName = Options.UserName;

            SendCredentials();
        }


        public IrcClient(string userName, string token, string channel)
            : this()
        {
            this._channel = channel;
            this._userName = userName;
            this._token = token;
            //outputStream.WriteLine("USER " + userName + " 8 * :" + userName);

            SendCredentials();
        }

        public void Join()
        {
            if (string.IsNullOrWhiteSpace(_channel)) return;

            _outputStream.WriteLine("JOIN #" + _channel + "\r\n");
        }

        public void JoinRoom(string channel)
        {
            this._channel = channel;
            Join();
        }

        public void ReplyPong(string message)
        {
            _outputStream.WriteLine(message.Replace("PING", "PONG"));
        }

        public void ParseCommand(string message)
        {
            if (message.StartsWith("/"))
            {
                var reg = Regex.Match(message, @"\/join (\w+)");
                if (reg.Success)
                {
                    JoinRoom(reg.Groups[1].Value);
                }
            }
            else
            {
                SendChatMesage(message);
            }

        }

        public void SendChatMesage(string message)
        {
            SendIrcMessage($":{_userName}!{_userName}@{_userName}.tmi.twitch.tv PRIVMSG #{_channel} :{message}");
        }

        public string ReadMessage()
        {
            string message = _inputStream.ReadLine();
            if (message != null && message.StartsWith("PING"))
            {
                ReplyPong(message);
                return " ";
            }
            return message;
        }

        public async Task<string> ReadMessageAsync()
        {
            var message = await _inputStream.ReadLineAsync();
            if (message == null) return "";
            if (message.StartsWith("PING"))
            {
                ReplyPong(message);
                message = "";
            }
            return message;
        }

        private void HandlePingPong(string message)
        {
            if (message.StartsWith("PING"))
            {
                ReplyPong(message);
            }
        }
        private void SendCredentials()
        {
            _outputStream.WriteLine("PASS oauth:" + _token);
            _outputStream.WriteLine("NICK " + _userName);           
        }

        private void SendIrcMessage(string message)
        {
            _outputStream.WriteLine(message);
        }

        internal void RequestMembership()
        {
            SendIrcMessage("CAP REQ :twitch.tv/membership");
        }

        internal void RequestTags()
        {
            SendIrcMessage("CAP REQ :twitch.tv/tags");
        }
    }
}
