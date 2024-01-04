using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Curly.LibCURL
{
    public static class CurlInterop
    {
        private const string libcurlName = "libcurl/libcurl-x64.dll";

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_init();

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_easy_cleanup(IntPtr curl);

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int curl_easy_setopt(IntPtr curl, int option, IntPtr param);

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int curl_easy_setopt(IntPtr curl, int option, string param);

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int curl_easy_perform(IntPtr curl);

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int curl_easy_getinfo(IntPtr curl, int info, ref IntPtr data);

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int curl_easy_getinfo(IntPtr curl, int info, ref long data);

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_version();

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_slist_append(IntPtr list, string header);

        [DllImport(libcurlName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_slist_free_all(IntPtr list);
    }
}
