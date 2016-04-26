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
        public MainWindow()
        {
            InitializeComponent();            

            parser = new NgramsParser();
            parser.Load();
            predictionAI = new TextPredictionAI();
            //suggestions = GetSuggestions();


            UpdateSuggestions(null, null);
        }

        //static int nbrCalls = 0;

        
        private void UpdateSuggestions(object sender, TextChangedEventArgs e)
        {
            
            //if (sender != null) nbrCalls++;
            //testLabel.Content = "Number of calls to update suggestions done : " + nbrCalls;
            suggestionsPanel.Children.Clear();
            suggestions = GetSuggestions();
            foreach (string s in suggestions)
            {
                suggestionsPanel.Children.Add(new Label { Content = s });
            }
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
                userInput.Text += " " + suggestions.First() + " ";
                UpdateSuggestions(null, null);
                e.Handled = true;
                userInput.Select(userInput.Text.Length, 0);
            }else if (e.Key == Key.Enter)
            {
                userInput.Text = "";
                userInput.Select(userInput.Text.Length, 0);
                e.Handled = true;
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
