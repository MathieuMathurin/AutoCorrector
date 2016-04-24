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
        int frequency;

        public NGram(string definition)
        {
            int splitIndex = definition.LastIndexOf(',');
            this.frequency = Int32.Parse(definition.Substring(splitIndex));
            this.text = definition.Substring(0, splitIndex);
        }

        public bool Contains(string someText)
        {
            return this.text.Contains(someText);
        }

        public int GetFrequency()
        {
            return this.frequency;
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
