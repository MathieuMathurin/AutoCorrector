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


        public List<NGram> GetSuggestions(string userInput, NgramsParser knowledge)
        {
            //Ne comporte pas les cas en milieu de mot
            //
            //Normaliser user input
            //S'il y a rien dans userInput, retourner retourner debuts de phrase les plus frequents

            //S'il y a un n mots checher (n+1)grams,(n)grams,(n-1)grams, etc
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
            List<NGram> result = new List<NGram>();
            for(int i = 0; i<nbrToGet; i++)
            {
                aGram[]
            }
        }

        private double ComputeProbability(string userInput, string word, NgramsParser knowledge)
        {
            double probability = 1;
            string[] userWords = userInput.Split(' ');


        }

    }
}
