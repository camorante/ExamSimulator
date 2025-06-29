using ExamGenerator.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ExamStatus.ClearData();
            var item = this.ExamGrid.SelectedItem;
            
            if (item != null)
            {
                ExamStatus.DeleteData(((Exam)item).ID);
                ExamWindow examWin = new ExamWindow((Exam)item);
                examWin.Show();

                this.Close();
            }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {        
            var item = this.ExamGrid.SelectedItem;
            
            if (item != null)
            {
                ExamStatus.LoadData(((Exam)item).ID);
                ExamWindow examWin = new ExamWindow((Exam)item);
                examWin.Show();

                this.Close();
            }
        }
    }
}
