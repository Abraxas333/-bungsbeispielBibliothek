using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÜbungsbeispielBibliothek.Model
{
    public class Buch
    {
        public int BuchID { get; set; }
        public string Titel {  get; set; }
        public string Autor { get; set; }
        public int Jahr { get; set; }
        public float Preis {  get; set; }
        public string Genre { get; set; }
        public int VerlagID {  get; set; }

    }
}

