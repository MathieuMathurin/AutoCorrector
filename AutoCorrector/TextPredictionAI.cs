using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrector
{
    class TextPredictionAI
    {
        //todo
        Dictionary<string, NGram> startgrams = new Dictionary<string, NGram>();
        Dictionary<string, NGram> unigrams = new Dictionary<string, NGram>();
        Dictionary<string, NGram> bigrams = new Dictionary<string, NGram>();
        OrderedDictionary aGram = new OrderedDictionary();
        public List<NGram> GetSuggestions(string userInput)
        {
            string[] words = userInput.Split(' ');
            if (words.Length == 0)
            {
                return GetFirstWords()
            }
            return 
        }

        public List<NGram> GetFirstWords()
        {
            return startgrams.Take(20).ToDictionary();
        }

        public List<NGram> GetGrams(Dictionary<string, NGram> grams, string key)
        {
            if (!grams.ContainsKey(key)) return new List<NGram>();
            OrderedDictionary aGram = grams[key].dictionary;
            int nbrToGet;
            if (aGram.Count < 20)
            {
                nbrToGet = aGram.Count;
            }
            else
            {
                nbrToGet = 20;
            }
        }

    }
}
