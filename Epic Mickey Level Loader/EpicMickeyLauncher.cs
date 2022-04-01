using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Mickey_Level_Loader
{
    public static class EpicMickeyLauncher
    {
        public static bool CheckForDeletedFile(string path)
        {
            return !File.Exists(path);
        }
        public static bool CheckForDeletedFolder(string path)
        {
            return !Directory.Exists(path);
        }
    }
}
