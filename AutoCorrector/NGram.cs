using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrector
{
    class NGram
    {
        string text;
        int Frequency { get; set; }

        public NGram(string definition)
        {
            int splitIndex = definition.LastIndexOf(',');
            this.Frequency = Int32.Parse(definition.Substring(splitIndex));
            this.text = definition.Substring(0, splitIndex);
        }

        public bool Contains(string someText)
        {
            return this.text.Contains(someText);
        }

        public bool Equals(string other)
        {
            return this.text == other;
        }

        public bool Equals(NGram other)
        {
            return this.text == other.text;
        }
    }
}
