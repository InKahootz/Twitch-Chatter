using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Twitch_Chatter
{
    internal static class ChannelCache
    {
        internal static Dictionary<string, Dictionary<int,BitmapImage>> Channels = new Dictionary<string, Dictionary<int,BitmapImage>>();


        internal static async void GetChannelInfo(string channel)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(Constants.KRAKEN_BASE);
                client.DefaultRequestHeaders.Add("Client-ID", Constants.CLIENT_ID);
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v5+json");

                var response = await client.GetAsync($"https://api.twitch.tv/kraken/users?login={channel}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var id = JsonConvert.DeserializeObject<UserResponse>(responseBody).users[0]._id;

                response = await client.GetAsync($"https://badges.twitch.tv/v1/badges/channels/{id}/display");
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();

                //var tasks = new List<Task<BitmapImage>>();
                var badges = JsonConvert.DeserializeObject<RootObject>(responseBody);
                var images = new Dictionary<int, BitmapImage>();
                foreach (KeyValuePair<int,Versions> item in badges.badge_sets.subscriber.versions)
                {
                    var downloadTask = await LoadImage(item.Value.image_url_1x);
                    images.Add(item.Key, downloadTask);
                    //tasks.Add(downloadTask);
                }

                //await Task.WhenAll(tasks.ToArray());

                //var images = new Dictionary<int, BitmapImage>();
                //foreach (var item in tasks)
                //{
                    
                //}
            }
        }

        private async static Task<BitmapImage> LoadImage(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Stream st = await client.GetStreamAsync(url);
                    var ms = new MemoryStream();
                    await st.CopyToAsync(ms);
                    ms.Position = 0;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();

                    ms.Dispose();
                    st.Dispose();


                    return bitmap;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }

    internal class UserResponse
    {
        public int _total { get; set; }

        public List<TwitchJsonUser> users { get; set; }
    }

    internal class TwitchJsonUser
    {
        public string _id { get; set; }
    }

    public class Versions
    {
        public string image_url_1x { get; set; }
        public string image_url_2x { get; set; }
        public string image_url_4x { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string click_action { get; set; }
        public string click_url { get; set; }
    }

    public class Subscriber
    {
        public Dictionary<int,Versions> versions { get; set; }
    }

    public class BadgeSets
    {
        public Subscriber subscriber { get; set; }
    }

    public class RootObject
    {
        public BadgeSets badge_sets { get; set; }
    }
}
