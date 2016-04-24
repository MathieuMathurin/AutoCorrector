using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrector
{
    class NGram
    {       
        public int Frequency { get; set; }
        public OrderedDictionary dictionary { get; set;}

        public NGram()
        {

        }

        public NGram(string definition)
        {
            int splitIndex = definition.LastIndexOf(',');
            this.Frequency = Int32.Parse(definition.Substring(splitIndex));           
        }

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
