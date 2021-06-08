using System.Threading.Tasks;

namespace HoloCS.DSharpPlus.Types
{
    public class Sfw
    {
        internal Sfw()
        {
        }
        
        public async Task<HoloResponce> GetImage(string category)
        {
            return await HoloClient.GetRequest<HoloResponce>(category);
        }
        
        public  async Task<HoloResponce> GetImage(Categories category)
        {
            return await HoloClient.GetRequest<HoloResponce>(category.ToString());
        }
    }

    public class HoloResponce
    {
        /// <summary>
        ///     An error message, != null only when error
        /// </summary>
        public string message { get; set; }
        
        /// <summary>
        ///     An image url, != null if 
        /// </summary>
        public string url { get; set; }
    }

    public enum Categories
    {
        art,
        ask,
        bite,
        cry,
        cuddle,
        dance,
        ego,
        glare,
        highfive,
        hug,
        kiss,
        lick,
        nom,
        pat,
        poke,
        pressf,
        punch,
        sex,
        shoot,
        slap,
        slappope,
        smug,
        suicide,
        tickle,
        wasted,
        wink
    }
}