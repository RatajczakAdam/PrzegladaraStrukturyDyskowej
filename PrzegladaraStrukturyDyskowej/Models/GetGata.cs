using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PrzegladaraStrukturyDyskowej.Models
{
    public class GetGata
    {
        private string root = ConfigurationManager.AppSettings["root"];
        public List<File> GetValues(string path)
        {
            List<File> filesInformation = new List<File>();
            try
            {
                string[] files = Directory.GetFiles(path);
                string[] dir = Directory.GetDirectories(path);
                filesInformation = Makelist(files, dir);
            }
            catch { }
            if (filesInformation.Count == 0 )
            {
                char[] mineRoot = root.ToCharArray();
                string pathDict = path.TrimStart(mineRoot);
                File info = new File { Path = pathDict };
                filesInformation.Add(info);
            }
            return filesInformation;
        }
        private List<File> Makelist(string[] fileArr, string[] dirArr)
        {
            List<File> filesInformation = new List<File>();
            foreach (string s in dirArr)
            {
                FileInfo directory = new FileInfo(s);
                File dir = GetDirInfo(directory,s);
                dir.Id = filesInformation.Count + 1;
                filesInformation.Add(dir);
            }
            foreach (string s in fileArr)
            {
                FileInfo file = new FileInfo(s);
                File fil = GetFileInfo(file,s);
                fil.Id = filesInformation.Count + 1;
                filesInformation.Add(fil);
            }
            return filesInformation;
        }
        private File GetFileInfo(FileInfo fileInfo,string path)
        {
            char[] mineRoot = root.ToCharArray();
            char[] namefile = fileInfo.Name.ToCharArray();
            File info = new File {
                Name = fileInfo.Name,
                LastWriteTime = fileInfo.LastWriteTime,
                FileType = Path.GetExtension(path),
                WeightByte = fileInfo.Length.ToString()+" b",
                Path = fileInfo.FullName.TrimStart(mineRoot).TrimEnd(namefile),
                Atributes = fileInfo.Attributes.ToString()
            };
            return info;
        }
        private File GetDirInfo(FileInfo dirInfo, string path)
        {
            char[] mineRoot = root.ToCharArray();
            char[] nameDir = dirInfo.Name.ToCharArray();
            File info = new File
            {
                Name = dirInfo.Name,
                LastWriteTime = dirInfo.LastWriteTime,
                Path = dirInfo.FullName.TrimStart(mineRoot).TrimEnd(nameDir),
                Atributes = dirInfo.Attributes.ToString()
            };
            return info;
        }
        public string GetOldPatch(string path)
        {
            string newPath="";
            int lastValue=0;
            char[] charPath = path.ToCharArray();
            char[] testValue = @"\".ToCharArray();

            for (int i = charPath.Length ; i >0; i--)
            {
                
                if (i<2) break;
                else if (charPath[i-2]== testValue[0])
                {
                    lastValue = i-1 ;
                    break;
                }
            }
            for (int i = 0; i < lastValue; i++)
            {
                newPath = newPath + charPath[i];
            }
            return newPath;
        }

    }
}
