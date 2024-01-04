using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Curly.LibCURL.CurlEnums;

namespace Curly.LibCURL
{
    internal class CurlHelper
    {
        private readonly CurlHandle _curlHandle;

        public CurlHelper(CurlHandle curlHandle)
        {
            _curlHandle = curlHandle ?? throw new ArgumentNullException(nameof(curlHandle));
        }
        public async Task<CurlResult> GetAsync(string url)
        {
            return await Task.Run(() =>
            {
                using (var curlHandle = new CurlHandle())
                {
                    SetupCurl(curlHandle.Pointer, url);

                    StringBuilder response = new StringBuilder();
                    StringBuilder headerBuilder = new StringBuilder();

                    SetCurlCallbacks(curlHandle.Pointer, response, headerBuilder);

                    curlHandle.Perform();

                    return new CurlResult
                    {
                        Response = response.ToString(),
                        ResponseUrl = GetStringInfo(curlHandle.Pointer, CurlEnums.Infos.CURLINFO_EFFECTIVE_URL),
                        IpAddress = GetStringInfo(curlHandle.Pointer, CurlEnums.Infos.CURLINFO_PRIMARY_IP),
                        ResponseCode = GetLongInfo(curlHandle.Pointer, CurlEnums.Infos.CURLINFO_RESPONSE_CODE),
                    };
                }
            });
        }


