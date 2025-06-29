    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSimulator.Model
{
    public class Opinion : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		private string username;
		public string Username
		{
			get { return username; }
			set { 
				username = value;
                OnPropertyChanged("Username");
            }
		}

        private string score;
        public string Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }

        private string createdOn;
        public string CreatedOn
        {
            get { return createdOn; }
            set
            {
                createdOn = value;
                OnPropertyChanged("CreatedOn");
            }
        }

        private string opinionText;
        public string OpinionText
        {
            get { return opinionText; }
            set
            {
                opinionText = value;
                OnPropertyChanged("OpinionText");
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
