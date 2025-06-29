using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Models
{
    public class Answer
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
