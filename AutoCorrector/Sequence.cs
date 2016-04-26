using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrector
{
    class Sequence
    {       
        public int Frequency { get; set; }
        private int subSum;
        public int maxFrequency { get; set; }
        public Dictionary<String, Sequence> dictionary { get; set;}

        public int Sum()
        {
            if (subSum == null)
            {
                subSum = dictionary.Values.Sum(x => x.Frequency);
            }
            return subSum;
        }
    }
}
