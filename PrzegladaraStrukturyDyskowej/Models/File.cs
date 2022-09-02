using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PrzegladaraStrukturyDyskowej.Models
{
    public class File
    {
        public Icon FileIcon { get; set; }
        public string Name { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string WeightByte { get; set; }
        public string FileType { get; set; }
        public string path { get; set; }
    }
}
