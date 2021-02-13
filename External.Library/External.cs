using System.IO;

namespace Externals
{
    public static class External
    {
        public static string GetExternalFolderPath()
        {
            return Directory.GetCurrentDirectory() + @"\..\..\..\..\..\External\";
        }
    }
}
