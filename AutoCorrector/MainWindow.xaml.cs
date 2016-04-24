using System;
using System.Collections.Generic;
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
        static List<string> suggestions1 = new List<string>();
        static List<string> suggestions2 = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            var parser = new NgramsParser();
            parser.Load();


            //Temporaire
            suggestions1.Add("Allo");
            suggestions1.Add(":)");
            suggestions1.Add("Hey");
            suggestions1.Add("ca");
            suggestions1.Add("Jmen");
            suggestions1.Add("Penses");
            suggestions1.Add("ahah");
            suggestions1.Add("Mon");
            suggestions1.Add("Ma");
            suggestions1.Add("Veux");
            suggestions1.Add("XD");



            suggestions2.Add("va");
            suggestions2.Add("?");
            suggestions2.Add(":O");
            suggestions2.Add("viens");
            suggestions2.Add("tu");
            suggestions2.Add("je");
            suggestions2.Add("ahah");
            suggestions2.Add(":p");



            UpdateSuggestions(null, null);
        }

        static int nbrCalls = 0;
        static List<string> suggestions = suggestions1;

        private void UpdateSuggestions(object sender, TextChangedEventArgs e)
        {
            if(sender != null) nbrCalls++;
            testLabel.Content = "Number of calls to update suggestions done : " + nbrCalls;
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
        //Temporaire, sortir cette methode pour la mettre dans une classe AI.
        private List<string> GetSuggestions()
        {
            if (nbrCalls % 2 == 0)
            {
                return suggestions1;
            }
            return suggestions2;
        }

    
    }

    

}
