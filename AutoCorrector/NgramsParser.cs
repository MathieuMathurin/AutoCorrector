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
        private string PREFIX = "perso_";
        private string POSTFIX = "grams.txt";
        List<Dictionary<String, NGram>> nGrams;
        // todo
        public void Load()
        {
            this.nGrams = new List<Dictionary<string, NGram>>();
            string[] args = Environment.GetCommandLineArgs();
            if(args != null && args.Length > 1 && args[1] != null)
            {
                var folder = args[1];
                var n = 0;
                string line;

                while (true)
                {
                    var nGram = new Dictionary<string, NGram>();
                    // Read the file load it line by line.
                    System.IO.StreamReader file;
                    var path = Environment.ExpandEnvironmentVariables(folder + PREFIX + n + POSTFIX);
                    try
                    {
                        file = new System.IO.StreamReader(path);
                    }
                    catch(FileNotFoundException e)
                    {
                        //Load les nGrams en ordre tant qu'il y en a sinon sort.
                        break;
                    }                    
                    while ((line = file.ReadLine()) != null)
                    {
                        var index = line.LastIndexOf(',');
                        var sequence = line.Substring(0, index);                                       
                        var frequency = line.Substring(index + 1); ;
                        String key;
                        var gram = new NGram();

                        if(n > 1)
                        {
                            var words = sequence.Split(' ');
                            var last = words.Last();
                            key = "";
                            for(var i = 0; i < words.Length - 1; ++i)
                            {
                                key += words[i];
                            }

                            if (nGram.Keys.Contains(key))
                            {
                                //ajout dans le subdictionary
                                gram.Frequency = parseFrequency(frequency);
                                nGram[key].dictionary.Add(last, gram);
                            }
                            else
                            {
                                //creation du subdictionary et ajout de l'entree
                                gram.dictionary = new Dictionary<string, NGram>();
                                var temp = new NGram();
                                temp.Frequency = parseFrequency(frequency);
                                gram.dictionary.Add(last, temp);
                                nGram.Add(key, gram);
                            }                            
                        }
                        else
                        {
                            //Pour l'unigram et l'unigram de debut de phrase
                            key = sequence;
                            gram.Frequency = parseFrequency(frequency);
                            nGram.Add(key, gram);
                        }                                                
                    }
                    file.Close();
                    n += 1;
                    this.nGrams.Add(nGram);
                }                
            }
        }

        //private NGram subDictionnary(string key, string frequency)
        //{

        //}

        private int parseFrequency(string frequency)
        {
            Int32 freq;
            Int32.TryParse(frequency, out freq);
            return freq;
        }
    }
}
