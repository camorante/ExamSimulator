using ExamSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Models
{
    public class Question
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public string Explanation { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Opinion> Opinions { get; set; }

        public Question()
        {
            this.Answers = new List<Answer>();
            this.Text = "";
            this.Explanation = "";
            this.Type = QuestionType.Single;
            this.Opinions = new List<Opinion>();
        }

        public enum QuestionType
        {
            Multiple,
            Single
        }
    }
}
