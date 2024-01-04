using Curly.LibCURL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Curly
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Test.GetLibCurlVersionAsync();

            string[] urls = new string[]
            {
                "https://www.gekysan.fun/kaio",
                "https://tls.peet.ws/api/tls",
                "https://am.i.mullvad.net/country"
            };

            string filePath = "multi-thread.txt";

            await Test.RunAsync(urls, filePath, 20);

            Console.WriteLine("All threads have completed.");
        }
    }
}
