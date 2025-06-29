using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSimulator.Model
{
    public class ExamManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int ID;

        public int MyProperty
        {
            get { return ID; }
            set { ID = value; }
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
