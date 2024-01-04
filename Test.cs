using Curly.LibCURL;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Curly
{
    public static class Test
    {
        public static async Task RunAsync(string[] urls, string filePath, int numberOfTasks)
        {
            Task[] tasks = new Task[numberOfTasks];
            SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

            for (int i = 0; i < tasks.Length; i++)
            {
                int localI = i;
                int index = localI % urls.Length;
                tasks[localI] = Task.Run(async () =>
                {
                    using (var curlHandle = new CurlHandle())
                    {
                        var curlHelper = new CurlHelper(curlHandle);
                        CurlResult result = await curlHelper.GetAsync(urls[index]);

                        await WriteToFileAsync(semaphore, filePath, $"Thread {localI} completed with result: {result.Response.Length} characters\nURL: {result.ResponseUrl}\nIP: {result.IpAddress}\nResponse Code: {result.ResponseCode}\nResponse: {result.Response}\n");
                    }
                });
            }

            await Task.WhenAll(tasks);
        }

        private static async Task WriteToFileAsync(SemaphoreSlim semaphore, string filePath, string content)
        {
            await semaphore.WaitAsync();
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    await writer.WriteLineAsync(content);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task GetLibCurlVersionAsync()
        {
            await Task.Run(() =>
            {
                string version = CurlHelper.GetLibCurlVersion();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("LibCurl version: " + version);
                Console.ResetColor();
            });
        }
    }
}
