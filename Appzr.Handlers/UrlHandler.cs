using Appzr.Handlers.Infra;
using System.Threading.Tasks;

namespace Appzr.Handlers
{
    public class UrlHandler
    {
        public async static Task OpenInBrowserAsync(string[] urls)
        {
            await Task.Run(() => 
                Parallel.ForEach(urls, async url => 
                    await Task.Run(() => 
                        UrlUtil.Open(url))));
        }
    }
}
