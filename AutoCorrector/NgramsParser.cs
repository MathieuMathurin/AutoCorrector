using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrector
{       
    class NgramsParser
    {        
        public List<NGram> nGramsPerso;
        public List<NGram> nGramsPublic;
        public NGram nGramDebutPhrase;
        
        public void Load()
        {
            LoadPerso();
            LoadPublic();
        }

        private void LoadPerso()
        {            
            this.nGramsPerso = new List<NGram>();
            LoadFromFile(true, "perso_", "grams.txt");
        }

        private void LoadPublic()
        {
            this.nGramsPublic = new List<NGram>();
            LoadFromFile(false, "", "gramstop10k.csv");
        }

        private void LoadFromFile(bool isPerso, string PREFIX, string POSTFIX)
        {
            string[] args = Environment.GetCommandLineArgs();
            string folder = "";
            var n = 1;
            if (isPerso)
            {
                if (args != null && args.Length > 1 && args[1] != null)
                {
                    folder = args[1];
                    //cherche des fichiers contenant un nombre >= 0
                    n = 0;
                }
            }
            else
            {
                if (args != null && args.Length > 2 && args[2] != null)
                {
                    folder = args[2];
                    //cherche des fichiers contenant un nombre >= 1
                    n = 1;
                }
            }                       
            string line;

            while (true)
            {
                var nGram = new NGram();
                // Read the file load it line by line.
                System.IO.StreamReader file;
                var path = Environment.ExpandEnvironmentVariables(folder + PREFIX + n + POSTFIX);
                try
                {
                    file = new System.IO.StreamReader(path);
                }
                catch (FileNotFoundException e)
                {
                    //Load les nGrams en ordre tant qu'il y en a sinon sort.
                    break;
                }
                while ((line = file.ReadLine()) != null)
                {
                    var index = line.LastIndexOf(',');
                    var rawSequence = line.Substring(0, index);
                    var frequency = line.Substring(index + 1); ;
                    String key;
                    var sequence = new Sequence();

                    if (n > 1)
                    {
                        var words = rawSequence.Split(' ');
                        var last = words.Last();
                        key = "";
                        for (var i = 0; i < words.Length - 1; ++i)
                        {
                            key = i > 0 ? key + " " + words[i] : words[i];
                        }

                        if (nGram.dictionary.Keys.Contains(key))
                        {
                            //ajout dans le subdictionary
                            sequence.Frequency = parseFrequency(frequency);
                            if (!nGram.dictionary[key].dictionary.Keys.Contains(last))
                            {
                                nGram.dictionary[key].dictionary.Add(last, sequence);
                            }                            
                        }
                        else
                        {
                            //creation du subdictionary et ajout de l'entree
                            sequence.dictionary = new Dictionary<string, Sequence>();
                            var temp = new Sequence();
                            temp.Frequency = parseFrequency(frequency);
                            if (!sequence.dictionary.Keys.Contains(last))
                            {
                                sequence.dictionary.Add(last, temp);
                            }
                            if (!nGram.dictionary.Keys.Contains(key))
                            {
                                nGram.dictionary.Add(key, sequence);
                            }                            
                        }
                    }
                    else
                    {
                        //Pour l'unigram et l'unigram de debut de phrase
                        key = rawSequence;
                        sequence.Frequency = parseFrequency(frequency);
                        if (!nGram.dictionary.Keys.Contains(key))
                        {
                            nGram.dictionary.Add(key, sequence);
                        }                        
                    }
                }
                file.Close();
                                
                nGram.Sort();
                nGram.Sum();

                if(n == 0)
                {
                    this.nGramDebutPhrase = nGram;
                }
                else
                {
                    if (isPerso)
                    {
                        this.nGramsPerso.Add(nGram);
                    }
                    else
                    {
                        this.nGramsPublic.Add(nGram);
                    }
                }                

                n += 1;
            }
        }

        private int parseFrequency(string frequency)
        {
            Int32 freq;
            Int32.TryParse(frequency, out freq);
            return freq;
        }
    }
}
