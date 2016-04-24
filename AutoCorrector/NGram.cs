using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrector
{
    class NGram
    {       
        public int Frequency { get; set; }
        public Dictionary<String, NGram> dictionary { get; set;}

        //public bool Contains(string someText)
        //{
        //    return this.text.Contains(someText);
        //}

        //public bool Equals(string other)
        //{
        //    return this.text == other;
        //}

        //public bool Equals(NGram other)
        //{
        //    return this.text == other.text;            
        //}

    }
}