        private void SetCurlCallbacks(IntPtr curl, StringBuilder response, StringBuilder headerBuilder)
        {
            var writeCallbackDelegate = WriteCallback(response);
            var headerCallbackDelegate = WriteCallback(headerBuilder);

            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_WRITEFUNCTION, Marshal.GetFunctionPointerForDelegate(writeCallbackDelegate));
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_WRITEDATA, IntPtr.Zero);
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_HEADERFUNCTION, Marshal.GetFunctionPointerForDelegate(headerCallbackDelegate));
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_HEADERDATA, IntPtr.Zero);
        }

        private void SetupCurl(
            IntPtr curl,

            string URL = DefaultData.URL,
            bool autoRedirect = true,
            int timeoutMilliseconds = DefaultData.timeoutMilliseconds,
            int maxNumberOfRedirects = DefaultData.maxNumberOfRedirects,

            // HttpMethod httpMethod = HttpMethod.GET,
            HttpVersion httpVersion = HttpVersion.CURL_HTTP_VERSION_NONE,

            // string content = DefaultData.content,
            string contentType = DefaultData.contentType,
            Dictionary<string, string> customHeaders = null,
            string userAgent = DefaultData.userAgent,

            SecurityProtocol sslVersion = SecurityProtocol.CURL_SSLVERSION_DEFAULT,
            bool enableALPN = DefaultData.enableALPN,
            bool falseStart = DefaultData.falseStart,
            bool customEllipticCurveAlgorithm = DefaultData.customEllipticCurveAlgorithm,
            string ellipticCurveAlgorithm = DefaultData.ellipticCurveAlgorithm,
            bool customCipherList = DefaultData.customCipherList,
            string cipherList = DefaultData.cipherList,
            bool verifyPeer = DefaultData.verifyPeer,
            string certificatPath = DefaultData.certificatPath,

            bool customDNS = DefaultData.customDNS,
            string DNS = DefaultData.DNS
            )
        {
            /*******************************************
             *                                         *
             *                 Base                    *
             *                                         *
             *******************************************/

            // Configuration de base
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_URL, URL);

            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_FOLLOWLOCATION, autoRedirect ? new IntPtr(1) : IntPtr.Zero);
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_TIMEOUT_MS, new IntPtr(timeoutMilliseconds));
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_MAXREDIRS, new IntPtr(maxNumberOfRedirects));
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_USERAGENT, userAgent);
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_HTTP_VERSION, new IntPtr((int)httpVersion));


            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_SSL_VERIFYPEER, verifyPeer ? new IntPtr(1) : IntPtr.Zero);
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_CAPATH, certificatPath);

            /*******************************************
             *                                         *
             *                  SSL                    *
             *                                         *
             *******************************************/

            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_SSL_VERIFYPEER, verifyPeer ? new IntPtr(1) : IntPtr.Zero);
            if (verifyPeer && !string.IsNullOrEmpty(certificatPath))
            {
                CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_CAINFO, certificatPath);
            }

            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_SSLVERSION, new IntPtr((int)sslVersion));
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_SSL_ENABLE_ALPN, enableALPN ? new IntPtr(1) : IntPtr.Zero);
            CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_SSL_FALSESTART, falseStart ? new IntPtr(1) : IntPtr.Zero);

            if (customEllipticCurveAlgorithm) CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_SSL_EC_CURVES, ellipticCurveAlgorithm);
            if (customCipherList) CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_SSL_CIPHER_LIST, cipherList);
            
            if (customHeaders != null && customHeaders.Count > 0)
            {
                IntPtr slist = IntPtr.Zero;
                foreach (var header in customHeaders)
                {
                    string formattedHeader = $"{header.Key}: {header.Value}";
                    slist = CurlInterop.curl_slist_append(slist, formattedHeader);
                }

                slist = CurlInterop.curl_slist_append(slist, "Content-Type: " + contentType);

                if (slist != IntPtr.Zero)
                {
                    CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_HTTPHEADER, slist);
                }
            }

            /*******************************************
             *                                         *
             *                  Others                 *
             *                                         *
             *******************************************/

            if (customDNS) CurlInterop.curl_easy_setopt(curl, (int)CurlEnums.Options.CURLOPT_DNS_SERVERS, DNS);

        }

        private static string FormatResponse(IntPtr curl, StringBuilder response, StringBuilder headerBuilder)
        {
            string responseUrl = GetStringInfo(curl, CurlEnums.Infos.CURLINFO_EFFECTIVE_URL);
            string ipAddress = GetStringInfo(curl, CurlEnums.Infos.CURLINFO_PRIMARY_IP);
            long responseCode = GetLongInfo(curl, CurlEnums.Infos.CURLINFO_RESPONSE_CODE);

            return $"Response URL: {responseUrl}\n" +
                   $"Server IP: {ipAddress}\n" +
                   $"Response Code: {responseCode}\n" +
                   "Response Headers:\n" + headerBuilder.ToString() + "\n" +
                   "Response Body:\n" + response.ToString();
        }

        private static string GetStringInfo(IntPtr curl, CurlEnums.Infos info)
        {
            IntPtr infoPtr = IntPtr.Zero;
            CurlInterop.curl_easy_getinfo(curl, (int)info, ref infoPtr);
            return Marshal.PtrToStringAnsi(infoPtr);
        }

        private static long GetLongInfo(IntPtr curl, CurlEnums.Infos info)
        {
            long infoValue = 0;
            CurlInterop.curl_easy_getinfo(curl, (int)info, ref infoValue);
            return infoValue;
        }



        private static CurlCallbacks.CurlWriteCallbackDelegate WriteCallback(StringBuilder stringBuilder)
        {
            return (IntPtr buffer, int size, int nmemb, IntPtr userdata) =>
            {
                int realSize = size * nmemb;
                byte[] data = new byte[realSize];
                Marshal.Copy(buffer, data, 0, realSize);
                stringBuilder.Append(Encoding.UTF8.GetString(data));
                return realSize;
            };
        }

        public static string GetLibCurlVersion()
        {
            IntPtr versionPtr = CurlInterop.curl_version();
            return Marshal.PtrToStringAnsi(versionPtr);
        }
    }

    class CurlHandle : IDisposable
    {
        public IntPtr Pointer { get; private set; }

        public CurlHandle()
        {
            Pointer = CurlInterop.curl_easy_init();
            if (Pointer == IntPtr.Zero)
                throw new Exception("Failed to initialize cURL.");
        }

        public void Perform()
        {
            if (Pointer == IntPtr.Zero)
                throw new InvalidOperationException("Cannot perform cURL operation because the handle is not initialized.");

            int result = CurlInterop.curl_easy_perform(Pointer);
            if (result != 0)
                // throw new Exception($"cURL request failed with error code: {result}, and message : {CurlEnums.ErrorMessages[result]}");
                Console.WriteLine($"cURL request failed with error code: {result}, and message : {CurlEnums.ErrorMessages[result]}");
        }

        public void Dispose()
        {
            if (Pointer != IntPtr.Zero)
            {
                CurlInterop.curl_easy_cleanup(Pointer);
                Pointer = IntPtr.Zero;
            }
        }
    }
}
