using ExamGenerator.Models;
using ExamSimulator.Controls;
using ExamSimulator.Database;
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
using System.Windows.Shapes;

namespace ExamSimulator
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsWindow(Exam exam)
        {
            InitializeComponent();
            LoadQuestions(exam);
        }

        private void LoadQuestions(Exam exam)
        {
            try
            {
                int validAnswers = 0;
                foreach (var item in exam.Questions)
                {
                    var answer = ExamStatus.Instance.CurrentAnswers.FirstOrDefault(t => t.ContainsKey(item.ID));

                    QuestionItem question = new QuestionItem();
                    question.Question = item;
                    question.ShowAnswer = true;
                    Dictionary<int, bool> currentAnswer = new Dictionary<int, bool>();
                    currentAnswer.Add(1, false);
                    currentAnswer.Add(2, false);
                    currentAnswer.Add(3, false);
                    currentAnswer.Add(4, false);
                    currentAnswer.Add(5, false);
                    if (answer != null)
                    {
                        currentAnswer = answer[item.ID];
                    }
                    question.SetSelectedAnswers(currentAnswer);
                    questionList.Children.Add(question);

                    if (question.QuestionList.IsValid)
                        validAnswers++;
                }
                lbltitle.Content = "Resultados: " + ((validAnswers * 100) / exam.Questions.Count) + "% (" + $"{validAnswers} de {exam.Questions.Count}" +   ")";
            }
            catch (Exception)
            {

            }
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {

            MainWindow main = new MainWindow();
            main.Show();

            this.Close();
        }
    }
}
