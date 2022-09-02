using DocumentFormat.OpenXml.Office.CustomUI;
using Google.Apis.Compute.v1.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

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
            char[] MyChar0 = root.ToCharArray();
            char[] MyChar1 = fileInfo.Name.ToCharArray();
            File info = new File {
                FileIcon = Icon.ExtractAssociatedIcon(filePath),
                Name = fileInfo.Name,
                LastWriteTime = fileInfo.LastWriteTime,
                FileType = fileInfo.Attributes.ToString(),
                WeightByte = fileInfo.Length.ToString(),
                path=fileInfo.FullName.TrimStart(MyChar0).TrimEnd(MyChar1)
            };
            return info;
        }
        private File GetDirInfo(FileInfo dirInfo, string filePath)
        {
            char[] MyChar0 = root.ToCharArray();
            char[] MyChar1 = dirInfo.Name.ToCharArray();
            File info = new File
            {
                //FileIcon = Icon.ExtractAssociatedIcon(filePath),
                Name = dirInfo.Name,
                LastWriteTime = dirInfo.LastWriteTime,
                FileType = dirInfo.Attributes.ToString(),
                path = dirInfo.FullName.TrimStart(MyChar0).TrimEnd(MyChar1)
            };
            return info;
        }
    }
}
