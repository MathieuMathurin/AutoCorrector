using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace AutoCorrector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NgramsParser parser;
        TextPredictionAI predictionAI;
        List<string> suggestions;
        List<string> completionSuggestions;
        public MainWindow()
        {
            InitializeComponent();            

            parser = new NgramsParser();
            parser.Load();
            predictionAI = new TextPredictionAI();

            UpdateSuggestions(null, null);
        }


        
        private void UpdateSuggestions(object sender, TextChangedEventArgs e)
        {
            //if (sender != null) nbrCalls++;
            //testLabel.Content = "Number of calls to update suggestions done : " + nbrCalls;
            suggestionsPanel.Children.Clear();
            if (IsTypingWord())
            {
                completionSuggestions = new List<string>();
                string typed = predictionAI.GetStartOfWord(userInput.Text);
                foreach(string s in suggestions.Where(x => x.StartsWith(typed))){
                    suggestionsPanel.Children.Add(new Label { Content = s });
                    completionSuggestions.Add(s);
                }
            }
            else
            {
                suggestions = GetSuggestions();
                foreach (string s in suggestions)
                {
                    suggestionsPanel.Children.Add(new Label { Content = s });
                }
            }
        }


        
        private bool IsTypingWord()
        {
            if (userInput.Text == null || userInput.Text.Length == 0) return false;
            char lastChar= userInput.Text.Last();
            return Char.IsLetterOrDigit(lastChar) || lastChar == '\'' || lastChar == '-' || lastChar == '_';
        }

        
        private void SuggestionClicked(object sender, MouseButtonEventArgs e)
        {
            Label source = (Label)e.Source;
            userInput.Text += " " + source.Content.ToString() + " ";
            userInput.Select(userInput.Text.Length, 0); ;
        }

        private void UserInputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if(IsTypingWord() == false)
                {
                    userInput.Text += " " + suggestions.First() + " ";
                }
                else
                {
                    string textCopy = userInput.Text;
                    while (Char.IsLetterOrDigit(textCopy.Last()))
                    {
                        textCopy = textCopy.Substring(0, textCopy.Length-1);
                        if (textCopy.Length == 0) break;
                    }
                    userInput.Text = textCopy + completionSuggestions.First() + " ";
                }
                UpdateSuggestions(null, null);
                e.Handled = true;
                userInput.Select(userInput.Text.Length, 0);
            }
            else if (e.Key == Key.Enter)
            {
                userInput.Text = "";
                e.Handled = true;
                userInput.Select(userInput.Text.Length, 0);
            }
            
        }

        private List<string> GetSuggestions()
        {
            List<string> results = new List<string>();
            OrderedDictionary pairs = predictionAI.GetSuggestions(userInput.Text, this.parser);
            foreach(DictionaryEntry entry in pairs)
            {
                results.Add(entry.Key.ToString());
            }
            return results;

        }

    
    }

    

}
