using System;
using System.IO;
using AssistidCollector2.Interfaces;
using AssistidCollector2.iOS.Implementations;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImplementationSaveLoad))]
namespace AssistidCollector2.iOS.Implementations
{
    public class ImplementationSaveLoad : InterfaceSaveLoad
    {
        public bool FileExists(string filename)
        {
            throw new NotImplementedException();
        }

        public string GetLocalFilePath(string filename)
        {
            /*
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(docFolder, filename);
            */

            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }

        public void InstallLocationFile(string filename)
        {
            throw new NotImplementedException();
        }

        public string LoadFile(string filename)
        {
            throw new NotImplementedException();
        }

        public void SaveFile(string filename, string text)
        {
            throw new NotImplementedException();
        }
    }
}
