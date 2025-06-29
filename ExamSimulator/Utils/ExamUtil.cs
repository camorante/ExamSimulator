using ExamGenerator.Models;
using ExamSimulator.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSimulator.Utils
{
    public class ExamUtil
    {
        public static Exam DeserializeExam(string data)
        {
            Exam result = new Exam();
            try
            {
                JObject examData = JObject.Parse(data);
                result = examData.ToObject<Exam>();
            }
            catch (Exception)
            {

            }
            return result;
        }

        public static ExamStatus DeserializeExamStatus(string data)
        {
            ExamStatus result = ExamStatus.Instance;
            try
            {
                JObject examData = JObject.Parse(data);
                result = examData.ToObject<ExamStatus>();
            }
            catch (Exception)
            {

            }
            return result;
        }

    }
}
