using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Setup
{
    public class SetupHelper
    {

        public static int CompareFileNames(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.CompareTo(f2.FullName);
        }

        public static Version ParseVersionFromFileName(String fileName)
        {
            Version version = null;

            int major = 0;
            int minor = 0;
            int build = 0;
            int revision = 0;
            bool success = true;


            if (fileName != null)
            {
                char[] separator = { '.' };
                string[] args = fileName.Replace(".config", String.Empty).Split(separator);
                if (args.Length >= 4)
                {
                    if (!(int.TryParse(args[0], out major)))
                    {
                        major = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[1], out minor)))
                    {
                        minor = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[2], out build)))
                    {
                        build = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[3], out revision)))
                    {
                        revision = 0;
                        success = false;
                    }

                    if (success)
                    {
                        version = new Version(major, minor, build, revision);
                    }

                }

            }


            return version;

        }
    }
}
