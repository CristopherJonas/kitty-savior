using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MataGato.Models
{
    public class SorterWord
    {
        public string Word { get; set; }
        public int Position { get; set; }

        public SorterWord(string word, int position)
        {
            Word = word;
            Position = position;
        }
    }
}
