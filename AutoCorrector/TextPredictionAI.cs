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
        NgramsParser knowledge;
        OrderedDictionary results;

        public TextPredictionAI(NgramsParser parser)
        {
            this.knowledge = parser;
        }

        public OrderedDictionary GetSuggestions(string userInput)
        {

            if(userInput == null || userInput.Trim().Length == 0 ||
                IsNewSentence(userInput))
            {
                return GetFirstWords(knowledge);
            }
            string inputText = NormalizeInput(userInput);
            string[] seperators = { " " };
            string[] words = userInput.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
           
            results = new OrderedDictionary();

            AddSuggestionsPerso(words);

            return results;
        }

        private void AddSuggestionsPerso(string[] words)
        {
            int nbrWords = words.Length;
            if (nbrWords + 1 >= knowledge.nGramsPerso.Count)
            {
                nbrWords = knowledge.nGramsPerso.Count - 2;
            }
            for (int i = nbrWords; i > 0; i--)
            {
                string[] wordsSelection = words.Skip(words.Length - i).Take(i).ToArray();
                string inputTextSelection = string.Join(" ", wordsSelection).Trim();
                if (knowledge.nGramsPerso[i + 1].dictionary.ContainsKey(inputTextSelection))
                {
                    int count = 0;
                    foreach (KeyValuePair<string, Sequence> entry in knowledge.nGramsPerso[i + 1].dictionary[inputTextSelection].dictionary)
                    {
                        //Should first take the most frequent x keys
                        //calcul de probabilit/ de base
                        if (results.Contains(entry.Key) == false)
                        {
                            results.Add(entry.Key, ComputeProbability(inputTextSelection, entry.Key, knowledge.nGramsPerso, i));
                        }
                        count += 1;
                        if (count >= 100) break;
                    }
                }
            }
        }

        private void AddSuggestionsPublic(string[] words)
        {
            //Work in progress
            int nbrWords = words.Length;
            if (nbrWords + 1 >= knowledge.nGramsPublic.Count)
            {
                nbrWords = knowledge.nGramsPerso.Count - 2;
            }
            for (int i = nbrWords; i > 0; i--)
            {
                string[] wordsSelection = words.Skip(words.Length - i).Take(i).ToArray();
                string inputTextSelection = string.Join(" ", wordsSelection).Trim();
                if (knowledge.nGramsPerso[i + 1].dictionary.ContainsKey(inputTextSelection))
                {
                    int count = 0;
                    foreach (KeyValuePair<string, Sequence> entry in knowledge.nGramsPerso[i + 1].dictionary[inputTextSelection].dictionary)
                    {
                        //Should first take the most frequent x keys
                        //calcul de probabilit/ de base
                        if (results.Contains(entry.Key) == false)
                        {
                            results.Add(entry.Key, ComputeProbability(inputTextSelection, entry.Key, knowledge.nGramsPerso, i));
                        }

                        count += 1;
                        if (count >= 100) break;
                    }
                }
            }
        }

        public string GetStartOfWord(string userInput)
        {
            string inputText = NormalizeInput(userInput);
            string[] seperators = { " " };
            string[] words = userInput.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            return words.Last();
        }

        //Returns a list of frequent words that start sentences
        public OrderedDictionary GetFirstWords(NgramsParser knowledge)
        {
            OrderedDictionary results = new OrderedDictionary();
            int count = 0;
            foreach(KeyValuePair<string, Sequence> entry in knowledge.nGramsPerso[0].dictionary)
            {
                if(entry.Key.StartsWith("!") == false && entry.Key.StartsWith("?") == false && entry.Key.StartsWith(".") == false)
                {
                    int sum = knowledge.nGramsPerso[0].Sum();
                    int freq = entry.Value.Frequency;
                    double value = (double)freq / sum;
                    results.Add(entry.Key, value);
                    count += 1;
                    if (count >= 100) break;
                }
                
            }
            return results;
        }

        
        private double ComputeProbability(string inputText, string word, List<NGram> nGrams, int inputLength)
        {

            double probability = nGrams[inputLength + 1].dictionary[inputText].dictionary[word].Frequency / nGrams[inputLength + 1].dictionary[inputText].Sum();
            string[] userWords = inputText.Split(' ');
            double discount = 0.9;
            for(int i = inputLength-1; i >= 0; i--)
            {
                string[] wordsSelection = userWords.Skip(userWords.Length - i).Take(i).ToArray();
                string key = string.Join(" ", wordsSelection).Trim();
                if (nGrams[i+1].dictionary.ContainsKey(key) && nGrams[i+1].dictionary[key].dictionary.ContainsKey(word))
                {
                    probability += (nGrams[i + 1].dictionary[key].dictionary[word].Frequency / nGrams[i + 1].dictionary[key].Sum()) * discount;
                }
                
                discount *= discount;
            }
            return probability;

        }
        
        private bool IsNewSentence(string input)
        {
            if (input == null) return false;
            string text = input.Trim();
            return text.Length == 0 || text.Last() == '.' || text.Last() == '!' || text.Last() == '?';
        }

        private string NormalizeInput(string userInput)
        {
            userInput = userInput.Replace(",", " , ");
            userInput = userInput.Replace("!", " ! ");
            userInput = userInput.Replace("*", " * ");
            userInput = userInput.Replace("\"", " \" ");
            userInput = userInput.Replace("!", " ! ");
            //Temporary solution
            //Must deal with with triple dots first
            //then dots
            userInput = userInput.Replace(".", " . ");
            string[] res;

            res = System.Text.RegularExpressions.Regex.Split(userInput, @"\s{2,}");
            return string.Join(" ", res).Trim();
        }
    }
}
