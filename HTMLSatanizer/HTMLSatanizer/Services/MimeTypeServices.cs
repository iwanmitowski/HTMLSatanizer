using HTMLSatanizer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services
{
    public class MimeTypeServices:IMimeTypeServices
    {
        private static List<string> knownTypes;

        private static Dictionary<string, string> mimeTypes;

        public MimeTypeServices()
        {
            mimeTypes = new Dictionary<string, string>();
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Auto)]
        private static extern UInt32 FindMimeFromData(
            UInt32 pBC, [MarshalAs(UnmanagedType.LPStr)]
        string pwzUrl, [MarshalAs(UnmanagedType.LPArray)]
        byte[] pBuffer, UInt32 cbSize, [MarshalAs(UnmanagedType.LPStr)]
        string pwzMimeProposed, UInt32 dwMimeFlags, ref UInt32 ppwzMimeOut, UInt32 dwReserverd
        );

        public string GetContentType(string fileName)
        {
            if (knownTypes == null || mimeTypes == null)
                InitializeMimeTypeLists();
            string contentType = "";
            string extension = System.IO.Path.GetExtension(fileName).Replace(".", "").ToLower();
            mimeTypes.TryGetValue(extension, out contentType);
            if (string.IsNullOrEmpty(contentType) || knownTypes.Contains(contentType))
            {
                string headerType = ScanFileForMimeType(fileName);
                if (headerType != "application/octet-stream" || string.IsNullOrEmpty(contentType))
                    contentType = headerType;
            }
            return contentType;
        }

        private string ScanFileForMimeType(string fileName)
        {
            try
            {
                byte[] buffer = new byte[256];
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    int readLength = Convert.ToInt32(Math.Min(256, fs.Length));
                    fs.Read(buffer, 0, readLength);
                }

                UInt32 mimeType = default(UInt32);

                FindMimeFromData(0, null, buffer, 256, null, 0, ref mimeType, 0);

                IntPtr mimeTypePtr = new IntPtr(mimeType);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);

                Marshal.FreeCoTaskMem(mimeTypePtr);

                if (string.IsNullOrEmpty(mime))
                {
                    mime = "application/octet-stream";

                }
                return mime;
            }
            catch (Exception ex)
            {
                return "application/octet-stream";
            }
        }

        private void InitializeMimeTypeLists()
        {
            knownTypes = new string[] {
            "text/plain",
            "text/html",
            "text/xml",
            "text/richtext",
        }.ToList();

            mimeTypes.Add("text", "text/plain");
            mimeTypes.Add("acgi", "text/html");
            mimeTypes.Add("htm", "text/html");
            mimeTypes.Add("html", "text/html");
            mimeTypes.Add("htmls", "text/html");
            mimeTypes.Add("htt", "text/webviewhtml");
            mimeTypes.Add("htx", "text/html");
            mimeTypes.Add("shtml", "text/html");
            mimeTypes.Add("ssi", "text/x-server-parsed-html");
            mimeTypes.Add("abc", "text/vnd.abc");
            mimeTypes.Add("aip", "text/x-audiosoft-intra");
            mimeTypes.Add("asm", "text/x-asm");
            mimeTypes.Add("asp", "text/asp");
            mimeTypes.Add("c", "text/plain");
            mimeTypes.Add("c++", "text/plain");
            mimeTypes.Add("cc", "text/plain");
            mimeTypes.Add("conf", "text/plain");
            mimeTypes.Add("cpp", "text/x-c");
            mimeTypes.Add("css", "text/css");
            mimeTypes.Add("def", "text/plain");
            mimeTypes.Add("etx", "text/x-setext");
            mimeTypes.Add("f", "text/plain");
            mimeTypes.Add("f90", "text/x-fortran");
            mimeTypes.Add("for", "text/x-fortran");
            mimeTypes.Add("g", "text/plain");
            mimeTypes.Add("h", "text/plain");
            mimeTypes.Add("hh", "text/plain");
            mimeTypes.Add("htc", "text/x-component");
            mimeTypes.Add("idc", "text/plain");
            mimeTypes.Add("jav", "text/plain");
            mimeTypes.Add("java", "text/plain");
            mimeTypes.Add("list", "text/plain");
            mimeTypes.Add("log", "text/plain");
            mimeTypes.Add("lst", "text/plain");
            mimeTypes.Add("m", "text/plain");
            mimeTypes.Add("mar", "text/plain");
            mimeTypes.Add("pl", "text/plain");
            mimeTypes.Add("sdml", "text/plain");
            mimeTypes.Add("talk", "text/x-speech");
            mimeTypes.Add("tsv", "text/tab-separated-values");
            mimeTypes.Add("txt", "text/plain");
            mimeTypes.Add("xml", "text/xml");
        }
    }
}
