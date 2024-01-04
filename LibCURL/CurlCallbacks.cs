using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Curly.LibCURL
{
    public class UploadData
    {
        public byte[] Data { get; set; }
        public int Position { get; set; }
    }
    public static class CurlCallbacks
    {
        public delegate int CurlWriteCallbackDelegate(IntPtr buffer, int size, int nmemb, IntPtr userdata);
        public delegate int CurlReadCallbackDelegate(IntPtr buffer, int size, int nmemb, IntPtr userdata);

        public static int WriteCallback(IntPtr buffer, int size, int nmemb, IntPtr userdata)
        {
            int realSize = size * nmemb;
            byte[] data = new byte[realSize];
            Marshal.Copy(buffer, data, 0, realSize);

            GCHandle gch = GCHandle.FromIntPtr(userdata);
            if (gch.Target is StringBuilder sb)
            {
                sb.Append(Encoding.UTF8.GetString(data));
            }

            return realSize;
        }

        public static int ReadCallback(IntPtr buffer, int size, int nmemb, IntPtr userdata)
        {
            GCHandle gch = GCHandle.FromIntPtr(userdata);
            if (gch.Target is UploadData uploadData)
            {
                int bufferLength = size * nmemb;
                int bytesToCopy = Math.Min(uploadData.Data.Length - uploadData.Position, bufferLength);
                Marshal.Copy(uploadData.Data, uploadData.Position, buffer, bytesToCopy);
                uploadData.Position += bytesToCopy;
                return bytesToCopy;
            }

            return 0;
        }
    }
}
