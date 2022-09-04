using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PrzegladaraStrukturyDyskowej.Models
{
    public class File
    {
        public int Id { get; set; }
        [DisplayName("Nazwa pliku")]
        public string Name { get; set; }
        [DisplayName("Ostatnia data edycji")]
        public DateTime LastWriteTime { get; set; }
        [DisplayName("Rozmiar")]
        public string WeightByte { get; set; }
        public string FileType { get; set; }
        public string Path { get; set; }
        public string Atributes { get; set; }
    }
}
