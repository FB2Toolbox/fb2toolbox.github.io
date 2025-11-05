using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FB2Toolbox.Utilities
{
    internal static class NativeMethods
    {
        #region Private
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        private static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32.dll")]
        private static extern bool FindClose(IntPtr hFindFile);
        #endregion
        public static bool CheckDirectoryEmpty_Fast(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(path);
            }

            if (Directory.Exists(path))
            {
                if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    path += "*";
                }
                else
                {
                    path += Path.DirectorySeparatorChar + "*";
                }

                IntPtr findHandle = FindFirstFile(path, out WIN32_FIND_DATA findData);

                if (findHandle != INVALID_HANDLE_VALUE)
                {
                    try
                    {
                        bool empty = true;
                        do
                        {
                            if (findData.cFileName != "." && findData.cFileName != "..")
                            {
                                empty = false;
                            }
                        } while (empty && FindNextFile(findHandle, out findData));

                        return empty;
                    }
                    finally
                    {
                        _ = FindClose(findHandle);
                    }
                }

                throw new Exception("Failed to get directory first file");
            }

            throw new DirectoryNotFoundException();
        }
    }
}
