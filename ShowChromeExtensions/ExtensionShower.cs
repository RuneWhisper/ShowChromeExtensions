using Newtonsoft.Json;
using ShowChromeExtensions.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowChromeExtensions
{
    public class ExtensionShower
    {
        private readonly string _extensionFolderPath;

        public ExtensionShower(bool useChromium, string pathOverride = null)
        {
            _extensionFolderPath = pathOverride ?? DetectPath(useChromium);
        }

        private string DetectPath(bool useChromium)
        {
            List<string> winPaths = new List<string>()
            {
                @"C:\Users\%USERNAME%\AppData\Local\Google\Chrome\User Data\Default\Extensions",
                @"C:\Users\%USERNAME%\AppData\Local\Chromium\User Data\Default\Extensions"
            };

            List<string> macOsPaths = new List<string>()
            {
                "~/Library/Application Support/Google/Chrome/Default/Extensions",
                "~/Library/Application Support/Chromium/Default/Extensions"
            };

            List<string> linuxPaths = new List<string>()
            {
                "~/.config/google-chrome/Default/Extensions",
                "~/.config/chromium/Default/Extensions"
            };

            string username = Environment.UserName;
            string realPath = null;

            if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                foreach (string possiblePath in macOsPaths)
                {
                    if (Directory.Exists(possiblePath))
                    {
                        return possiblePath;
                    }
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                //Mono on Mac OS X returns Unix in some versions, so check for both
                foreach (string possiblePath in macOsPaths)
                {
                    if (Directory.Exists(possiblePath))
                    {
                        return possiblePath;
                    }
                }

                foreach (string possiblePath in linuxPaths)
                {
                    realPath = possiblePath.Replace("~", "/home/" + username);

                    if (Directory.Exists(realPath))
                    {
                        return realPath;
                    }
                }
            }
            else
            {
                foreach (string possiblePath in winPaths)
                {
                    realPath = possiblePath.Replace("%USERNAME%", username);

                    if (Directory.Exists(realPath))
                    {
                        return realPath;
                    }
                }
            }

            throw new NoExtensionFolderFoundException();
        }

        public List<string> GetInstalledExtensions()
        {
            Console.WriteLine("Searching for extensions in " + _extensionFolderPath);

            var res = new List<string>();

            IEnumerable<string> extensionFolders = Directory.EnumerateDirectories(_extensionFolderPath);

            var foldersToInspect = new List<string>();

            foreach(string extFolder in extensionFolders)
            {
                foldersToInspect.AddRange(Directory.EnumerateDirectories(extFolder));
            }

            foreach (string extVerFolder in foldersToInspect)
            {
                Console.WriteLine("Found: " + extVerFolder);

                string manifestFilePath = Path.Combine(extVerFolder, "manifest.json");

                if (!File.Exists(manifestFilePath))
                    continue;

                string manifestContents = File.ReadAllText(manifestFilePath);
                dynamic mainfestJson = JsonConvert.DeserializeObject(manifestContents);

                Console.WriteLine("\nName:" + mainfestJson.name);
                Console.WriteLine("Permissions:" + mainfestJson.permissions);
                Console.WriteLine();
            }

            return res;
        }
    }
}
