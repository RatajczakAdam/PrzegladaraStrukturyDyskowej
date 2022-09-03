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
            string[] files = Directory.GetFiles(path);
            string[] dir = Directory.GetDirectories(path);
            List<File> filesInformation = Makelist(files, dir);
            return Makelist(files, dir);
        }
        private List<File> Makelist(string[] fileArr, string[] dirArr)
        {
            List<File> filesInformation = new List<File>();
            foreach (string s in dirArr)
            {
                FileInfo directory = new FileInfo(s);
                File dir = GetDirInfo(directory, s);
                dir.id = filesInformation.Count + 1;
                filesInformation.Add(dir);
            }
            foreach (string s in fileArr)
            {
                FileInfo file = new FileInfo(s);
                File fil = GetFileInfo(file, s);
                fil.id = filesInformation.Count + 1;
                filesInformation.Add(fil);
            }
            return filesInformation;
        }
        private File GetFileInfo(FileInfo fileInfo, string filePath)
        {
            char[] mineRoot = root.ToCharArray();
            char[] namefile = fileInfo.Name.ToCharArray();
            File info = new File {
                //FileIcon = Icon.ExtractAssociatedIcon(filePath),
                Name = fileInfo.Name,
                LastWriteTime = fileInfo.LastWriteTime,
                FileType = fileInfo.Attributes.ToString(),
                WeightByte = fileInfo.Length.ToString(),
                path=fileInfo.FullName.TrimStart(mineRoot).TrimEnd(namefile)
            };
            return info;
        }
        private File GetDirInfo(FileInfo dirInfo, string filePath)
        {
            char[] mineRoot = root.ToCharArray();
            char[] nameDir = dirInfo.Name.ToCharArray();
            File info = new File
            {
                //FileIcon = Icon.ExtractAssociatedIcon(filePath),
                Name = dirInfo.Name,
                LastWriteTime = dirInfo.LastWriteTime,
                FileType = dirInfo.Attributes.ToString(),
                path = dirInfo.FullName.TrimStart(mineRoot).TrimEnd(nameDir)
            };
            return info;
        }
    }
}
