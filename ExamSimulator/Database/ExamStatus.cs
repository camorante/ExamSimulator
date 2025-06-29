using ExamSimulator.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExamSimulator.Database
{
    public class ExamStatus
    {
        private static readonly Lazy<ExamStatus> instance = new Lazy<ExamStatus>(() => new ExamStatus());

        public int ExamID { get; set; }
        public List<Dictionary<int, Dictionary<int,bool>>> CurrentAnswers { get; set; }
        public int CurrentAnswerIndex { get; set; }
        public int TimeLeft { get; set; }

        private ExamStatus()
        {
            CurrentAnswers = new List<Dictionary<int, Dictionary<int, bool>>>();
        }

        public static ExamStatus Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private static void SetData(ExamStatus status)
        {
            instance.Value.CurrentAnswerIndex = status.CurrentAnswerIndex;
            instance.Value.CurrentAnswers = status.CurrentAnswers;
            instance.Value.TimeLeft = status.TimeLeft;
        }

        public static void SaveData()
        {
            try
            {
                JObject saveData = JObject.FromObject(ExamStatus.Instance);
                string jsonData = saveData.ToString(Newtonsoft.Json.Formatting.None);

                string encrData = EncryptionUtils.EncryptText(jsonData, "%6Yej$2S");

                string path = AppDomain.CurrentDomain.BaseDirectory;
                //path = path.Substring(6, path.Length - 6);

                if (!Directory.Exists(path + @"\save"))
                {
                    Directory.CreateDirectory(path + @"\save");
                }

                string filename = path + @"\save\" + ExamStatus.Instance.ExamID + ".esv";

                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                File.WriteAllText(filename, encrData);
            }
            catch (Exception)
            {

 
            }
        }

        public static void LoadData(int examID)
        {
            try
            {
                String path = AppDomain.CurrentDomain.BaseDirectory;
                //path = path.Substring(6, path.Length - 6);
                if (Directory.Exists(path + @"\save") && File.Exists(path + @"\save\" + examID + ".esv"))
                {
                    string decrData = EncryptionUtils.DecryptText(File.ReadAllText(path + @"\save\" + examID + ".esv"), "%6Yej$2S");

                    ExamStatus examSt = ExamUtil.DeserializeExamStatus(decrData);
                    SetData(examSt);
                }

            }
            catch (Exception)
            {

            }
        } 

        public static void DeleteData(int examID)
        {
            try
            {
                String path = AppDomain.CurrentDomain.BaseDirectory;

                if (Directory.Exists(path + @"\save") && File.Exists(path + @"\save\" + examID + ".esv"))
                {
                    File.Delete(path + @"\save\" + examID + ".esv");
                    ClearData();
                }
            }
            catch (Exception)
            {

            }
        }

        public static void ClearData()
        {
            try
            {
                ExamStatus.Instance.CurrentAnswerIndex = 0;
                ExamStatus.Instance.CurrentAnswers.Clear();
                ExamStatus.Instance.ExamID = 0;
                ExamStatus.Instance.TimeLeft = 0;
            }
            catch (Exception)
            {

            }
        }
    }
}
