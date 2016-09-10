using System.IO;

namespace GroceryValue.Library
{
    public static class Logger
    {
        private static bool _isInitialized;

        public static void LogHandler(string message)
        {
            var log = $@"{BaseDirectory.GetBaseDirectory()}\GroceryValue.Extensions\Log.txt";
            if (_isInitialized == false)
            {
                File.WriteAllText(log, string.Empty);
                _isInitialized = true;
            }
            using (var file = File.AppendText(log))
            {
                file.WriteLine(message);
            }
        }
    }
}
