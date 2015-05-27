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

namespace ShortAnswerInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* 'composite' is created by checking the user's input against the actual answer.
         * Correct letters in the correct position are displayed as such.
         * Incorrect letters are displayed as dashes.
         */
        String answer;
        String composite;

        public MainWindow()
        {
            InitializeComponent();

            shortAnswer("Who was that one guy?", "Guy Pierce");
        }

        //shortAnswer() sets up the initial question and 'composite' hint
        private void shortAnswer(String question, String answer)
        {
            //Store answer
            this.answer = answer;

            //Display question
            lblQuestion.Content = question;

            //Initialize variable 'temp' for creation of 'composite' hint
            Char[] temp = new Char[answer.Length];

            //Replace alphanumeric characters with dashes; keep spaces intact
            for (int i = 0; i < temp.Length; i++)
            {
                if (answer[i] != ' ')
                    temp[i] = '-';
                else
                    temp[i] = ' ';
            }

            //Display hint
            lblComposite.Content = new String(temp);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Char[] temp = new Char[answer.Length];

            //Compare strings 'answer' and user input.
            for(int i = 0; i < answer.Length; i++)
            {
                if (i < txtInput.Text.Length)
                {
                    //Display correct characters in correct position
                    if (txtInput.Text[i] == answer[i])
                    {
                        temp[i] = answer[i];
                    }
                    //Display spaces unmodified
                    else if (answer[i] == ' ')
                    {
                        temp[i] = ' ';
                    }
                    //Display dashes where input is incorrect
                    else
                    {
                        temp[i] = '-';
                    }
                }
                //If length of actual answer exceeds length of user input, pad with dashes
                else
                    temp[i] = '-';
            }

            lblComposite.Content = new String(temp);
        }
    }
}
