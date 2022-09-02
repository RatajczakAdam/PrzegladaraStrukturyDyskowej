using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PrzegladaraStrukturyDyskowej.Models
{
    public class GetGata
    {
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
                Icon icon;

                filesInformation.Add(GetDirInfo(directory, s));
            }
            foreach (string s in fileArr)
            {
                FileInfo file = new FileInfo(s);
                filesInformation.Add(GetFileInfo(file,s));
            }
            return filesInformation;
        }
        private File GetFileInfo(FileInfo fileInfo, string filePath)
        {
            File info = new File {
                FileIcon = Icon.ExtractAssociatedIcon(filePath),
                Name = fileInfo.Name,
                LastWriteTime = fileInfo.LastWriteTime,
                FileType = fileInfo.Attributes.ToString(),
                WeightByte = fileInfo.Length
            };
            return info;
        }
        private File GetDirInfo(FileInfo dirInfo, string filePath)
        {
            File info = new File
            {
                FileIcon = Icon.ExtractAssociatedIcon(filePath),
                Name = dirInfo.Name,
                LastWriteTime = dirInfo.LastWriteTime,
                FileType = dirInfo.Attributes.ToString(),
            };
            return info;
        }
    }
}
