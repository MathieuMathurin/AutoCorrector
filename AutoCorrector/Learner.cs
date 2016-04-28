using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCorrector
{
    class Learner
    {
        string[] endOfSentences = { "!", "?", ".", "..." };
        //string end
        public void GramsFromMessage(int gramsize, string message, NGram gramCollection)
        {
            //clef, mot, frequence
            //List<KeyValuePair<string,Sequence>> newGrams = new List<KeyValuePair<string, Sequence>>();
            message = NormalizeInput(message);
            string[] seperators = { " " };
            string[] words = message.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            for(int i=0; i <= words.Length - gramsize; i++)
            {
                string key = "";
                string word = "";
                if (gramsize - 1 > 0)
                { //nGrams
                    key = String.Join(" ", words.Skip(i).Take(gramsize - 1));
                    word = words[i + gramsize - 1];
                    if (gramCollection.dictionary.ContainsKey(key))
                    {
                        if (gramCollection.dictionary[key].dictionary.ContainsKey(word))
                        {
                            gramCollection.dictionary[key].dictionary[word].Frequency += 3;
                            gramCollection.dictionary[key].dictionary[word].Sort(false);
                        }
                        else
                        {
                            Sequence newSequence = new Sequence();
                            newSequence.Frequency = 5;
                            gramCollection.dictionary[key].dictionary.Add(word, newSequence);
                        }
                    }else
                    {
                        Sequence newKey = new Sequence();
                        Sequence newSequence = new Sequence();
                        newSequence.Frequency = 5;

                        newKey.dictionary = new Dictionary<string, Sequence>();
                        newKey.dictionary.Add(word,newSequence);
                        gramCollection.dictionary.Add(key, newKey);
                    }
                } //Unigrams
                else
                {
                    key = words[i];
                    if (gramCollection.dictionary.ContainsKey(key))
                    {
                        gramCollection.dictionary[key].Frequency += 3;
                    }
                    else
                    {
                        Sequence newSequence = new Sequence();
                        newSequence.Frequency = 5;
                        gramCollection.dictionary.Add(key,newSequence);
                    }
                }

            }

        }

        public void StarterGramsFromMessage(string message, NGram starterGrams)
        {
            message = NormalizeInput(message);
            string[] seperators = { " " };
            string[] words = message.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            bool prevWordIsEndOfSentence = false;

            if (words.Length == 0 || words == null) return;

            for(int i = 0; i< words.Length; i++)
            {
                if (i == 0) AddStarterGram(words[i], starterGrams);
                if (IsEndOfSentence(words[i])) prevWordIsEndOfSentence = true;
                else if (prevWordIsEndOfSentence) AddStarterGram(words[i], starterGrams);
            }

        }

        private void AddStarterGram(string word, NGram starterGrams)
        {
            if(starterGrams.dictionary.ContainsKey(word))
            {
                starterGrams.dictionary[word].Frequency += 3;
            }
            else
            {
                Sequence newKey = new Sequence();
                newKey.Frequency = 5;
                starterGrams.dictionary.Add(word, newKey);
            }
        }

        private bool IsEndOfSentence(string word)
        {
            return endOfSentences.Contains(word);
        }

        private string NormalizeInput(string userInput)
        {
            userInput = userInput.Replace(",", " , ");
            userInput = userInput.Replace("!", " ! ");
            userInput = userInput.Replace("*", " * ");
            userInput = userInput.Replace("\"", " \" ");
            userInput = userInput.Replace("!", " ! ");

            //Deal with multiple dots
            string pattern = "\\.+";
            string replacement = " ... ";
            Regex rgx = new Regex(pattern);
            userInput = rgx.Replace(userInput, replacement);
            //Deal with single dots
            string singleDotPattern = "(?<=[^\\.])\\.{1}(?!\\.)";
            string singleDotReplacement = " . ";
            rgx = new Regex(singleDotPattern);
            userInput = rgx.Replace(userInput, singleDotReplacement);

            string[] res;
            res = System.Text.RegularExpressions.Regex.Split(userInput, @"\s{2,}");
            return string.Join(" ", res).Trim().ToLower();
        }
    }

    
}
