using ExamGenerator.Models;
using ExamSimulator.Commands;
using ExamSimulator.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamSimulator.ModelView
{
    public class ExamViewModel : INotifyPropertyChanged
    {

        private Question question;

        public Question Question
        {
            get { return question; }
            set { 
                question = value;
                OnPropertyChanged("Question");
            }
        }

        private Exam exam;

        public Exam Exam
        {
            get { return exam; }
            set { 
                exam = value;
                OnPropertyChanged("Exam");
            }
        }

        private int examIndex;

        public int ExamIndex
        {
            get { return examIndex; }
            set { 
                examIndex = value;
                OnPropertyChanged("ExamIndex");
            }
        }

        private string examProgressText;

        public string ExamProgressText
        {
            get { 
                return examProgressText;             
            }
            set { 
                examProgressText = value;
                OnPropertyChanged("ExamProgressText");
            }
        }

        private int timeLeft;

        public int TimeLeft
        {
            get { return timeLeft; }
            set { 
                timeLeft = value;
                OnPropertyChanged("TimeLeft");
            }
        }

        private string timeLeftText;

        public string TimeLeftText
        {
            get { return timeLeftText; }
            set { 
                timeLeftText = value;
                OnPropertyChanged("TimeLeftText");
            }
        }



        public ExamViewModel()
        {
           
            //NextQuestionCmd = new CommandBase(param => NextQuestion());
            //PrevQuestionCmd = new CommandBase(param => PrevQuestion());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //private ICommand nextQuestionCmd;

        //public ICommand NextQuestionCmd
        //{
        //    get { return nextQuestionCmd; }
        //    set { nextQuestionCmd = value; }
        //}

        //private ICommand prevQuestionCmd;

        //public ICommand PrevQuestionCmd
        //{
        //    get { return prevQuestionCmd; }
        //    set { prevQuestionCmd = value; }
        //}

        //private void NextQuestion()
        //{
        //    ExamIndex++;
        //    if (ExamIndex < exam.Questions.Count)
        //    {

        //        this.Question = exam.Questions[ExamIndex];
        //    }
        //    else
        //    {
        //        ExamIndex = exam.Questions.Count;
        //    }
            
        //}

        //private void PrevQuestion()
        //{
        //    ExamIndex--;
        //    if (ExamIndex >= 0)
        //    {
        //        this.Question = exam.Questions[ExamIndex];
        //    }
        //    else
        //    {
        //        ExamIndex = 0;
        //    }
        //}
    }
}
