using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Hyponome.Server.Utils
{
    public static class AssemblyExtensions
    {
        public static string GetFileVersion(this Assembly assembly)
        {
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.FullLocalPath());
            return fileVersionInfo.FileVersion;
        }

        public static string GetInformationalVersion(this Assembly assembly)
        {
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.FullLocalPath());
            return fileVersionInfo.ProductVersion;
        }
        
        static string FullLocalPath(this Assembly assembly)
        {
            var str = Uri.UnescapeDataString(new UriBuilder(assembly.CodeBase).Path);
            if (IsRunningOnWindows)
                str = str.Replace("/", "\\");
            return str;
        }
        
        static bool IsRunningOnWindows
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }
    }
}