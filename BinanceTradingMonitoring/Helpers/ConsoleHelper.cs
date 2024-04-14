
using System.Runtime.InteropServices;

namespace BinanceTradingMonitoring.Helpers
{
    /// <summary>
    /// Provides helper methods for console handling.
    /// </summary>
    public class ConsoleHelper
    {
        // Import Windows API functions
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
        

        /// <summary>
        ///  Disable Quick Edit Mode to prevent stopping when clicking on the console
        /// </summary>
        public static void DisableQuickEditMode()
        {
            IntPtr consoleHandle = GetStdHandle(Constant.STD_INPUT_HANDLE);
            if (consoleHandle != IntPtr.Zero)
            {
                // Get current console mode
                if (GetConsoleMode(consoleHandle, out uint consoleMode))
                {
                    // Disable quick edit mode and extended flags
                    consoleMode = consoleMode & ~(Constant.ENABLE_QUICK_EDIT_MODE | Constant.ENABLE_EXTENDED_FLAGS);
                    SetConsoleMode(consoleHandle, consoleMode);
                }
            }
        }
    }
}
