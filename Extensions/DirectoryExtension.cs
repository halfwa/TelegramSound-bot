using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSound.Extensions
{
    public static class DirectoryExtension
    {
        public static string GetProjectPath()
        {
            return Path.GetFullPath(Directory.GetCurrentDirectory() + $@"\..\..\..\..\..\");
        }
    }
}
