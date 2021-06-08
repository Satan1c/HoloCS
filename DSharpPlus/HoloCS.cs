using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DSharpPlus;
using HoloCS.DSharpPlus.Types;
using Newtonsoft.Json;

namespace HoloCS.DSharpPlus
{
    public class HoloClient
    {
        internal static readonly TimeSpan RateLimit = new TimeSpan(0, 0, 0, 5);
        internal static DateTime LastRequest;

        internal static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("http://discord-holo-api.ml/api/")
        };

        public HoloClient(DiscordClient client)
        {
            Client = client;

            SFW = new Sfw();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public Sfw SFW { get; set; }

        internal string Token { get; set; }
        internal static DiscordClient Client { get; set; }

        internal static async Task<T> GetRequest<T>(string path)
        {
            await RateLimiter();

            var response = await HttpClient.GetAsync(path);
            response.EnsureSuccessStatusCode();

            LastRequest = DateTime.UtcNow;

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        internal static async Task<T> GetRequest<T>(string path, HttpClient client, TimeSpan timeout = new TimeSpan())
        {
            await RateLimiter(timeout != new TimeSpan() ? timeout : new TimeSpan(0, 0, 0, 0, 50));

            var response = await client.GetAsync(path);
            response.EnsureSuccessStatusCode();

            LastRequest = DateTime.UtcNow;

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        internal static async Task RateLimiter()
        {
            while (true)
            {
                if (LastRequest == new DateTime() || DateTime.UtcNow - LastRequest > RateLimit) return;
                await Task.Delay((int) MathF.Round((float) ((DateTime.UtcNow - LastRequest) - RateLimit).TotalMilliseconds, 0));
            }
        }

        internal static async Task RateLimiter(TimeSpan timeout)
        {
            while (true)
            {
                if (LastRequest == new DateTime() || DateTime.UtcNow - LastRequest > timeout) return;
                await Task.Delay((int) MathF.Round((float) ((DateTime.UtcNow - LastRequest) - timeout).TotalMilliseconds, 0));
            }
        }
    }
}