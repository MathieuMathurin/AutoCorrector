using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrector
{
    class NGram
    {
        int? totalSum;
        public Dictionary<string, Sequence> dictionary { get; set; }
        public List<KeyValuePair<string, Sequence>> orderedSequence { get; set; }

        public NGram()
        {
            this.dictionary = new Dictionary<string, Sequence>();
        }

        public int Sum()
        {
            if(totalSum == null)
            {
                totalSum = dictionary.Values.Sum(x => x.Sum());
            }
            return (int)totalSum;
        }

        public void Sort()
        {
            if(orderedSequence == null)
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
