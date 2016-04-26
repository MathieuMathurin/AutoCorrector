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
        public Dictionary<string, Sequence> dictionary { get; set;}
        public List<KeyValuePair<string, Sequence>> orderedSequence { get; set; }

        public int Sum()
        {
            if (dictionary == null) return Frequency+1;
            if (subSum == null)
            {
                subSum = dictionary.Values.Sum(x => x.Frequency);
            }
            return (int)subSum;
        }

        public void Sort()
        {
            if(dictionary != null)
            {
                if (orderedSequence == null)
                {
                    orderedSequence = dictionary.Select(kvp => kvp).OrderByDescending(kvp => kvp.Value.Frequency).ToList();
                }
                else
                {
                    orderedSequence = orderedSequence.OrderByDescending(kvp => kvp.Value.Frequency).ToList();
                }
            }            
        }
    }
}
