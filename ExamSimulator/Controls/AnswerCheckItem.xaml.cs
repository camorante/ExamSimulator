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

namespace ExamSimulator.Controls
{
    /// <summary>
    /// Interaction logic for AnswerCheckItem.xaml
    /// </summary>
    public partial class AnswerCheckItem : UserControl
    {
        public event Action<object, EventArgs> AnswerSelected;

        private int id;

        public int ID
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                this.lblAnswer.Text = text;
            }
        }

        private bool isCorrect;

        public bool IsCorrect
        {
            get { return isCorrect; }
            set
            {
                isCorrect = value;
            }
        }

        private string letter;

        public string Letter
        {
            get { 
                return letter; 
            }
            set
            {
                letter = value;
                this.lblLetter.Content = letter;
            }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                this.chkSelector.IsChecked = isSelected;
            }
        }

        public Brush AnswerColor { 
            set {
                this.lblAnswer.Foreground = value;
                this.lblLetter.Foreground = value;
            }
        }

        public AnswerCheckItem()
        {
            InitializeComponent();
        }


        private void chkSelector_Click(object sender, RoutedEventArgs e)
        {
            isSelected = chkSelector.IsChecked.Value;
            if (AnswerSelected != null)
                AnswerSelected.Invoke(this, null);
        }
    }
}
