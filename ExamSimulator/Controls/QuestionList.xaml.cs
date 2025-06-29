using ExamGenerator.Models;
using ExamSimulator.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for QuestionList.xaml
    /// </summary>
    public partial class QuestionList : UserControl
    {
        public static readonly DependencyProperty question =
            DependencyProperty.Register("Question", typeof(Question),
            typeof(QuestionList),
                new FrameworkPropertyMetadata(
                    default(Question),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnUriChanged)
                )
            );

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QuestionList)d).DrawAnswers();
        }

        public Question Question
        {
            get { return (Question)GetValue(question);  }
            set {
                SetValue(question, value);
                //DrawAnswers();
            }
        }

        private bool showAnswer;

        public bool ShowAnswer
        {
            get { return showAnswer; }
            set { showAnswer = value; }
        }

        public QuestionList()
        {
            InitializeComponent();
        }


        public bool IsValid
        {
            get {
                Dictionary<int, bool> selected = SelectedAnswers;
                var questionData = (Question)GetValue(question);
                bool result = true;
                foreach (var item in questionData.Answers)
                {
                    if (selected[item.ID] != item.IsCorrect)
                    {
                        result = false;
                        break;
                    }
                        
                }
                return result; 
            }
        }

        public Dictionary<int, bool> SelectedAnswers
        {
            get {
                var questionData = (Question)GetValue(question);
                var selectedAnswers = new Dictionary<int, bool>();
                foreach (var item in this.AnswerList.Children)
                {
                    
                    switch (questionData.Type)
                    {
                        case Question.QuestionType.Multiple:
                            {
                                bool val = ((AnswerCheckItem)item).IsSelected;
                                int id = ((AnswerCheckItem)item).ID;
                                selectedAnswers.Add(id, val);
                            }
                            
                            break;
                        case Question.QuestionType.Single:
                            {
                                bool val = ((AnswerRadioItem)item).IsSelected;
                                int id = ((AnswerRadioItem)item).ID;
                                selectedAnswers.Add(id, val);
                            }
                            break;
                    }
                }
                return selectedAnswers; 
            }
            set
            {
                var questionData = (Question)GetValue(question);
                foreach (var item in this.AnswerList.Children)
                {
                    switch (questionData.Type)
                    {
                        case Question.QuestionType.Multiple:
                            {
                                int id = ((AnswerCheckItem)item).ID;
                                ((AnswerCheckItem)item).IsSelected = value[id];

                                if (showAnswer)
                                {
                                    var answerData = questionData.Answers.FirstOrDefault(t => t.ID == id);
                                    if (answerData.IsCorrect == value[id])
                                    {
                                        ((AnswerCheckItem)item).AnswerColor = new SolidColorBrush(Colors.LightGreen);
                                        ((AnswerCheckItem)item).FontWeight = FontWeights.Bold;
                                    }
                                    else
                                    {
                                        ((AnswerCheckItem)item).AnswerColor = new SolidColorBrush(Colors.Red);
                                    }
                                   
                                }
                            }

                            break;
                        case Question.QuestionType.Single:
                            {
                                int id = ((AnswerRadioItem)item).ID;
                                ((AnswerRadioItem)item).IsSelected = value[id];

                                if (showAnswer)
                                {
                                    var answerData = questionData.Answers.FirstOrDefault(t => t.ID == id);
                                    if(value[id])
                                        if (answerData.IsCorrect == value[id])
                                        {
                                            ((AnswerRadioItem)item).AnswerColor = new SolidColorBrush(Colors.LightGreen);
                                            ((AnswerRadioItem)item).FontWeight = FontWeights.Bold;
                                        }
                                        else
                                        {
                                            ((AnswerRadioItem)item).AnswerColor = new SolidColorBrush(Colors.Red);
                                        }
                                    else
                                        ((AnswerRadioItem)item).AnswerColor = new SolidColorBrush(Colors.Red);
                                }
                            }
                            break;
                    }
                    
                }
            }
        }


        private void DrawAnswers()
        {
            try
            {
                var questionData = (Question)GetValue(question);
                this.AnswerList.Children.Clear();
                foreach (var answer in questionData.Answers)
                {
                    switch (questionData.Type)
                    {
                        case Question.QuestionType.Multiple:
                            AnswerCheckItem answerCheck = new AnswerCheckItem()
                            {
                                ID = answer.ID,
                                Text = answer.Text,
                                IsCorrect = answer.IsCorrect,
                                Letter = Convert.ToChar(64 + answer.ID).ToString() + ". "
                            };
                            answerCheck.AnswerSelected += AnswerRadio_AnswerSelected;
                            this.AnswerList.Children.Add(answerCheck);
                            break;
                        case Question.QuestionType.Single:
                            AnswerRadioItem answerRadio = new AnswerRadioItem()
                            {
                                ID = answer.ID,
                                Text = answer.Text,
                                IsCorrect = answer.IsCorrect,
                                Letter = Convert.ToChar(64 + answer.ID).ToString() + ". "
                            };
                            answerRadio.AnswerSelected += AnswerRadio_AnswerSelected;
                            this.AnswerList.Children.Add(answerRadio);
                            break;
                    }

                }


            }
            catch (Exception)
            {

            }
        }

        private void AnswerRadio_AnswerSelected(object sender, EventArgs e)
        {
            if(typeof(AnswerRadioItem) == sender.GetType())
            {
                AnswerRadioItem currAnswer = (AnswerRadioItem)sender;
                foreach (var item in this.AnswerList.Children)
                {
                    if (item.GetType() == typeof(AnswerRadioItem) && currAnswer.ID != ((AnswerRadioItem)item).ID)
                    {
                        ((AnswerRadioItem)item).IsSelected = false;
                    }
                }
            }

            var question = ExamStatus.Instance.CurrentAnswers.FirstOrDefault(t => t.ContainsKey(Question.ID));
            ExamStatus.Instance.CurrentAnswers.Remove(question);
        }
    }
}
