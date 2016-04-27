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
        public OrderedDictionary GetSuggestions(string userInput, NgramsParser knowledge)
        {
            //Ne comporte pas les cas en milieu de mot
            //
            //Normaliser user input
            //S'il y a rien dans userInput, retourner retourner debuts de phrase les plus frequents

            //S'il y a n mots, checher (n+1)grams,(n)grams,(n-1)grams, etc
            //Pour chaque type de (x)grams:
            //  prendre x derniers mots
            //      acceder au NGram
            //      prendre ses 20 plus frequents
            //          (peut etre que la probabilite devrait etre ajustee en fonction de taille du gram/ nombre de mots dans le userinput)
            //      pour chaque plus frequent, calculer probabilite et retourner une liste de mot manquant et sa probabilite
            //

            //calcul de probabilite
            // prob=0
            // Jusqua ce que la clef n'ait qu'un mot:
            //  prob+=frequence du mot / frequence de "la clef"
            //  descend de gram et "pop" le premier mot de la clef

            if(userInput == null || userInput.Trim().Length == 0)
            {
                return GetFirstWords(knowledge);
            }
            string inputText = NormalizeInput(userInput);
            string[] seperators = { " " };
            string[] words = userInput.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            int nbrWords = words.Length;
            if (nbrWords+1 >= knowledge.nGramsPerso.Count) {
                nbrWords = knowledge.nGramsPerso.Count-2;
            }

            OrderedDictionary results = new OrderedDictionary();
            for(int i = nbrWords; i > 0; i--)
            {
                string[] wordsSelection = words.Skip(words.Length - i).Take(i).ToArray();
                string inputTextSelection = string.Join(" ", wordsSelection).Trim();
                //temporary, les clefs des dict ont des espaces
                inputTextSelection = inputTextSelection;
                if (knowledge.nGramsPerso[i + 1].dictionary.ContainsKey(inputTextSelection))
                {
                    int count = 0;
                    foreach(KeyValuePair<string, Sequence> entry in knowledge.nGramsPerso[i + 1].dictionary[inputTextSelection].dictionary)
                    {
                        //Should first take the most frequent x keys
                        //calcul de probabilit/ de base
                        if( results.Contains(entry.Key) == false)
                        {
                            results.Add(entry.Key, ComputeProbability(inputTextSelection,entry.Key, knowledge.nGramsPerso,i));
                        }
                        //else
                        //{
                        //    //when ComputeProbability is done, this will be useless
                        //    /results[entry.Key] = (double)results[entry.Key] +(double)(entry.Value.Frequency / knowledge.nGramsPerso[i + 1].dictionary[inputTextSelection].Sum());
                        //}
                        count += 1;
                        if (count >= 20) break;
                    }
                    
                }
            }

            return results;
        }

        public string GetStartOfWord(string userInput)
        {
            string inputText = NormalizeInput(userInput);
            string[] seperators = { " " };
            string[] words = userInput.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            return words.Last();
        }

        public OrderedDictionary GetFirstWords(NgramsParser knowledge)
        {
            //return startgrams.Take(20).ToDictionary();
            OrderedDictionary results = new OrderedDictionary();
            int count = 0;
            foreach(KeyValuePair<string, Sequence> entry in knowledge.nGramsPerso[0].dictionary)
            {
                int sum = knowledge.nGramsPerso[0].Sum();
                int freq = entry.Value.Frequency;
                double value = (double)freq / sum;
                results.Add(entry.Key, value);
                count += 1;
                if (count >= 100) break;
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
