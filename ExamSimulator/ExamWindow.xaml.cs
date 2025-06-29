using ExamGenerator.Models;
using ExamSimulator.Database;
using ExamSimulator.ModelView;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ExamSimulator
{
    /// <summary>
    /// Interaction logic for ExamWindow.xaml
    /// </summary>
    public partial class ExamWindow : Window
    {
        private DispatcherTimer examTimer;
        private DispatcherTimer blinkTimer;
        private Exam exam;

        public ExamWindow(Exam exam)
        {
            InitializeComponent();
            this.exam = exam;
            var examVM = (ExamViewModel)this.ExamGrid.DataContext;
            examVM.Exam = exam;
            ExamStatus.Instance.ExamID = exam.ID;
            this.lblExamName.Content = exam.Name;

            var itemMemory = ExamStatus.Instance.CurrentAnswers.FirstOrDefault(t => t.ContainsKey(ExamStatus.Instance.CurrentAnswerIndex + 1));
            if (itemMemory != null)
            {
                examVM.Question = examVM.Exam.Questions[ExamStatus.Instance.CurrentAnswerIndex];
                examVM.ExamIndex = ExamStatus.Instance.CurrentAnswerIndex;
                examVM.TimeLeft = ExamStatus.Instance.TimeLeft;
                examVM.ExamProgressText = $"Pregunta {(examVM.ExamIndex + 1)} de {examVM.Exam.QuestionCount}";
                this.questionItem.SetSelectedAnswers(itemMemory[ExamStatus.Instance.CurrentAnswerIndex + 1]);
            }
            else
            {
                examVM.ExamIndex = 0;
                examVM.Question = exam.Questions[examVM.ExamIndex];
                examVM.ExamProgressText = $"Pregunta {(examVM.ExamIndex + 1)} de {examVM.Exam.QuestionCount}";
                examVM.TimeLeft = exam.Time;
            }

            examTimer = new DispatcherTimer();
            examTimer.Interval = TimeSpan.FromSeconds(1);
            examTimer.Tick += ExamTimer_Tick;
            examTimer.Start();

            blinkTimer = new DispatcherTimer();
            blinkTimer.Interval = TimeSpan.FromSeconds(0.5);
            blinkTimer.Tick += BlinkTimer_Tick;
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            lblTimer.Visibility = lblTimer.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void ExamTimer_Tick(object sender, EventArgs e)
        {
            var examVM = (ExamViewModel)this.ExamGrid.DataContext;
            examVM.TimeLeft = --examVM.TimeLeft;    
            
            if(examVM.TimeLeft >= 0)
            {
                examVM.TimeLeftText = TimeSpan.FromSeconds(examVM.TimeLeft).ToString(@"d\d\ hh\h\ mm\m\ ss\s");
                ExamStatus.Instance.TimeLeft = examVM.TimeLeft;
            }
            else
            {
                examTimer.Stop();
                finish();
            }              
        }

        private void SaveState(bool forward)
        {
            var examVM = (ExamViewModel)this.ExamGrid.DataContext;
            Question question = examVM.Question;
            var item = ExamStatus.Instance.CurrentAnswers.FirstOrDefault(t => t.ContainsKey(question.ID));
            questionItem.ShowAnswer = false;
            if (item == null)
            {
                var selectedAnswers = this.questionItem.GetSelectedAnswers();
                var answer = new Dictionary<int, Dictionary<int, bool>>();
                answer.Add(question.ID, selectedAnswers);
                ExamStatus.Instance.CurrentAnswers.Add(answer);
            }
            else
            {
                this.questionItem.SetSelectedAnswers(item[question.ID]);
            } 

            int index = -1;
            if (forward)
            {
                index = question.ID == examVM.Exam.Questions.Count ? question.ID : question.ID + 1;
                examVM.ExamIndex++;
                if (examVM.ExamIndex < examVM.Exam.Questions.Count)
                {

                    examVM.Question = examVM.Exam.Questions[examVM.ExamIndex];
                }
                else
                {
                    examVM.ExamIndex = examVM.Exam.Questions.Count - 1;
                }
            }
            else
            {
                index = question.ID == 1 ? question.ID : question.ID - 1;
                examVM.ExamIndex--;
                if (examVM.ExamIndex >= 0)
                {
                    examVM.Question = examVM.Exam.Questions[examVM.ExamIndex];
                }
                else
                {
                    examVM.ExamIndex = 0;
                }
            }

            var itemMemory = ExamStatus.Instance.CurrentAnswers.FirstOrDefault(t => t.ContainsKey(index));
            if (itemMemory != null)
            {
                this.questionItem.SetSelectedAnswers(itemMemory[index]);
            }

            examVM.ExamProgressText = $"Pregunta {(examVM.ExamIndex + 1).ToString()} de {examVM.Exam.QuestionCount}";

            ExamStatus.Instance.CurrentAnswerIndex = examVM.ExamIndex;
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            SaveState(false);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            SaveState(true);
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
            MessageBoxResult rsltMessageBox = MessageBox.Show("Do you wish to display the test results?", "Finish test", btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    {
                        examTimer.Stop();
                        finish();
                    }
                    break;
                case MessageBoxResult.No:
                    {
                        examTimer.Stop();
                        MainWindow main = new MainWindow();
                        main.Show();

                        this.Close();
                    } 
                    break;
            }

        }

        public void finish()
        {
            var examVM = (ExamViewModel)this.ExamGrid.DataContext;
            Question question = examVM.Question;
            var item = ExamStatus.Instance.CurrentAnswers.FirstOrDefault(t => t.ContainsKey(question.ID));
            if (item == null)
            {
                var selectedAnswers = this.questionItem.GetSelectedAnswers();
                var answer = new Dictionary<int, Dictionary<int, bool>>();
                answer.Add(question.ID, selectedAnswers);
                ExamStatus.Instance.CurrentAnswers.Add(answer);
            }

            ResultsWindow results = new ResultsWindow(this.exam);
            results.Show();

            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {          
            var examVM = (ExamViewModel)this.ExamGrid.DataContext;
            Question question = examVM.Question;
            var item = ExamStatus.Instance.CurrentAnswers.FirstOrDefault(t => t.ContainsKey(question.ID));
            if (item == null)
            {
                var selectedAnswers = this.questionItem.GetSelectedAnswers();
                var answer = new Dictionary<int, Dictionary<int, bool>>();
                answer.Add(question.ID, selectedAnswers);
                ExamStatus.Instance.CurrentAnswers.Add(answer);
            }
    
            ExamStatus.SaveData();

            MainWindow main = new MainWindow();
            main.Show();

            this.Close();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                examTimer.Stop();
                lblTimer.Foreground = Brushes.Red;
                blinkTimer.Start();
                btnContinue.Visibility =  Visibility.Visible;
                btnPause.Visibility = Visibility.Hidden;
                btnNext.Visibility = Visibility.Hidden;
                btnPrev.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {

            }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                examTimer.Start();
                lblTimer.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x12, 0x2F, 0xE4));
                lblTimer.Visibility = Visibility.Visible;
                blinkTimer.Stop();
                btnContinue.Visibility = Visibility.Hidden;
                btnPause.Visibility = Visibility.Visible;
                btnNext.Visibility = Visibility.Visible;
                btnPrev.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {

            }
        }

        private void btnShowCorrect_Click(object sender, RoutedEventArgs e)
        {
            questionItem.ShowAnswer = true;
        }
    }
}
