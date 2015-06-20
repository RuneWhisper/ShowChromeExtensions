using ShowChromeExtensions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowChromeExtensions.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && (args[0] == "--help" || args[0] == @"\?"))
            {
                PrintUsage();
                return;
            }

            bool useChromium = false;

            try
            {
                var extensionShower = new ExtensionShower(useChromium);

                var extensions = extensionShower.GetInstalledExtensions();
            }
            catch (NoExtensionFolderFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.Read();
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: ShowChromeExtensions | pathOverride | useChromium");
            Console.WriteLine("Searches for all Chrome/Chromium extensions installed (extenstion manager's list is unreliable)");
            Console.WriteLine("useChromium: prefer path for chromium browser (in case you've both browsers installed)");
            Console.WriteLine("pathOverride: provide custom path to extensions folder");
        }
    }
}
