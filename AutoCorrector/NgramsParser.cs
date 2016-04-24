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
                    try
                    {
                        file = new System.IO.StreamReader(folder + PREFIX + n + POSTFIX);
                    }
                    catch(FileNotFoundException e)
                    {
                        //Load les nGrams en ordre tant qu'il y en a sinon sort.
                        break;
                    }                    
                    while ((line = file.ReadLine()) != null)
                    {
                        var split = line.Split(',');                        
                        var frequency = split[1];
                        String key;
                        var gram = new NGram();
                        if(n > 1)
                        {
                            var words = split[0].Split(' ');
                            var last = words.Last();
                            key = words.TakeWhile(word => word != last).ToString();

                            if (nGram.Keys.Contains(key))
                            {
                                //ajout dans le subdictionary
                            }
                            else
                            {
                                //creation du subdictionary et ajout de l'entree
                            }                            
                        }
                        else
                        {
                            key = split[0];                            
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
