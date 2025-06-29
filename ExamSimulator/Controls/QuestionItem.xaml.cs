using ExamGenerator.Models;
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
    /// Interaction logic for QuestionItem.xaml
    /// </summary>
    public partial class QuestionItem : UserControl
    {

        public static readonly DependencyProperty question =
            DependencyProperty.Register("Question", typeof(Question),
            typeof(QuestionItem),
                new FrameworkPropertyMetadata(
                    default(Question),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnObjectChanged)
                )
            );

        private static void OnObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QuestionItem)d).SetQuestion();
        }

        public Question Question
        {
            get { return (Question)GetValue(question); }
            set
            {
                SetValue(question, value);
 
            }
        }

        private bool showAnswers;

        public bool ShowAnswer
        {
            get { return showAnswers; }
            set { 
                showAnswers = value;
                AnswerExp.Visibility = showAnswers ? Visibility.Visible : Visibility.Hidden;
                scrViewOpinions.VerticalScrollBarVisibility = showAnswers ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;
            }
        }

        public QuestionItem()
        {
            InitializeComponent();
            AnswerExp.Visibility = showAnswers ? Visibility.Visible : Visibility.Hidden;
        }

        public void SetQuestion()
        {
            try
            {
                var questionData = (Question)GetValue(question);
                this.txtQuestionText.Text = questionData.Text;
                this.QuestionList.Question = questionData;
                this.lblNumber.Content = questionData.ID.ToString() + ". ";
                this.txtExplanation.Text = questionData.Explanation;
                this.ctrOpinions.Question = questionData;
                StringBuilder validResponses = new StringBuilder();

                foreach (var answer in questionData.Answers)
                {
                    if(answer.IsCorrect)
                        validResponses.Append(Convert.ToChar(64 + answer.ID).ToString() + ",");                    
                }

                if(validResponses.Length > 0)
                {
                    validResponses.Remove(validResponses.Length - 1, 1);
                }

                lblRightAnswer.Content = validResponses.ToString();


            }
            catch (Exception ex)
            {
            }
        }

        public Dictionary<int, bool> GetSelectedAnswers()
        {
            Dictionary<int, bool> result = new Dictionary<int, bool>();
            try
            {
                result = this.QuestionList.SelectedAnswers;
            }
            catch (Exception)
            {

            }
            return result;
        }

        public void SetSelectedAnswers( Dictionary<int, bool> selectedAnswers)
        {
            try
            {
                this.QuestionList.ShowAnswer = ShowAnswer;
                this.QuestionList.SelectedAnswers = selectedAnswers;
                
                if (showAnswers)
                {
                    var isValid = this.QuestionList.IsValid;
                    
                    var path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\{(isValid ? "ok.png" : "wrong.png")}";
                    imgIcon.Source = new BitmapImage(new Uri(path));
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
