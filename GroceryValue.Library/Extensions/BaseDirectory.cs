using System;

namespace GroceryValue.Library
{
    public static class BaseDirectory
    {
        public static string GetBaseDirectory()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            if (directory.IndexOf(@"\GroceryValue.Host", StringComparison.Ordinal) > 0)
            {
                directory = directory.Substring(0, directory.LastIndexOf(@"\GroceryValue.Host", StringComparison.Ordinal));
            }
            return directory;
        }
    }
}
