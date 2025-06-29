using ExamGenerator.Models;
using ExamSimulator.Commands;
using ExamSimulator.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExamSimulator.ModelView
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Exam> exams;
        public List<Exam> Exams
        {
            get { return exams; }
            set { 
                exams = value;
                OnPropertyChanged("Exams");
            }
        }

        private object selectedItem;

        public object SelectedItem
        {
            get { return selectedItem; }
            set { 
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }


        private ICommand openExamCmd;

        public ICommand OpenExamCmd
        {
            get { return openExamCmd; }
            set { openExamCmd = value; }
        }

        private ICommand deleteExamCmd;

        public ICommand DeleteExamCmd
        {
            get { return deleteExamCmd; }
            set { deleteExamCmd = value; }
        }


        public MainViewModel()
        {
            OpenExamCmd = new CommandBase(param => GetExam());
            DeleteExamCmd = new CommandBase(param => DeleteExam());
            //exams = new List<Exam>();
            this.LoadFiles();
        }

        public void GetExam()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Exam files (*.exm)|*.exm";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                List<Exam> examsTemp = new List<Exam>(exams);
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        string fileData = EncryptionUtils.DecryptText(File.ReadAllText(filename), "%6Yej$2S");
                        var exam = ExamUtil.DeserializeExam(fileData);
                        FileInfo fInfo = new FileInfo(filename);
                        exam.FileName = fInfo.Name;

                        examsTemp.Add(exam);
                        this.SaveFile(filename);
                    }
                    this.Exams = examsTemp;

                }
            }
            catch (Exception)
            {

            }
        }

        public void DeleteExam()
        {
            try
            {
                var item = (Exam)this.SelectedItem;

                if(item != null)
                {
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                    MessageBoxResult rsltMessageBox = MessageBox.Show("Do you wish to delete this exam?", "Delete exam", btnMessageBox, icnMessageBox);

                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.Yes:
                            {
                                String path = AppDomain.CurrentDomain.BaseDirectory;

                                string filePath = path + @"\exams\" + item.FileName;
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                    this.Exams.Remove(item);
                                    this.Exams = new List<Exam>(this.Exams);
                                }
                            }
                            break;
                    }
                }
                
            }
            catch (Exception)
            {
            }
        }
        private void SaveFile(string file)
        {
            try
            {
                String path = AppDomain.CurrentDomain.BaseDirectory;

                if(!Directory.Exists(path + @"\exams"))
                {
                    Directory.CreateDirectory(path + @"\exams");
                }

                FileInfo fInfo = new FileInfo(file);
                string newFilePath = path + @"\exams\" + fInfo.Name;
                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }

                File.Copy(file, newFilePath);

            }
            catch (Exception)
            {

            }
        }

        private void LoadFiles()
        {
            try
            {
                String path = AppDomain.CurrentDomain.BaseDirectory;
                //path = path.Substring(6, path.Length - 6);

                if (Directory.Exists(path + @"\exams"))
                {
                    var files = Directory.GetFiles(path + @"\exams", "*.exm");
                    List<Exam> examsTemp = new List<Exam>();

                    foreach (var file in files)
                    {
                        string fileData = EncryptionUtils.DecryptText(File.ReadAllText(file), "%6Yej$2S");
                        var exam = ExamUtil.DeserializeExam(fileData);
                        FileInfo fInfo = new FileInfo(file);
                        exam.FileName = fInfo.Name;
                        exam.IsSaved = HasSavedState(exam.ID);
                        examsTemp.Add(exam);


                    }
                    this.Exams = examsTemp;
                }
                if (this.Exams == null)
                    this.Exams = new List<Exam>();
            }
            catch (Exception)
            {

            }
        }


        private bool HasSavedState(int examID)
        {
            bool result = false;
            try
            {
                String path = AppDomain.CurrentDomain.BaseDirectory;
                //path = path.Substring(6, path.Length - 6);
                result = File.Exists(path + @"\save\" + examID + ".esv");
                
            }
            catch (Exception)
            {

            }
            return result;
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
