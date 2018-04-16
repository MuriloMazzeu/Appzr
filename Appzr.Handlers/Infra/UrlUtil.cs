using System.Diagnostics;

namespace Appzr.Handlers.Infra
{
    internal static class UrlUtil
    {
        /// <summary>
        /// Open an url in default browser system
        /// </summary>
        /// <param name="url">link must be http or https protocol</param>
        internal static void Open(string url)
        {
            Process.Start(url);
        }
    }
}
