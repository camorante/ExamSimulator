using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Models
{
    public class Exam : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        public int ID
        {
            get { return id; }
            set { 
                id = value;
                OnPropertyChanged("ID");
            }
        }

        private int time;
        public int Time
        {
            get { return time; }
            set { 
                time = value;

                TimeSpan tsp = TimeSpan.FromSeconds(time);

                string timeF = tsp.ToString(@"d\d\ hh\h\ mm\m\ ss\s");
                this.TimeFormat = timeF;

                OnPropertyChanged("Time");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { 
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private List<Question> questions;
        public List<Question> Questions
        {
            get { return questions; }
            set { 
                questions = value;
                QuestionCount = questions.Count;
                OnPropertyChanged("Questions");
            }
        }

        private string timeFormat;

        public string TimeFormat
        {
            get { return timeFormat; }
            set { 
                timeFormat = value;
                OnPropertyChanged("TimeFormat");
            }
        }

        private int questionCount;

        public int QuestionCount
        {
            get { return questionCount; }
            set { 
                questionCount = value;
                OnPropertyChanged("QuestionCount");
            }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { 
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        private bool isSaved;

        public bool IsSaved
        {
            get { return isSaved; }
            set { 
                isSaved = value;
                OnPropertyChanged("IsSaved");
            }
        }


        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
