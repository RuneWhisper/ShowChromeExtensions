using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowChromeExtensions.Exceptions
{
    public class NoExtensionFolderFoundException : Exception
    {
        public NoExtensionFolderFoundException() 
            : base("Error: no Chrome/Chromium extension folder found. If you're sure it's installed, try manual path override") { }

        public NoExtensionFolderFoundException(string msg) : base(msg) { }
    }
}
