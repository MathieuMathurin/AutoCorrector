using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            string[] words = inputText.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
           
            results = new OrderedDictionary();

            //L'ordre des appels, fait l'ordre de recherche de suggestions

            //A voir comment on defini quand on a une fin de ligne dans un message
            //AddSuggestionsFromUniGram(knowledge.nGramDebutPhrase);

            AddSuggestionsPerso(words);

            if(results.Count < 20)
            {
                AddSuggestionsPublic(words);
            }

            AddSuggestionsFromUniGram(knowledge.nGramsPerso[0]);

            //Cas ultime TODO: Definir quand on l'appel
            //AddSuggestionsFromUniGram(knowledge.nGramsPublic[0]);

            return results;
        }

        private void AddSuggestionsPerso(string[] words)
        {
            int nbrWords = words.Length; //5
            if (nbrWords + 1 >= knowledge.nGramsPerso.Count)//6 >= 6
            {
                nbrWords = knowledge.nGramsPerso.Count - 2; // = 4, permettra de regarder dans nGramsPerso[4+1]
            }
            //Cherche dans tous les nGrams sauf unigram et unigram de debut de phrase
            AddSuggestionsFromNGrams(nbrWords, words, knowledge.nGramsPerso);
        }

        private void AddSuggestionsPublic(string[] words)
        {
            int nbrWords = words.Length;
            if (nbrWords + 1 >= knowledge.nGramsPublic.Count)
            {
                nbrWords = knowledge.nGramsPublic.Count - 2;
            }
            //Cherche dans tous les nGrams sauf unigram
            AddSuggestionsFromNGrams(nbrWords, words, knowledge.nGramsPublic);
        }

        //cherche dans les nGram > 1Gram
        private void AddSuggestionsFromNGrams(int nbrWords, string[] words, List<NGram> nGrams)
        {
            for (int i = nbrWords; i > 0; i--)
            {
                string[] wordsSelection = words.Skip(words.Length - i).Take(i).ToArray();
                string inputTextSelection = string.Join(" ", wordsSelection).Trim();
                if (nGrams[i].dictionary.ContainsKey(inputTextSelection))
                {
                    int count = 0;
                    nGrams[i].dictionary[inputTextSelection].Sort();
                    foreach (KeyValuePair<string, Sequence> entry in nGrams[i].dictionary[inputTextSelection].orderedSequence)
                    {
                        //Should first take the most frequent x keys
                        //calcul de probabilit/ de base
                        if (results.Contains(entry.Key) == false)
                        {
                            results.Add(entry.Key, ComputeProbability(inputTextSelection, entry.Key, nGrams, i));
                        }
                        count += 1;
                        if (count >= 100) break;
                    }
                }
            }
        }

        private void AddSuggestionsFromUniGram(NGram nGram)
        {
            int count = 0;
            nGram.Sort();
            foreach(KeyValuePair<string, Sequence> entry in nGram.orderedSequence)
            {
                //Should first take the most frequent x keys
                //calcul de probabilit/ de base
                if (results.Contains(entry.Key) == false)
                {
                    results.Add(entry.Key, entry.Value.Frequency / nGram.Sum());
                }
                count += 1;
                if (count >= 100) break;
            }
        }

        public string GetStartOfWord(string userInput)
        {
            string inputText = NormalizeInput(userInput);
            string[] seperators = { " " };
            string[] words = inputText.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            return words.Last();
        }

        //Returns a list of frequent words that start sentences
        public OrderedDictionary GetFirstWords(NgramsParser knowledge)
        {
            OrderedDictionary results = new OrderedDictionary();
            int count = 0;
            foreach(KeyValuePair<string, Sequence> entry in knowledge.nGramDebutPhrase.orderedSequence)
            {
                if(entry.Key.StartsWith("!") == false && entry.Key.StartsWith("?") == false && entry.Key.StartsWith(".") == false)
                {
                int sum = knowledge.nGramsPerso[0].Sum();
                int freq = entry.Value.Frequency;
                double value = (double)freq / sum;
                results.Add(FirstLetterUppercase(entry.Key), value);
                count += 1;
                if (count >= 100) break;
            }
                
            }
            return results;
        }

        //Recoit "unmot" retourne "Unmot"
        private string FirstLetterUppercase(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }


        private double ComputeProbability(string inputText, string word, List<NGram> nGrams, int inputLength)
        {
            //Juste reverifier ici si tout est ok
            if (String.IsNullOrEmpty(inputText))
            {
                return (double) (nGrams[0].dictionary[word].Frequency / nGrams[0].Sum());
            }
            double probability = nGrams[inputLength].dictionary[inputText].dictionary[word].Frequency / nGrams[inputLength].dictionary[inputText].Sum();
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
            return text.Length == 0 || text.Last() == '.' || text.Last() == '!' || text.Last() == '?' || input.Contains(" ")==false;
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
            //Deal with multiple dots
            //string pattern = ".+";
            //string replacement = " ... ";
            //Regex rgx = new Regex(pattern);
            //userInput = rgx.Replace(userInput, replacement);

            //string singleDotPattern = "(?<=[^\\.])\\.{1}(?!\\.)";
            //string singleDotReplacement = " . ";
            //rgx = new Regex(singleDotPattern);
            //userInput = rgx.Replace(userInput, singleDotReplacement);
            string[] res;


            res = System.Text.RegularExpressions.Regex.Split(userInput, @"\s{2,}");
            return string.Join(" ", res).Trim().ToLower();
        }
    }
}
